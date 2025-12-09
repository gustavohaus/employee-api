using System.Numerics;

namespace Employee.Domain.Entities;
public class Employee 
{    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string DocumentNumber { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public EmployeeRole Role { get;     set; }
    public EmployeeStatus Status { get; set; }
    public DateTime BirthDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Guid? ManagerId { get; set; }
    public Employee? Manager { get; set; }
    public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    public IReadOnlyCollection<Phone> Phones => _phones.AsReadOnly();

    private readonly List<Phone> _phones = new();

    protected Employee()
    {
        
    }

    public Employee(string firtName, string lastName, string email, string documentNumber, string password, DateTime birthDate ,EmployeeRole role)
    {
        FirstName = firtName;
        LastName = lastName;
        Email = email;
        DocumentNumber = documentNumber;
        Password = password;
        BirthDate = birthDate;
        Role = role;

        CreatedAt = DateTime.UtcNow;
        this.Activate();
    }

    public void AddPhones(Phone phone)
    {
        _phones.Add(phone);
        UpdatedAt = DateTime.UtcNow;
    }
    public void UpdatePhone(Guid phoneId, string number, PhoneType type, bool isPrimary)
    {
        var phone = _phones.FirstOrDefault(p => p.Id == phoneId);

        if (phone == null)
            throw new InvalidOperationException($"Phone with ID {phoneId} not found in the employee.");

        phone.Update(number, type, isPrimary);

        UpdatedAt = DateTime.UtcNow;
    }

    public void RemoveProduct(Guid phoneId)
    {
        var phoneToRemove = _phones.FirstOrDefault(p => p.Id == phoneId);

        if (phoneToRemove == null)
            throw new InvalidOperationException($"Phone with ID {phoneId} not found in the employee.");

        _phones.Remove(phoneToRemove);


    }
    public void Update(string firtName, string lastName, string email, string documentNumber, DateTime birthDate, EmployeeRole role)
    {
        FirstName = firtName;
        LastName = lastName;
        Email = email;
        DocumentNumber = documentNumber;
        BirthDate = birthDate;
        Role = role;

        this.Activate();
    }

    public void Activate()
    {
        Status = EmployeeStatus.Active;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        Status = EmployeeStatus.Inactive;
        UpdatedAt = DateTime.UtcNow;
    }
}