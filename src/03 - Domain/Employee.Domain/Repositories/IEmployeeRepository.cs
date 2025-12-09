namespace Employee.Domain.Repositories
{
    public interface IEmployeeRepository
    {
        Task<Employee.Domain.Entities.Employee?> GetByIdAsync(Guid? id, CancellationToken cancellationToken = default);
        Task<Employee.Domain.Entities.Employee> CreateAsync(Employee.Domain.Entities.Employee employee, CancellationToken cancellationToken = default);
        Task<Employee.Domain.Entities.Employee?> GetEmployeeByEmailOrCpf(string email, string cpf, CancellationToken cancellationToken = default);
        Task<Employee.Domain.Entities.Employee?> GetEmployeeByEmail(string email, CancellationToken cancellationToken = default);
        Task<Employee.Domain.Entities.Employee> UpdateAsync(Employee.Domain.Entities.Employee employee, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(Employee.Domain.Entities.Employee employee, CancellationToken cancellationToken = default);
       Task<(List<Employee.Domain.Entities.Employee>, int)> GetPagedSalesAsync(int pageNumber, int pageSize, DateTime? startDate, DateTime? endDate, Guid? managerId, EmployeeRole? role, CancellationToken cancellationToken = default);
    }
}
