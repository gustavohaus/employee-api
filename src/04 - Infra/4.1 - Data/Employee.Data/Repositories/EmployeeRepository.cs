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
                .Include(s => s.Manager)
                .Include(s => s.Employees)
                .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        }
        public async Task<Employee.Domain.Entities.Employee?> GetEmployeeByEmailOrCpf(string email, string cpf, CancellationToken cancellationToken = default)
        {
            return await _context.Employees
                .Include(s => s.Phones)
                .FirstOrDefaultAsync(s => s.Email == email || s.DocumentNumber == cpf, cancellationToken);
        }
        public async Task<Employee.Domain.Entities.Employee?> GetEmployeeByEmail(string email , CancellationToken cancellationToken = default)
        {
            return await _context.Employees
                .Include(s => s.Phones)
                .FirstOrDefaultAsync(s => s.Email == email, cancellationToken);
        }
        public async Task<bool> DeleteAsync(Employee.Domain.Entities.Employee employee, CancellationToken cancellationToken = default)
        {
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
        public async Task<Employee.Domain.Entities.Employee> UpdateAsync(Employee.Domain.Entities.Employee employee, CancellationToken cancellationToken = default)
        {
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync(cancellationToken);
            return employee;
        }

        public async Task<(List<Employee.Domain.Entities.Employee>, int)> GetPagedSalesAsync(
            int pageNumber,
            int pageSize,
            DateTime? startDate,
            DateTime? endDate,
            Guid? managerId,
            EmployeeRole? role,
            CancellationToken cancellationToken = default)
        {
            var query = _context.Employees.AsQueryable();

            if (startDate.HasValue)
                query = query.Where(s => s.CreatedAt >= DateTime.SpecifyKind(startDate.Value.Date, DateTimeKind.Utc));

            if (endDate.HasValue)
                query = query.Where(s => s.CreatedAt <= DateTime.SpecifyKind(endDate.Value.Date.AddDays(1).AddTicks(-1), DateTimeKind.Utc));

            if (managerId.HasValue)
                query = query.Where(s => s.ManagerId == managerId.Value);

            if (role.HasValue)
                query = query.Where(s => s.Role == role.Value);

            var totalCount = await query.CountAsync(cancellationToken);

            var sales = await query
                .Include(s => s.Phones)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return (sales, totalCount);
        }

    }
}
