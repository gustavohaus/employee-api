using Microsoft.AspNetCore.Mvc;

namespace Employee.WebApi.Controllers
{
    public class EmployeeController : Controller
    {
        public EmployeeController()
        {
            
        }
        [HttpGet("employee")]
        public async Task<IActionResult> GetEmployee()
        {
            return Ok();
        }
    }
}
