using Forum.DAL.Models;
using Forum.DAL.Repositories;
using Forum.Services.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IUnitOfWork _unitOfWork;
        public MessageService(IMessageRepository messageRepository, IUnitOfWork unitOfWork)
        {
            _messageRepository = messageRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<MessageResponse> AddAsync(Message message)
        {
            message.Id = Guid.NewGuid().ToString();
            try
            {
                await _messageRepository.AddAsync(message);
                await _unitOfWork.CompleteAsync();

                return new MessageResponse(message);
            }
            catch(Exception ex)
            {
                return new MessageResponse($"An error occurred when saving the message: {ex.Message}");
            }
        }
    }
}
