namespace Employee.Domain.Entities;
public class Phone 
{
    public Guid Id { get; set; }
    public Guid EmployeeId { get; set; }
    public Employee Employee { get; set; }
    public string Number { get; set; } 
    public PhoneType Type { get; set; }
    public bool IsPrimary { get; set; }

    protected Phone()
    {
        
    }
    public Phone(Employee employee, string number, PhoneType type, bool isPrimary)
    {
        Employee = employee;
        Number = number;
        Type = type;
        IsPrimary = isPrimary;
    }
}