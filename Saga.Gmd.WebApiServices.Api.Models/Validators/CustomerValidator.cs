using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Saga.Gmd.WebApiServices.Api.Models.Parameters;
using Saga.Gmd.WebApiServices.Common;
 

namespace Saga.Gmd.WebApiServices.Api.Models.Validators
{
    public class CustomerValidator : AbstractValidator<Customer>
    {
        public CustomerValidator()
        {
            RuleFor(x => x.Email)
                .Cascade(CascadeMode.StopOnFirstFailure).Length(0, 80)
                .WithMessage(ErrorMessages.EmailLengthError)
                .EmailAddress()
                .WithMessage(ErrorMessages.EmailIsInvalid)
                .Length(6, 80);
               

            RuleFor(x => x.Gender).Must(BeValidChars).WithMessage(ErrorMessages.Gender);
            RuleFor(c => c.SystemSource).NotEmpty().WithMessage(ErrorMessages.SystemSourceCannotBeEmpty).Length(0, 20).WithMessage(ErrorMessages.SystemSource);
            RuleFor(c => c.Title).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().WithMessage(ErrorMessages.TitleCannotBeEmpty).Length(2, 10).WithMessage(ErrorMessages.Title);
            RuleFor(c => c.Forename).Length(0, 100).WithMessage(ErrorMessages.Forename);
            RuleFor(c => c.Surname).NotEmpty().WithMessage(ErrorMessages.SurNameRequried).Length(0, 100).WithMessage(ErrorMessages.Surname);
            RuleFor(c => c.DateOfBirth.ToString()).Must(BevalidDate).WithMessage(ErrorMessages.DateofBirth);
            RuleFor(c => c.Telephone).Length(0, 16).WithMessage(ErrorMessages.Telephone);
            RuleFor(c => c.CustomerAddress).SetValidator(new CustomerAddressValidator());
        }

        public bool BeValidChars(char value)
        {
            return (value == 'F' || value == 'M');
        }
         
         
        public bool BevalidDate(string value)
        {
            DateTime date;
            return DateTime.TryParse(value, out date);
        }
    }
}
