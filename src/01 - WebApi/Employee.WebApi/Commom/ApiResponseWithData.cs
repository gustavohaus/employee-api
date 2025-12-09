namespace Employee.WebApi.Commom
{
    public class ApiResponseWithData<T> : ApiResponse
    {
        public T? Data { get; set; }
    }

}
