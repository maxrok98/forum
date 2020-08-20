using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Contracts
{
    public static class ApiRoutes
    {
        public const string Root = "api";

        public const string Version = "v1";

        public const string Base = Root;

        public static class Posts
        {
            public const string GetAll = Base + "/post/get";

            public const string Update = Base + "/post/put/{postId}";

            public const string Delete = Base + "/post/delet/{postId}";

            public const string Get = Base + "/post/get/{postId}";

            public const string Create = Base + "/post/post";
        }

        public static class Threads
        {
            public const string GetAll = Base + "/thread/get";

            public const string Update = Base + "/thread/put/{Id}";

            public const string Delete = Base + "/thread/delet/{Id}";

            public const string Get = Base + "/thread/get/{Id}";

            public const string Create = Base + "/thread/post";

            public const string Subscribe = Base + "subscribe/{id}";

            public const string Unsubscribe = Base + "unsubscribe/{id}";
        }

        public static class User
        {
            public const string Get = Base + "/user/get/";
        }

        public static class Tags
        {
            public const string GetAll = Base + "/tags";

            public const string Get = Base + "/tags/{tagName}";

            public const string Create = Base + "/tags";

            public const string Delete = Base + "/tags/{tagName}";
        }

        public static class Identity
        {
            public const string Login = Base + "/identity/login";

            public const string Register = Base + "/identity/register";

            public const string Refresh = Base + "/identity/refresh";
        }
    }
}
