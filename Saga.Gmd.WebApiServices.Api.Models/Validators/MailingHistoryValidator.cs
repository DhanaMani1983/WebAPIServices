using FluentValidation;
using Saga.Gmd.WebApiServices.Common;
using System;
using Saga.Gmd.WebApiServices.Models;
using Saga.Gmd.WebApiServices.Models.MailingHistory;

namespace Saga.Gmd.WebApiServices.Api.Models.Validators
{
    public class MailingHistoryValidator : AbstractValidator<MailingHistory>
    {
        public MailingHistoryValidator()
        {
            RuleFor(x => x.FromDate).Cascade(CascadeMode.StopOnFirstFailure)
                .Must(BeAValidDate)
                .WithMessage(ErrorMessages.FromDateIsInvalid)
                .When(x => x.FromDate?.Date != DateTime.MinValue.Date)
               ;

            RuleFor(x => x.ToDate).Cascade(CascadeMode.StopOnFirstFailure)
               .Must(BeAValidDate)
               .WithMessage(ErrorMessages.ToDateIsInvalid)
               .When(x => x.ToDate?.Date != DateTime.MinValue.Date)
             ;

            RuleFor(x => x.ToDate).Cascade(CascadeMode.StopOnFirstFailure)
              .Must(BeAValidDate)
              .WithMessage(ErrorMessages.ToDateIsInvalid)
              .When(x => x.ToDate?.Date != DateTime.MinValue.Date)
              .GreaterThanOrEqualTo(x => x.FromDate)
              .When(x => x.ToDate != DateTime.MaxValue && x.FromDate != DateTime.MaxValue)
              .When(x => x.ToDate != DateTime.MinValue && x.FromDate != DateTime.MinValue)
              .WithMessage(ErrorMessages.ToDateIsGreaterThanFromDate)
              .WithName("ToDateComapre");

        }

        private bool BeAValidDate(DateTime? value)
        {
            if (value == null)
            {
                return false;
            }
            else
            {
                return value != DateTime.MaxValue;
            }
        }
    }
}
