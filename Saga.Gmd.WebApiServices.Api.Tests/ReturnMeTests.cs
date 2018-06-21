using FluentValidation;
using FluentValidation.Internal;
using FluentValidation.Validators;
using NUnit.Framework;
using Saga.Gmd.WebApiServices.Api.Models.ParameterModels;
using Saga.Gmd.WebApiServices.Api.Models.Parameters;
using Saga.Gmd.WebApiServices.Api.Models.Validators;
using System.Linq;

namespace Saga.Gmd.WebApiServices.Api.Tests
{
    [TestFixture]
    public class ReturnMeTests
    {
        private ReturnMeValidator _returnMeValidator;
        
        [OneTimeSetUp]
        public void Setup()
        {
            _returnMeValidator = new ReturnMeValidator();
        }

        [Test]
        public void when_Get_request_arrives_and_no_customerskey_provided_in_customerKeys_ReturnMe_message_should_be_sent()
        {
            var getParameters = new GetParameters();
            getParameters.IwillSend = "KeyValuePair";
            getParameters.ReturnMe = new ReturnMe();
            getParameters.ReturnMe.CustomerKeys = new System.Collections.Generic.List<string>();
            var expectedMessage = "Customer keys cannot be null or empty.";
            var context = new ValidationContext(getParameters, new PropertyChain(), ValidatorOptions.ValidatorSelectors.DefaultValidatorSelectorFactory());
            var propertyValidatorContext = new PropertyValidatorContext(context, PropertyRule.Create<GetParameters, ReturnMe>(t => t.ReturnMe), "ReturnMe");
            var validator = new ReturnMeValidator.CustomerKeysCannotBeEmpty();
            var output = validator.Validate(propertyValidatorContext);
            var actualMessage = output.Select(x => x.ErrorMessage).ToList().Count > 0 ? output.Select(x => x.ErrorMessage).ToList()[0] : "";
            Assert.AreEqual(expectedMessage, actualMessage);
        }

    }
}
