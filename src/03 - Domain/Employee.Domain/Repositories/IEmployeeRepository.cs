namespace Employee.Domain.Repositories
{
    public interface IEmployeeRepository
    {
        Task<Employee.Domain.Entities.Employee?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Employee.Domain.Entities.Employee> CreateAsync(Employee.Domain.Entities.Employee employee, CancellationToken cancellationToken = default);
        Task<Employee.Domain.Entities.Employee?> GetEmployeeByEmailOrCpf(string email, string cpf , CancellationToken cancellationToken = default);
        Task<Employee.Domain.Entities.Employee?> GetEmployeeByManagerId(Guid managerId, CancellationToken cancellationToken = default);
    }
}
