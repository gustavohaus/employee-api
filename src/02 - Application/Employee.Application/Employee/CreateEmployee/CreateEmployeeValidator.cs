using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee.Application.Employee.CreateEmployee
{
    public class CreateEmployeeValidator : AbstractValidator<CreateEmployeeCommand>
    {
        public CreateEmployeeValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .WithMessage("First name is required.")
                .MaximumLength(50)
                .WithMessage("First name must not exceed 50 characters.");

            RuleFor(x => x.LastName)
                .NotEmpty()
                .WithMessage("Last name is required.")
                .MaximumLength(50)
                .WithMessage("Last name must not exceed 50 characters.");

            RuleFor(x => x.BirthDate)
                .NotEmpty()
                .WithMessage("Birth date is required.")
                .Must(date => date <= DateTime.Now.AddYears(-18))
                .WithMessage("Employee must be at least 18 years old.");

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email is required.")
                .EmailAddress()
                .WithMessage("Invalid email format.");

            RuleFor(x => x.DocumentNumber)
                .NotEmpty()
                .WithMessage("Document number is required.")
                .Matches(@"^\d{11}$") 
                .WithMessage("Document number must be 11 digits.");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password is required.")
                .MinimumLength(8)
                .WithMessage("Password must be at least 8 characters long.");

            RuleFor(x => x.Role)
                .IsInEnum()
                .WithMessage("Invalid role.");

            RuleFor(x => x.Status)
                .IsInEnum()
                .WithMessage("Invalid status.");

            RuleForEach(x => x.Phones).ChildRules(phone =>
            {
                phone.RuleFor(p => p.Number)
                    .NotEmpty()
                    .WithMessage("Phone number is required.")
                    .Matches(@"^\+?[1-9]\d{1,14}$") 
                    .WithMessage("Invalid phone number format.");

                phone.RuleFor(p => p.Type)
                    .IsInEnum()
                    .WithMessage("Invalid phone type.");

                phone.RuleFor(p => p.IsPrimary)
                    .NotNull()
                    .WithMessage("IsPrimary must be specified.");
            });
        }
    }
}
