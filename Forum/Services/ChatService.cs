using Forum.DAL.Models;
using Forum.DAL.Repositories;
using Forum.Services.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Services
{
    public class ChatService : IChatService
    {
        private readonly IChatRepository _chatRepository;
        private readonly IUnitOfWork _unitOfWork;
        public ChatService(IChatRepository chatRepository, IUnitOfWork unitOfWork)
        {
            _chatRepository = chatRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ChatsResponse> GetAllAsync()
        {
            var chats = await _chatRepository.GetAllAsync();
            return new ChatsResponse(chats);
        }

        public async Task<ChatsResponse> GetAllFromUserAsync(string id)
        {
            var chats = await _chatRepository.GetAllFromUserAsync(id);
            return new ChatsResponse(chats);
        }

        public async Task<ChatResponse> GetAsync(string id, string userId)
        {
            var chat = await _chatRepository.GetAsync(id);
            if (chat == null)
                return new ChatResponse("Chad does not exist!");
            else if(chat.Users.Any(u => u.UserId == userId))
                return new ChatResponse(chat);
            return new ChatResponse("You are not participant of this chat!");
        }

        public async Task<ChatResponse> AddAsync(Chat chat)
        {
            chat.Id = Guid.NewGuid().ToString();
            foreach(var u in chat.Users)
                u.Id = Guid.NewGuid().ToString();

            try
            {
                await _chatRepository.AddAsync(chat);
                await _unitOfWork.CompleteAsync();

                return new ChatResponse(chat);
            }
            catch(Exception ex)
            {
                return new ChatResponse($"An error occurred when saving new chat: {ex.Message}");
            }
        }

        public async Task<ChatResponse> RemoveAsync(string id, string userId)
        {
            var existingChat = await _chatRepository.GetAsync(id);

            if (existingChat == null)
                return new ChatResponse("Chat not found.");

            if(!existingChat.Users.Any(u => u.UserId == userId))
                return new ChatResponse("You are not participant of this chat!");

            try
            {
                _chatRepository.Remove(existingChat);
                await _unitOfWork.CompleteAsync();

                return new ChatResponse(existingChat);
            }
            catch(Exception ex)
            {
                return new ChatResponse($"An error occurred when deleting chat: {ex.Message}");
            }
            
        }

        public async Task<ChatResponse> UpdateAsync(string id, string userId, Chat chat)
        {
            var existingChat = await _chatRepository.GetAsync(id);

            if (existingChat == null)
                return new ChatResponse("Chat not found.");

            if(!chat.Users.Any(u => u.UserId == userId))
                return new ChatResponse("You are not participant of this chat!");

            existingChat.Name = chat.Name ?? existingChat.Name;

            try
            {
                _chatRepository.Update(existingChat);
                await _unitOfWork.CompleteAsync();

                return new ChatResponse(existingChat);
            }
            catch (Exception ex)
            {
                return new ChatResponse($"An error occurred when updating the chat: {ex.Message}");
            }
        }
    }
}
