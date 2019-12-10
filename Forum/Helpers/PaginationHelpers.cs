using Forum.Contracts.Requests.Queries;
using Forum.Models;
using Forum.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forum.Contracts.Responses;

namespace Forum.Helpers
{
    public class PaginationHelpers
    {
        public static PageResponse<T> CreatePaginatedResponse<T>(IUriService uriService, PaginationFilter pagination, IEnumerable<T> response, string url)
        {
            var nextPage = pagination.PageNumber >= 1
                ? uriService
                    .GetAllPostUri(url, new PaginationQuery(pagination.PageNumber + 1, pagination.PageSize)).ToString()
                : null;
            var previousPage = pagination.PageNumber - 1 >= 1
                ? uriService
                    .GetAllPostUri(url, new PaginationQuery(pagination.PageNumber - 1, pagination.PageSize)).ToString()
                : null;

            return new PageResponse<T>
            {
                Data = response,
                PageNumber = pagination.PageNumber >= 1 ? pagination.PageNumber : (int?)null,
                PageSize = pagination.PageSize >= 1 ? pagination.PageSize : (int?)null,
                NextPage = response.Any() ? nextPage.ToString() : null,
                PreviousPage = previousPage
            };
        }
    }
}
