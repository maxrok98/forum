using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forum.Services.Communication;
using Forum.DAL.Models;
using Forum.DAL.Repositories;

namespace Forum.Services
{
    public class ThreadService : IThreadService
    {
        private readonly IThreadRepository _threadRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IImageHostService _imageHostService;
        public ThreadService(IThreadRepository threadRepository, ISubscriptionRepository subscriptionRepository, IUnitOfWork unitOfWork, IImageHostService imageHostService)
        {
            _threadRepository = threadRepository;
            _unitOfWork = unitOfWork;
            _subscriptionRepository = subscriptionRepository;
            _imageHostService = imageHostService;
        }
        public async Task<ThreadResponse> RemoveAsync(string id)
        {
            var existingThread = await _threadRepository.GetAsync(id);

            if (existingThread == null)
                return new ThreadResponse("Thread not found");

            try
            {
                _threadRepository.Remove(existingThread);
                await _unitOfWork.CompleteAsync();

                return new ThreadResponse(existingThread);
            }
            catch(Exception ex)
            {
                return new ThreadResponse($"An error occurred when deleting the thread: {ex.Message}");
            }
        }

        public async Task<IEnumerable<Thread>> GetAllAsync()
        {
            return await _threadRepository.GetAllAsync();
        }

        public async Task<Thread> GetAsync(string id)
        {
            return await _threadRepository.GetAsync(id);
        }

        public async Task<ThreadResponse> AddAsync(Thread thread)
        {
            if(thread.ImageLink != null || thread.ImageLink != String.Empty)
            {
                var res = await _imageHostService.SaveImageAsync(thread.ImageLink);
                if (res == null || !res.success)
                    return new ThreadResponse("Could not store image");
                thread.ImageLink = res.data.display_url;
            }
            thread.Id = Guid.NewGuid().ToString();
            try
            {
                await _threadRepository.AddAsync(thread);
                await _unitOfWork.CompleteAsync();

                return new ThreadResponse(thread);
            }
            catch (Exception ex)
            {
                return new ThreadResponse($"An error occurred when saving the thread: {ex.Message}");
            }
        }

        public async Task<ThreadResponse> UpdateAsync(string id, Thread thread)
        {
            var existingThread = await _threadRepository.GetAsync(id);

            if (existingThread == null)
                return new ThreadResponse("Thread not found.");

            existingThread.Name = thread.Name ?? existingThread.Name;
            existingThread.Description = thread.Description ?? existingThread.Description;

            try
            {
                _threadRepository.Update(existingThread);
                await _unitOfWork.CompleteAsync();

                return new ThreadResponse(existingThread);
            }
            catch (Exception ex)
            {
                return new ThreadResponse($"An error occurred when updating the thread: {ex.Message}");
            }

        }

        public async Task<SubscriptionResponse> Subscribe(string userId, string threadId)
        {
            var thread = await _threadRepository.GetAsync(threadId);

            if (thread == null)
                return new SubscriptionResponse("Thread not found.");

            var sub = await _subscriptionRepository.FindInstance(userId, threadId);
            if (sub != null)
                return new SubscriptionResponse("You already subscribed.");

            var Sub = new Subscription { ThreadId = threadId, UserId = userId, Id = Guid.NewGuid().ToString() };
            try
            {
                await _subscriptionRepository.AddAsync(Sub);
                await _unitOfWork.CompleteAsync();

                return new SubscriptionResponse(Sub);
            }
            catch(Exception ex)
            {
                return new SubscriptionResponse($"An error occurred when saving the subscription: {ex.Message}");
            }

        }

        public async Task<SubscriptionResponse> UnSubscribe(string userId, string threadId)
        {
            var thread = await _threadRepository.GetAsync(threadId);

            if (thread == null)
                return new SubscriptionResponse("Thread not found.");

            var sub = await _subscriptionRepository.FindInstance(userId, threadId);
            if (sub == null)
                return new SubscriptionResponse("You are not subscribed.");

            try
            {
                _subscriptionRepository.Remove(sub);
                await _unitOfWork.CompleteAsync();

                return new SubscriptionResponse(sub);
            }
            catch(Exception ex)
            {
                return new SubscriptionResponse($"An error occurred when deleting the subscription: {ex.Message}");
            }
        }
    }
}
