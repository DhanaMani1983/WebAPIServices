using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using FluentValidation;
using FluentValidation.Validators;
using log4net;
using Saga.Gmd.WebApiServices.Api.Models.ParameterModels;
using Saga.Gmd.WebApiServices.Common;
using Saga.Gmd.WebApiServices.BLL.Customer;
using Saga.Gmd.WebApiServices.Models;

namespace Saga.Gmd.WebApiServices.Api.Models.Validators
{
    public class KeyValueValidator : AbstractValidator<KeyValue>
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(KeyValueValidator));
        public KeyValueValidator()
        {
            RuleFor(x => x.Key).Cascade(CascadeMode.StopOnFirstFailure)
               .NotEmpty()
               .WithMessage(ErrorMessages.KeyCannotBeEmpty)
               .Unless(x => x.Key?.ToLower() == ParameterType.KeyValuePair.ToString().ToLower())
               .Must(BeInTheList).WithMessage(ErrorMessages.ProvidedKeyIsInvalid);

            //RuleFor(x => x.Value).Cascade(CascadeMode.StopOnFirstFailure)
            //    .NotNull()
            //    .When(x => !string.IsNullOrEmpty(x.Key))
            //    .WithMessage(ErrorMessages.ValueCannotBeEmpty)
            //    .Must(BeValidGuid)
            //    .Unless(x => x.Key?.ToLower() != SourceKey.SSON.ToString().ToLower() && x.Key?.ToLower() != SourceKey.MEMB.ToLowerString())
            //    .WithMessage(ErrorMessages.SSONValueMustBeGUID);

            //RuleFor(x => x.Value).Cascade(CascadeMode.StopOnFirstFailure)
            //   .NotEmpty()
            //   .WithMessage(ErrorMessages.ValueCannotBeEmpty)
            //   .Must(BeValidInt)
            //   .Unless(x => x.Key?.ToLower() == SourceKey.SSON.ToString().ToLower() && x.Key?.ToLower() == SourceKey.MEMB.ToLowerString())
            //   .WithMessage(ErrorMessages.ValueMustBeIntegerType);
        }


        private bool BeValidInt(string value)
        {
            int outInt;
            return Int32.TryParse(value, out outInt);
        }
        private bool BeValidGuid(string value)
        {
            Guid outGuid;
            return Guid.TryParse(value, out outGuid);
        }

        private bool BeInTheList(string value)
        {
            List<string> sourceKeyList = new List<string>();
            try
            {
              

                MciSourceKeyService mciSourceKeyService = new MciSourceKeyService();
                sourceKeyList = mciSourceKeyService.GetSourceKey();
            }
            catch (SqlException ex)
            {
                log.Error("KeyValueValidator - BeInTheList:" + "ErrorTag: " + ErrorTagProvider.ErrorTagDatabase + " -- " + ex.Message, ex);
                throw new Exception(string.Format(DatabaseMessage.DatabaseException, ErrorTagProvider.ErrorTagDatabase));
            }

             var output = !string.IsNullOrWhiteSpace(value) && sourceKeyList.Exists(x => x.Contains(value));
                return output;
            

        }
    }
    
    public class KeyValueParameterValidator : AbstractValidator<KeyValueParameter>
    {
        
        public KeyValueParameterValidator()
        { 
            RuleFor(x => x.KeyValue).SetValidator(new KeyValueValidator());
            RuleFor(x => x.ReturnMe).SetValidator(new ReturnMeCannotBeEmpty()).SetValidator(new KeyValueReturnMeValidator()); 
        } 

    }

    public class ReturnMeCannotBeEmpty : PropertyValidator
    {
        public ReturnMeCannotBeEmpty()
            : base(ErrorMessages.ReturnMeCannotBeEmpty)
        {
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            var parent = context.ParentContext.InstanceToValidate as KeyValueParameter;


            if (parent.ResponseRequestedItems == null ? false : parent.ResponseRequestedItems.Count == 0)
            {
                return false;
            }
            return true;
        }
    }
}