using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forum.Models;
using Forum.Services.Communication;
using Forum.Repositories;

namespace Forum.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly IUnitOfWork _unitOfWork;
        public PostService(IPostRepository postRepository, IUnitOfWork unitOfWork)
        {
            _postRepository = postRepository;
            _unitOfWork = unitOfWork;

        }

        public async Task<IEnumerable<Post>> GetAllAsync()
        {
            return await _postRepository.GetAllAsync(); 
        }

        public async Task<Post> GetAsync(string id)
        {
            return await _postRepository.GetAsync(id);
        }

        public async Task<IEnumerable<Post>> GetOrderByDateAsync()
        {
            return await _postRepository.GetOrderByDateAsync();
        }

        public async Task<IEnumerable<Post>> GetOrderByVoteAsync()
        {
            return await _postRepository.GetOrderByVoteAsync(); 
        }

        public async Task<PostResponse> RemoveAsync(string id)
        {
            var existingPost = await _postRepository.GetAsync(id);

            if (existingPost == null)
                return new PostResponse("Post not found.");

            try
            {
                _postRepository.Remove(existingPost);
                await _unitOfWork.CompleteAsync();

                return new PostResponse(existingPost);
            }
            catch (Exception ex)
            {
                return new PostResponse($"An error occurred when deleting the post: {ex.Message}");
            }
        }

        public async Task<PostResponse> UpdateAsync(string id, Post post)
        {
            var existingPost = await _postRepository.GetAsync(id);

            if (existingPost == null)
                return new PostResponse("Post not found.");

            existingPost.Name = post.Name ?? existingPost.Name;
            existingPost.Content = post.Content ?? existingPost.Content;
            existingPost.ImageId = post.ImageId ?? existingPost.ImageId;

            try
            {
                _postRepository.Update(existingPost);
                await _unitOfWork.CompleteAsync();

                return new PostResponse(existingPost);
            }
            catch (Exception ex)
            {
                return new PostResponse($"An error occurred when updating the post: {ex.Message}");
            }
            
        }

        public async Task<PostResponse> AddAsync(Post post)
        {
            try
            {
                await _postRepository.AddAsync(post);
                await _unitOfWork.CompleteAsync();

                return new PostResponse(post);
            }
            catch(Exception ex)
            {
                return new PostResponse($"An error occurred when saving the post: {ex.Message}");
            }
        }
    }
}
