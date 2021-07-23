using Forum.Contracts.Responses;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace Forum.Contracts
{

    /// <summary>
    /// Generic client class that interfaces .NET Standard/Blazor with SignalR Javascript client
    /// </summary>
    public class ChatClient : IAsyncDisposable
    {
        public const string HUBURL = "/ChatHub";

        private readonly string _hubUrl;
        private HubConnection _hubConnection;

        public ChatClient(string chatId, string siteUrl)
        {
            // check inputs
            if (string.IsNullOrWhiteSpace(chatId))
                throw new ArgumentNullException(nameof(chatId));
            if (string.IsNullOrWhiteSpace(siteUrl))
                throw new ArgumentNullException(nameof(siteUrl));
            // save username
            _chatId = chatId;
            // set the hub URL
            _hubUrl = siteUrl.TrimEnd('/') + HUBURL;
        }

        private readonly string _chatId;

        private bool _started = false;

        public async Task StartAsync(Func<Task<string>> getToken)
        {
            if (!_started)
            {
                // create the connection using the .NET SignalR client
                _hubConnection = new HubConnectionBuilder()
                    .WithUrl(_hubUrl, options =>
                    {
                        options.AccessTokenProvider = async () => await getToken();
                    })
                    .Build();
                Console.WriteLine("ChatClient: calling Start()");

                // add handler for receiving messages
                _hubConnection.On<MessageResponse>(Messages.RECEIVE, (messageResponse) =>
                 {
                     HandleReceiveMessage(messageResponse);
                 });

                // start the connection
                await _hubConnection.StartAsync();

                Console.WriteLine($"ChatClient: Start returned:  {_hubConnection.State.ToString()}");
                _started = true;

                // register user on hub to let other clients know they've joined
                await _hubConnection.SendAsync(Messages.REGISTER, _chatId);
            }
        }

        private void HandleReceiveMessage(MessageResponse messageResponse)
        {
            // raise an event to subscribers
            MessageReceived?.Invoke(this, new MessageReceivedEventArgs(messageResponse));
        }

        public event MessageReceivedEventHandler MessageReceived;

        public async Task SendAsync(string message)
        {
            // check we are connected
            if (!_started)
                throw new InvalidOperationException("Client not started");
            // send the message
            await _hubConnection.SendAsync(Messages.SEND, _chatId, message);
        }

        public async Task StopAsync()
        {
            if (_started)
            {
                // disconnect the client
                await _hubConnection.StopAsync();
                // There is a bug in the mono/SignalR client that does not
                // close connections even after stop/dispose
                // see https://github.com/mono/mono/issues/18628
                // this means the demo won't show "xxx left the chat" since 
                // the connections are left open
                await _hubConnection.DisposeAsync();
                _hubConnection = null;
                _started = false;
            }
        }

        public async ValueTask DisposeAsync()
        {
            Console.WriteLine("ChatClient: Disposing");
            await StopAsync();
        }
    }

    public delegate void MessageReceivedEventHandler(object sender, MessageReceivedEventArgs e);

    public class MessageReceivedEventArgs : EventArgs
    {
        public MessageReceivedEventArgs(MessageResponse messageResponse)
        {
            this.messageResponse = messageResponse;
        }

        public MessageResponse messageResponse { get; set; }
    }

}

