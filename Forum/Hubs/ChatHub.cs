using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forum.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Forum.Services;
using Forum.Contracts.Responses;
using AutoMapper;
using Forum.DAL.Models;

namespace Forum.Hubs
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ChatHub : Hub
    {
        private readonly IUserService _userService;
        private readonly IChatService _chatService;
        private readonly IMessageService _messageService;
        private readonly IMapper _mapper;

        private static readonly Dictionary<string, List<UserConnection>> userLookup = new Dictionary<string, List<UserConnection>>();

        private class UserConnection
        {
            public UserShortResponse User { get; set; }
            public string ConnectionId { get; set; }
        }

        public ChatHub(IUserService userService, IChatService chatService, IMessageService messageService, IMapper mapper)
        {
            _userService = userService;
            _chatService = chatService;
            _messageService = messageService;
            _mapper = mapper;
        }

        public async Task SendMessage(string chatId, string message)
        {
            var userId = Context.User.Claims.Single(x => x.Type == "id").Value;
            var connections = userLookup[chatId].Where(x => x.User.Id != userId).Select(x => x.ConnectionId).ToList();
            var user = userLookup[chatId].Where(x => x.User.Id == userId).Select(x => x.User).FirstOrDefault();
            var messageResponse = new MessageResponse() { Date = DateTime.Now, Id = Guid.NewGuid().ToString(), Text = message, User = user };
            var respose = await _messageService.AddAsync(new Message() { ChatId = chatId, UserId = userId, Text = message, Date = DateTime.Now });
            if (respose.Success)
                await Clients.Clients(connections).SendAsync(Messages.RECEIVE, messageResponse);
        }

        public async Task Register(string chatId)
        {
            var userId = Context.User.Claims.Single(x => x.Type == "id").Value;
            var user = await _userService.GetAsync(userId);
            if(!user.Chats.Any(x => x.ChatId == chatId))
            {
                return; // handle in case user is not belong to this chat
            }
            var currentId = Context.ConnectionId;
            var userShort = _mapper.Map<User, UserShortResponse>(user);
            if (!userLookup.ContainsKey(chatId))
            {
                // maintain a lookup of connectionId-to-username
                userLookup.Add(chatId, new List<UserConnection>() { new UserConnection() { ConnectionId = currentId, User = userShort } });
            }
            else
            {
                userLookup[chatId].Add(new UserConnection() { ConnectionId = currentId, User = userShort});
            }
            // re-use existing message for now
            var connections = userLookup[chatId].Where(x => x.User.Id != userId).Select(x => x.ConnectionId).ToList();
            var messageResponse = new MessageResponse() { Date = DateTime.Now, Id = Guid.NewGuid().ToString(), Text = $"{user.UserName} joined the chat", User = userShort };
            await Clients.Clients(connections).SendAsync(
                Messages.RECEIVE,
                messageResponse);
        }

        public override Task OnConnectedAsync()
        {
            Console.WriteLine("Connected");
            return base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception e)
        {
            Console.WriteLine($"Disconnected {e?.Message} {Context.ConnectionId}");
            // try to get connection
            string id = Context.ConnectionId;
            var chatConnection = userLookup.Where(x => x.Value.Any(y => y.ConnectionId == id)).FirstOrDefault();

            //if (!userLookup.TryGetValue(id, out string username))
            //    username = "[unknown]";

            var userConnection = chatConnection.Value.Where(x => x.ConnectionId == id).FirstOrDefault();
            userLookup[chatConnection.Key].Remove(userConnection);
            if(userLookup[chatConnection.Key].Count == 0)
                userLookup.Remove(chatConnection.Key);

            var messageResponse = new MessageResponse() { Date = DateTime.Now, Id = Guid.NewGuid().ToString(), Text = $"{userConnection.User.UserName} has left the chat", User = userConnection.User };
            var connectionsInChat = chatConnection.Value.Where(y => y.ConnectionId != id).Select(x => x.ConnectionId).ToList();
            await Clients.Clients(connectionsInChat).SendAsync(
                Messages.RECEIVE,
                messageResponse);
            await base.OnDisconnectedAsync(e);
        }

    }
}
