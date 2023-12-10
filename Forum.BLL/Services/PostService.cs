using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;
using Newtonsoft.Json;
using Forum.Shared.Contracts.Requests;
using Forum.DAL.Models;
using Forum.DAL.Repositories;
using Forum.BLL.Services.Communication;
using Forum.Shared.Contracts;

namespace Forum.BLL.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly IVoteRepository _voteRepository;
        private readonly ICalendarRepository _calendarRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageHostService _imageHostService;
        private readonly ICognitiveService _cognitiveService;
        public PostService(IPostRepository postRepository, IVoteRepository voteRepository, ICalendarRepository calendarRepository, IUnitOfWork unitOfWork, IImageHostService imageHostService, ICognitiveService cognitiveService)
        {
            _postRepository = postRepository;
            _unitOfWork = unitOfWork;
            _voteRepository = voteRepository;
            _calendarRepository = calendarRepository;
            _imageHostService = imageHostService;
            _cognitiveService = cognitiveService;
        }

        public async Task<PostsResponse> GetAllAsync(string postName = null, string threadId = null, PaginationFilter paginationFilter = null, string orderByQueryString = null, string type = null, string daysAtTown = null)
        {
            var posts = await _postRepository.GetFilteredAndPagedFromThreadAsync(postName, threadId, paginationFilter, orderByQueryString, type, daysAtTown);
            var amount = await _postRepository.GetCountOfFilteredPostsInThreadAsync(postName, threadId, type, daysAtTown);
            return new PostsResponse(posts, amount);
        }

        public async Task<PostResponse> GetAsync(string id)
        {
            var existingPost = await _postRepository.GetAsync(id);

            if (existingPost == null)
                return new PostResponse("Post not found.");

            return new PostResponse(existingPost);
        }

        public async Task<int> GetCountOfAllPostsAsync()
        {
            return await _postRepository.GetCountOfAllPostsAsync();
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

        public async Task<PostResponse> AddEventAsync(Event post)
        {
            if (post.ImageLink != null || post.ImageLink != string.Empty)
            {
                var res = await _imageHostService.SaveImageAsync(post.ImageLink);
                if (res == null || !res.success)
                {
                    return new PostResponse("An error accurred when saving image");
                }
                post.ImageLink = res.data.display_url;
#if DEBUG
                post.Tags = await _cognitiveService.TagsFromImage(res.data.display_url);
#endif
            }

            post.Id = Guid.NewGuid().ToString();
            try
            {
                await _postRepository.AddAsync(post);
                await _unitOfWork.CompleteAsync();

                return new PostResponse(post);
            }
            catch (Exception ex)
            {
                return new PostResponse($"An error occurred when saving the post: {ex.Message}");
            }
        }

        public async Task<PostResponse> AddAsync(Post post, PostType postType)
        {
            if (post.ImageLink != null && post.ImageLink != string.Empty)
            {
                var res = await _imageHostService.SaveImageAsync(post.ImageLink);
                if (res == null || !res.success)
                {
                    return new PostResponse("An error accurred when saving image");
                }
                post.ImageLink = res.data.display_url;
#if DEBUG
                post.Tags = await _cognitiveService.TagsFromImage(res.data.display_url);
#endif
            }

            post.Id = Guid.NewGuid().ToString();
            try
            {
                if (postType == PostType.Event)
                    await _postRepository.AddAsync((Event)post);
                else
                    await _postRepository.AddAsync((Place)post);
                await _unitOfWork.CompleteAsync();

                return new PostResponse(post);
            }
            catch (Exception ex)
            {
                return new PostResponse($"An error occurred when saving the post: {ex.Message}");
            }
        }

        public async Task<bool> UserOwnsPostAsync(string PostId, string UserId)
        {
            var post = await _postRepository.UserOwnsPostAsync(PostId, UserId);

            if (post == null)
                return false;
            if (post.UserId != UserId)
                return false;

            return true;
        }

        public async Task<VoteResponse> Vote(string PostId, string UserId)
        {
            var post = await _postRepository.GetAsync(PostId);

            if (post == null)
                return new VoteResponse("Post not found.");

            var vote = await _voteRepository.FindInstance(PostId, UserId);
            if (vote != null)
                return new VoteResponse("You already voted.");

            var Vote = new Vote { PostId = PostId, UserId = UserId };
            Vote.Id = Guid.NewGuid().ToString();
            try
            {
                await _voteRepository.AddAsync(Vote);
                post.Rating += 1;
                await _unitOfWork.CompleteAsync();

                return new VoteResponse(Vote);
            }
            catch (Exception ex)
            {
                return new VoteResponse($"An error occurred when saving the vote: {ex.Message}");
            }
        }

        public async Task<VoteResponse> UnVote(string PostId, string UserId)
        {
            var post = await _postRepository.GetAsync(PostId);

            if (post == null)
                return new VoteResponse("Post not found.");

            var vote = await _voteRepository.FindInstance(PostId, UserId);
            if (vote == null)
                return new VoteResponse("You did not vote.");

            try
            {
                _voteRepository.Remove(vote);
                post.Rating -= 1;
                await _unitOfWork.CompleteAsync();

                return new VoteResponse(vote);
            }
            catch (Exception ex)
            {
                return new VoteResponse($"An error occurred when deleting the vote: {ex.Message}");
            }

        }

        public async Task<CalendarResponse> AddToCalendar(string PostId, string UserId)
        {
            var post = await _postRepository.GetAsync(PostId);

            if (post == null)
                return new CalendarResponse("Event not found.");

            if (post.GetType() != typeof(Event))
                return new CalendarResponse("This post is not event.");

            var calendar = await _calendarRepository.FindInstance(PostId, UserId);
            if (calendar != null)
                return new CalendarResponse("You already added to calendar.");

            var Calendar = new Calendar { EventId = PostId, UserId = UserId };
            Calendar.Id = Guid.NewGuid().ToString();
            try
            {
                await _calendarRepository.AddAsync(Calendar);
                await _unitOfWork.CompleteAsync();

                return new CalendarResponse(Calendar);
            }
            catch (Exception ex)
            {
                return new CalendarResponse($"An error occurred when saving the event to calendar: {ex.Message}");
            }
        }

        public async Task<CalendarResponse> RemoveFromCalendar(string PostId, string UserId)
        {
            var post = await _postRepository.GetAsync(PostId);

            if (post == null)
                return new CalendarResponse("Event not found.");

            var calendar = await _calendarRepository.FindInstance(PostId, UserId);
            if (calendar == null)
                return new CalendarResponse("This event is not in your calendar.");

            try
            {
                _calendarRepository.Remove(calendar);
                await _unitOfWork.CompleteAsync();

                return new CalendarResponse(calendar);
            }
            catch (Exception ex)
            {
                return new CalendarResponse($"An error occurred when removing event from calendar: {ex.Message}");
            }
        }
    }
}
