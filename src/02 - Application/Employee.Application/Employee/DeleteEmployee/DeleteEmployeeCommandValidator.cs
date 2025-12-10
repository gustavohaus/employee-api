using FluentValidation;

namespace Employee.Application.Employee.DeleteEmployee
{
    public class DeleteEmployeeCommandValidator : AbstractValidator<DeleteEmployeeCommand>
    {
        public DeleteEmployeeCommandValidator()
        {
            RuleFor(x => x.EmployeeId)
                .NotEmpty().WithMessage("EmployeeId is required.");
        }
    }
}