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
        public static PageResponse<T> CreatePaginatedResponse<T>(IUriService uriService, PaginationFilter pagination, IEnumerable<T> response, int countOfAll, string url)
        {
            var nextPage = pagination.PageNumber >= 1
                ? uriService
                    .GetAllPostUri(url, new PaginationQuery(pagination.PageNumber + 1, pagination.PageSize)).ToString()
                : null;
            var previousPage = pagination.PageNumber - 1 >= 1
                ? uriService
                    .GetAllPostUri(url, new PaginationQuery(pagination.PageNumber - 1, pagination.PageSize)).ToString()
                : null;
            var count = (double)countOfAll / pagination.PageSize;
            var pageCount = count == Math.Floor(count) ? count : Math.Floor(count) + 1;

            return new PageResponse<T>
            {
                Results = response,
                CurrentPage = pagination.PageNumber >= 1 ? pagination.PageNumber : (int?)null,
                PageSize = pagination.PageSize >= 1 ? pagination.PageSize : (int?)null,
                PageCount = (int)pageCount,
                NextPage = response.Any() ? nextPage.ToString() : null,
                PreviousPage = previousPage
            };
        }
    }
}
