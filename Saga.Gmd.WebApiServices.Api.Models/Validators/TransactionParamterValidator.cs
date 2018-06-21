using System;
using FluentValidation;

using Saga.Gmd.WebApiServices.Common;
using Saga.Gmd.WebApiServices.Models.Customer;
using Saga.Gmd.WebApiServices.Models.Transaction;


namespace Saga.Gmd.WebApiServices.Api.Models.Validators
{
    public class TransactionParamterValidator : AbstractValidator<ParameterModels.TransactionParamter>
    {
        public TransactionParamterValidator()
        {
            RuleFor(x => x.FeederSystem).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().Matches("^[a-zA-Z]+$");
            RuleFor(x => x.SourceSystem).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().Matches("^[a-zA-Z]+$");
            RuleFor(x => x.TransactionBrand).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().Matches("^[a-zA-Z]+$");
            //RuleFor(x => x.MarketingSource).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().Matches("^[a-zA-Z]+$");
            RuleFor(x => x.SeqNo).Cascade(CascadeMode.StopOnFirstFailure).NotNull().NotEmpty();
            RuleFor(x => x.Customer).SetValidator(new TransactCustomerValidator());
            RuleFor(x => x.Transaction).NotNull();
        }
    }


    public class TransactCustomerValidator : AbstractValidator<TransactionCustomerLoad>
    {
        public TransactCustomerValidator()
        {
            RuleFor(x => x.Surname).NotEmpty();
            //RuleFor(x => x.Title).NotEmpty();
            //RuleFor(x => x.DateOfBirth).Must(x => x == null || BeAValidDate(x.Value)).WithMessage(ErrorMessages.DOBIsInValid);
            //RuleFor(x => x.Address).Cascade(CascadeMode.StopOnFirstFailure).NotNull().SetValidator(new TranssactionCustomerAddressValidator());
        }
        private bool BeAValidDate(DateTime value)
        {
            return value != DateTime.MaxValue;
        }
    }
}
