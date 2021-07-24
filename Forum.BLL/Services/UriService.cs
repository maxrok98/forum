using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forum.Shared.Contracts.Requests.Queries;
using Microsoft.AspNetCore.WebUtilities;

namespace Forum.BLL.Services
{
    public class UriService : IUriService
    {
        private string _baseUri;
        public UriService(string baseUri)
        {
            _baseUri = baseUri;
        }
        public Uri GetAllPostUri(string url, PaginationQuery paginationQuery = null)
        {
            _baseUri += url;
            var uri = new Uri(_baseUri);
            if (paginationQuery == null)
            {
                return uri;
            }

            var modifiedUri = QueryHelpers.AddQueryString(_baseUri, "pageNumber", paginationQuery.PageNumber.ToString());
            modifiedUri = QueryHelpers.AddQueryString(modifiedUri, "pageSize", paginationQuery.PageSize.ToString());

            return new Uri(modifiedUri);
        }

        public Uri GetPostUri(string postId)
        {
            throw new NotImplementedException();
        }
    }
}
