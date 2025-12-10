using Employee.Common.Validation;
using System.Diagnostics.CodeAnalysis;

namespace Employee.WebApi.Commom
{
    [ExcludeFromCodeCoverage]
    public class ApiResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public IEnumerable<ValidationErrorDetail> Errors { get; set; } = [];
    }

}
