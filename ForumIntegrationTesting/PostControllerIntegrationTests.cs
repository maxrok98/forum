using Forum;
using Forum.Contracts.Responses;
using Forum.Contracts.Requests;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Forum.Models;
using System.Net.Http.Headers;
//using Newtonsoft.Json;
using Forum.Contracts;
//using Nancy.Json;
using System.Net.Http.Json;

namespace ForumIntegrationTesting
{
    public class PostControllerIntegrationTests : IClassFixture<TestingForumFactory<Startup>>
    {
        private readonly HttpClient _client;

        public PostControllerIntegrationTests(TestingForumFactory<Startup> factory)
        {
            _client = factory.CreateClient();
            AuthenticateAsync();
        }

        protected void AuthenticateAsync()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", GetJwtAsync());
        }

        private string GetJwtAsync()
        {
            var jsonObj = new UserRegistrationRequest
            {
                Email = "newuser@example.com",
                UserName = "newuser",
                Password = "123456New@",
                ConfirmPassword = "123456New@"
            };

            var response = _client.PostAsJsonAsync(ApiRoutes.Identity.Register, jsonObj).Result;

            var registrationResponse = response.Content.ReadAsStringAsync().Result;
            var registrationResponseObj = JsonConvert.DeserializeObject<AuthSuccessResponse>(registrationResponse);
            return registrationResponseObj.Token;
        }

        protected void Login()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", GetJwtForLogin());
        }

        private string GetJwtForLogin()
        {
            var jsonObj = new UserLoginRequest
            {
                Email = "newuser@example.com",
                Password = "123456New@"
            };

            var response = _client.PostAsJsonAsync(ApiRoutes.Identity.Login, jsonObj).Result;

            var registrationResponse = response.Content.ReadAsStringAsync().Result;
            var registrationResponseObj = JsonConvert.DeserializeObject<AuthSuccessResponse>(registrationResponse);
            return registrationResponseObj.Token;
        }

        [Fact]
        public async Task Get_ShouldReturnAllPosts()
        {
            //Login();
            var response = await _client.GetAsync("api/post/get");

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var responseJson = JsonConvert.DeserializeObject<PageResponse<PostResponse>>(responseString);
            List<PostResponse> posts = new List<PostResponse>();
            foreach(PostResponse pr in responseJson.Results)
            {
                posts.Add(pr);
            }

            Assert.Equal("Little bit about OS", posts[0].Name);
            Assert.Equal("Little bit about ARM architecture", posts[1].Name);
        }

        [Fact]
        public async Task GetById_ShouldReturnPostWithId_WhenPostExist()
        {
            //Login();
            var response = await _client.GetAsync("api/post/get");

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var responseJson = JsonConvert.DeserializeObject<PageResponse<PostResponse>>(responseString);
            List<PostResponse> posts = new List<PostResponse>();
            foreach (PostResponse pr in responseJson.Results)
            {
                posts.Add(pr);
            }

            var response2 = await _client.GetAsync($"api/post/get/{posts[0].Id}");
            response2.EnsureSuccessStatusCode();

            var responseString2 = await response2.Content.ReadAsStringAsync();
            var responseJson2 = JsonConvert.DeserializeObject<PostResponse>(responseString2);

            Assert.Equal(posts[0].Content, responseJson2.Content);
        }

        [Fact]
        public async Task GetById_ShouldReturnBadRequest_WhenPostDoesNotExist()
        {
            //Login();
            var id = Guid.NewGuid().ToString();

            var response2 = await _client.GetAsync($"api/post/get/{id}");

            Assert.False(response2.IsSuccessStatusCode);
        }

        [Fact]
        public async Task Post_ShouldReturnCreatedPost()
        {
            Login();

            var newPost = new PostRequest
            {
                Name = "Post created for test",
                Content = "Some content for test",
                ThreadId = Guid.NewGuid().ToString(),
                Image = new byte[10]
            };

            var response = await _client.PostAsJsonAsync("api/post/post/", newPost);

            var registrationResponse = await response.Content.ReadAsStringAsync();
            var registrationResponseObj = JsonConvert.DeserializeObject<PostResponse>(registrationResponse);


            Assert.Equal(newPost.Name, registrationResponseObj.Name);
            Assert.Equal(newPost.Content, registrationResponseObj.Content);
        }

        [Fact]
        public async Task Put_ShouldReturnUpdatedPost()
        {
            Login();
            //---------CREATE NEW POST
            string threadId = Guid.NewGuid().ToString();
            var newPost = new PostRequest
            {
                Name = "Post created for test",
                Content = "Some content for test",
                ThreadId = threadId,
                Image = new byte[10]
            };

            var jsonString = new StringContent(JsonConvert.SerializeObject(newPost), Encoding.UTF8, "application/json");
            var response = _client.PostAsync("api/post/post/", jsonString).Result;

            var registrationResponse = response.Content.ReadAsStringAsync().Result;
            var registrationResponseObj = JsonConvert.DeserializeObject<PostResponse>(registrationResponse);

            //--------UPDATE CREATED POST
            var newPost2 = new PostRequest
            {
                Name = "Post for update",
                Content = "Content has to to be updated",
                ThreadId = threadId,
                Image = new byte[10]
            };

            var postId = registrationResponseObj.Id;
            var jsonString2 = new StringContent(JsonConvert.SerializeObject(newPost2), Encoding.UTF8, "application/json");
            var response2 = _client.PutAsync($"api/post/put/{postId}", jsonString2).Result;

            var registrationResponse2 = response2.Content.ReadAsStringAsync().Result;
            var registrationResponseObj2 = JsonConvert.DeserializeObject<PostResponse>(registrationResponse2);

            Assert.Equal(newPost2.Name, registrationResponseObj2.Name);
            Assert.Equal(newPost2.Content, registrationResponseObj2.Content);
        }

        [Fact]
        public async Task Delete_ShouldReturnDeletedPost()
        {
            //Login();
            //---------CREATE NEW POST
            var newPost = new PostRequest
            {
                Name = "Post created for test",
                Content = "Some content for test",
                ThreadId = Guid.NewGuid().ToString(),
                Image = new byte[10]
            };

            var jsonString = new StringContent(JsonConvert.SerializeObject(newPost), Encoding.UTF8, "application/json");
            var response = _client.PostAsync("api/post/post/", jsonString).Result;

            var registrationResponse = response.Content.ReadAsStringAsync().Result;
            var registrationResponseObj = JsonConvert.DeserializeObject<PostResponse>(registrationResponse);

            //---------DELETE NEW POST
            var postId = registrationResponseObj.Id;
            var response2 = _client.DeleteAsync($"api/post/delete/{postId}").Result;

            //---------CHECK IF DELETED
            var response3 = await _client.GetAsync($"api/post/get/{postId}");

            Assert.False(response3.IsSuccessStatusCode);

        }
    }
}
