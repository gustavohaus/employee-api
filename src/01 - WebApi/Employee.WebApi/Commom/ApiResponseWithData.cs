using System.Diagnostics.CodeAnalysis;

namespace Employee.WebApi.Commom
{
    [ExcludeFromCodeCoverage]
    public class ApiResponseWithData<T> : ApiResponse
    {
        public T? Data { get; set; }
    }

}
