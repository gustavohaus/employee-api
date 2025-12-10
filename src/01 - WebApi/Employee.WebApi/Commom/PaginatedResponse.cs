using System.Diagnostics.CodeAnalysis;

namespace Employee.WebApi.Commom
{
    [ExcludeFromCodeCoverage]
    public class PaginatedResponse<T> : ApiResponseWithData<IEnumerable<T>>
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
    }
}
