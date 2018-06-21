using FluentValidation;
 
using Saga.Gmd.WebApiServices.Models.Customer;
using Saga.Gmd.WebApiServices.Common;

namespace Saga.Gmd.WebApiServices.Api.Models.Validators
{
   public class CustomerloadAddressValidator: AbstractValidator<CusotmerAddress>
    {
       public CustomerloadAddressValidator()
       {
            RuleFor(c => c.Postcode)
                .NotEmpty()
                .When(x => !string.IsNullOrEmpty(x.Address1) ||
                !string.IsNullOrEmpty(x.Address2) ||
                !string.IsNullOrEmpty(x.TownCity) ||
                !string.IsNullOrEmpty(x.County))
                .WithMessage(ErrorMessages.PostcodeRequried);
        }
    }
}
