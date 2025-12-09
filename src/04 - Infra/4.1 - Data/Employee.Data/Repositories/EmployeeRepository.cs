using Employee.Domain.Entities;
using Employee.Domain.Repositories;
using Microsoft.EntityFrameworkCore;


namespace Employee.Data.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly DefaultContext _context;

        public EmployeeRepository(DefaultContext context)
        {
            _context = context;
        }

        public async Task<Employee.Domain.Entities.Employee> CreateAsync(Employee.Domain.Entities.Employee employee, CancellationToken cancellationToken = default)
        {
            await _context.Employees.AddAsync(employee, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return employee;
        }

        public async Task<Employee.Domain.Entities.Employee?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Employees
                .Include(s => s.Phones)
                .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        }
        public async Task<Employee.Domain.Entities.Employee?> GetEmployeeByEmailOrCpf(string email, string cpf, CancellationToken cancellationToken = default)
        {
            return await _context.Employees
                .Include(s => s.Phones)
                .FirstOrDefaultAsync(s => s.Email == email || s.DocumentNumber == cpf, cancellationToken);
        }
        public async Task<Employee.Domain.Entities.Employee?> GetEmployeeByManagerId(Guid managerId, CancellationToken cancellationToken = default)
        {
            return await _context.Employees
                .Include(s => s.Phones)
                .FirstOrDefaultAsync(s => s.Id == managerId, cancellationToken);
        }

    }
}
