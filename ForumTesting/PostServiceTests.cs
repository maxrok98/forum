using Forum.Services.Communication;
using Forum.Models;
using Forum.Repositories;
using Forum.Services;
using Microsoft.AspNetCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ForumTesting
{
    public class PostServiceTests
    {
        private readonly PostService _sut;
        private readonly Mock<IPostRepository> _postRepoMock = new Mock<IPostRepository>();
        private readonly Mock<IVoteRepository> _voteRepoMock = new Mock<IVoteRepository>();
        private readonly Mock<IUnitOfWork> _unitRepoMock = new Mock<IUnitOfWork>();
        private readonly Mock<IImageHostService> _imageHostService = new Mock<IImageHostService>();

        public PostServiceTests()
        {
            _sut = new PostService(_postRepoMock.Object, _voteRepoMock.Object, _unitRepoMock.Object, _imageHostService.Object);
        }

        [Fact]
        public async Task GetAsync_ShouldReturnPost_WhenPostExist()
        {
            //Arrange
            var id = Guid.NewGuid().ToString();
            var name = "Post for test";
            var content = "Content for testing";
            var post = new Post
            {
                Id = id,
                Name = name,
                Content = content
            };
            _postRepoMock.Setup(x => x.GetAsync(id)).ReturnsAsync(post);

            //Act
            PostResponse postToTest = await _sut.GetAsync(id);

            //Assert
            Assert.Equal(id, postToTest.Resource.Id);
            Assert.Equal(name, postToTest.Resource.Name);
            Assert.Equal(content, postToTest.Resource.Content);
        }

        [Fact]
        public async Task GetAsync_ShouldReturnNull_WhenPostDoesNotExist()
        {
            //Arrange
            _postRepoMock.Setup(x => x.GetAsync(It.IsAny<Guid>().ToString())).Returns(() => null);

            //Act
            PostResponse postToTest = await _sut.GetAsync(Guid.NewGuid().ToString());

            //Assert
            Assert.False(postToTest.Success);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnResponseSuccessTrue_WhenPostAdded()
        {
            //Arange
            //_postRepoMock.Setup(x => x.AddAsync())
            var id = Guid.NewGuid().ToString();
            var name = "Post for test";
            var content = "Content for testing";
            var post = new Post
            {
                Id = id,
                Name = name,
                Content = content
            };

            //Act
            PostResponse pr = await _sut.AddAsync(post);

            //Assert
            Assert.True(pr.Success);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnResponseSeccessFalse_WhenPostDoesNotAdded()
        {
            //Arrange
            var id = Guid.NewGuid().ToString();
            var name = "Post for test";
            var content = "Content for testing";
            var post = new Post
            {
                Id = id,
                Name = name,
                Content = content
            };
            _postRepoMock.Setup(x => x.AddAsync(post)).Throws(new Exception());

            //Act
            PostResponse pr = await _sut.AddAsync(post);

            //Assert
            Assert.False(pr.Success);
        }

        [Fact]
        public async Task AddAsync_ResponseMustMuch()
        {
            //Arange
            var id = Guid.NewGuid().ToString();
            var name = "Post for test";
            var content = "Content for testing";
            var post = new Post
            {
                Id = id,
                Name = name,
                Content = content
            };

            //Act
            PostResponse pr = await _sut.AddAsync(post);

            //Assert
            Assert.Same(post, pr.Resource);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnResponseSeccessTrue_WhenPostUpdated()
        {
            //Arange
            var id = Guid.NewGuid().ToString();
            var name = "Post for test";
            var content = "Content for testing";
            var post = new Post
            {
                Id = id,
                Name = name,
                Content = content
            };
            name = "Name to update";
            content = "Content to update";
            var postU = new Post
            {
                Name = name,
                Content = content
            };
            _postRepoMock.Setup(x => x.GetAsync(id)).ReturnsAsync(post);

            //Act
            PostResponse pr = await _sut.UpdateAsync(id, postU);

            //Assert
            Assert.True(pr.Success);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnResponseSeccessFalse_WhenPostDoesNotExist()
        {
            //Arange
            var id = Guid.NewGuid().ToString();
            var name = "Post for test";
            var content = "Content for testing";
            var post = new Post
            {
                Id = id,
                Name = name,
                Content = content
            };
            name = "Name to update";
            content = "Content to update";
            var postU = new Post
            {
                Name = name,
                Content = content
            };
            _postRepoMock.Setup(x => x.GetAsync(id)).ReturnsAsync(() => null);

            //Act
            PostResponse pr = await _sut.UpdateAsync(id, postU);

            //Assert
            Assert.False(pr.Success);
        }

        [Fact]
        public async Task UpdateAsync_ShouldMuch_WhenPostUpdated()
        {
            //Arange
            var id = Guid.NewGuid().ToString();
            var name = "Post for test";
            var content = "Content for testing";
            var post = new Post
            {
                Id = id,
                Name = name,
                Content = content
            };
            name = "Name to update";
            content = "Content to update";
            var postU = new Post
            {
                Name = name,
                Content = content
            };
            _postRepoMock.Setup(x => x.GetAsync(id)).ReturnsAsync(post);

            //Act
            PostResponse pr = await _sut.UpdateAsync(id, postU);

            //Assert
            Assert.Equal(name, pr.Resource.Name);
            Assert.Equal(content, pr.Resource.Content);
        }

        [Fact]
        public async Task RemoveAsync_ShoulReturnResponseSeccessTrue_WhenPostExistAndDeleted()
        {
            //Arange
            var id = Guid.NewGuid().ToString();
            var name = "Post for test";
            var content = "Content for testing";
            var post = new Post
            {
                Id = id,
                Name = name,
                Content = content
            };
            _postRepoMock.Setup(x => x.GetAsync(id)).ReturnsAsync(post);

            //Act
            PostResponse pr = await _sut.RemoveAsync(id);

            //Assert
            Assert.True(pr.Success);
        }

        [Fact]
        public async Task RemoveAsync_ShoulReturnResponseSeccessFalse_WhenPostDoesNotExist()
        {
            //Arange
            var id = Guid.NewGuid().ToString();
            _postRepoMock.Setup(x => x.GetAsync(It.IsAny<Guid>().ToString())).Returns(() => null);

            //Act
            PostResponse pr = await _sut.RemoveAsync(id);

            //Assert
            Assert.False(pr.Success);
        }

        [Fact]
        public async Task UserOwnsPostAsync_ShouldReturnFalse_WhenPostDoesNotExist()
        {
            //Arrange
            var postId = Guid.NewGuid().ToString();
            var userId = Guid.NewGuid().ToString();
            _postRepoMock.Setup(x => x.UserOwnsPostAsync(postId, userId)).ReturnsAsync(() => null);

            //Act
            bool result = await _sut.UserOwnsPostAsync(postId, userId);

            //Assert
            Assert.False(result);
        }

        [Fact]
        public async Task UserOwnsPostAsync_ShouldReturnTrue_WhenUserOwnsPost()
        {
            //Arange
            var userId = Guid.NewGuid().ToString();
            var id = Guid.NewGuid().ToString();
            var name = "Post for test";
            var content = "Content for testing";
            var post = new Post
            {
                Id = id,
                Name = name,
                Content = content,
                UserId = userId
            };
            _postRepoMock.Setup(x => x.UserOwnsPostAsync(id, userId)).ReturnsAsync(post);

            //Act
            bool result = await _sut.UserOwnsPostAsync(id, userId);

            //Assert
            Assert.True(result);
        }

        [Fact]
        public async Task Vote_ShouldReturnResponseSeccessFalse_WhenPostDoesNotExist()
        {
            //Arange
            var postId = Guid.NewGuid().ToString();
            var userId = Guid.NewGuid().ToString();
            _postRepoMock.Setup(x => x.GetAsync(postId)).ReturnsAsync(() => null);

            //Act
            VoteResponse vr = await _sut.Vote(postId, userId);

            //Assert
            Assert.False(vr.Success);
            Assert.Equal("Post not found.", vr.Message);
        }

        [Fact]
        public async Task Vote_ShouldReturnResponseSeccessFalse_WhenUserAlreadyVoted()
        {
            //Arange
            var postId = Guid.NewGuid().ToString();
            var userId = Guid.NewGuid().ToString();
            Vote vote = new Vote
            {
                Id = Guid.NewGuid().ToString(),
                UserId = userId,
                PostId = postId
            };
            _postRepoMock.Setup(x => x.GetAsync(postId)).ReturnsAsync(new Post());
            _voteRepoMock.Setup(x => x.FindInstance(postId, userId)).ReturnsAsync(vote);

            //Act
            VoteResponse vr = await _sut.Vote(postId, userId);

            //Assert
            Assert.False(vr.Success);
            Assert.Equal("You already voted.", vr.Message);
        }

        [Fact]
        public async Task Vote_ShouldReturnResponseSuccessTrue_WhenUserSuccessfulyVote()
        {
            //Arange
            var postId = Guid.NewGuid().ToString();
            var userId = Guid.NewGuid().ToString();
            Vote vote = new Vote
            {
                Id = Guid.NewGuid().ToString(),
                UserId = userId,
                PostId = postId
            };
            _postRepoMock.Setup(x => x.GetAsync(postId)).ReturnsAsync(new Post());
            _voteRepoMock.Setup(x => x.FindInstance(postId, userId)).ReturnsAsync(() => null);

            //Act
            VoteResponse vr = await _sut.Vote(postId, userId);

            //Assert
            Assert.True(vr.Success);
            Assert.Equal(postId, vr.Resource.PostId);
            Assert.Equal(userId, vr.Resource.UserId);
        }

        [Fact]
        public async Task UnVote_ShouldReturnResponceSuccessFalse_WhenPostDoesNotExist()
        {
            //Arange
            var postId = Guid.NewGuid().ToString();
            var userId = Guid.NewGuid().ToString();
            _postRepoMock.Setup(x => x.GetAsync(postId)).ReturnsAsync(() => null);

            //Act
            VoteResponse vr = await _sut.UnVote(postId, userId);

            //Assert
            Assert.False(vr.Success);
            Assert.Equal("Post not found.", vr.Message);
        }

        [Fact]
        public async Task UnVote_ShouldReturnResponseSuccessFalse_WhenUserDidNotVote()
        {
            //Arange
            var postId = Guid.NewGuid().ToString();
            var userId = Guid.NewGuid().ToString();
            Vote vote = new Vote
            {
                Id = Guid.NewGuid().ToString(),
                UserId = userId,
                PostId = postId
            };
            _postRepoMock.Setup(x => x.GetAsync(postId)).ReturnsAsync(new Post());
            _voteRepoMock.Setup(x => x.FindInstance(postId, userId)).ReturnsAsync(() => null);

            //Act
            VoteResponse vr = await _sut.UnVote(postId, userId);

            //Assert
            Assert.False(vr.Success);
            Assert.Equal("You did not vote.", vr.Message);
        }

        [Fact]
        public async Task UnVote_ShouldReturnResponseSuccessTrue_WhenUserSuccessfulyUnVote()
        {
            //Arange
            var postId = Guid.NewGuid().ToString();
            var userId = Guid.NewGuid().ToString();
            Vote vote = new Vote
            {
                Id = Guid.NewGuid().ToString(),
                UserId = userId,
                PostId = postId
            };
            _postRepoMock.Setup(x => x.GetAsync(postId)).ReturnsAsync(new Post());
            _voteRepoMock.Setup(x => x.FindInstance(postId, userId)).ReturnsAsync(vote);

            //Act
            VoteResponse vr = await _sut.UnVote(postId, userId);

            //Assert
            Assert.True(vr.Success);
            Assert.Equal(postId, vr.Resource.PostId);
            Assert.Equal(userId, vr.Resource.UserId);
        }
    }
}
