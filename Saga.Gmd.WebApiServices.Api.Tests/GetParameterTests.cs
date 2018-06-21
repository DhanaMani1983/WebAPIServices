using FluentValidation;
using FluentValidation.TestHelper;
using NUnit.Framework;
using Saga.Gmd.WebApiServices.Api.Models;
using Saga.Gmd.WebApiServices.Api.Models.Validators;
using System;
using System.Linq;
using Saga.Gmd.WebApiServices.Api.Models.Parameters;
using Saga.Gmd.WebApiServices.Common;
using Saga.Gmd.WebApiServices.Models;

namespace Saga.Gmd.WebApiServices.Api.Tests
{
    [TestFixture]
    public class GetParameterTests
    {
        private GetParameterValidator _getParameterValidator;
        private GetParameters _getParameters;

        [OneTimeSetUp]
        public void Setup()
        {
            _getParameterValidator = new GetParameterValidator();
            _getParameterValidator.CascadeMode = CascadeMode.StopOnFirstFailure;
        }


        [Test]
        public void IWillSend_has_excactly_two_item_in_the_list()
        {
            var expectedCount = 2;
            var actualCount = Enum.GetNames(typeof(ParameterType)).Length;
            Assert.AreEqual(expectedCount, actualCount);
        }

        [Test]
        public void when_get_request_arrives_IWillSend_has_no_valuee_then_send_msg()
        {
            _getParameters = new GetParameters();
            var expectedMessage = "Parameter 'IWillSend' cannot be empty.";
            var output = _getParameterValidator.ShouldHaveValidationErrorFor(x => x.IwillSend, _getParameters);
            var actualMessage = output.Select(x => x.ErrorMessage).ToList()[0];
            Assert.AreSame(expectedMessage, actualMessage);
        }

        [Test]
        public void when_get_request_arrives_IWillSend_has_invalid_type_then_send_msg()
        {
            _getParameters = new GetParameters();
            _getParameters.IwillSend = "wrongvalue";
            var expectedMessage = "Parameter 'IWillSend' value has invalid argument.";
            var output = _getParameterValidator.ShouldHaveValidationErrorFor(x => x.IwillSend, _getParameters);
            var actualMessage = output.Select(x => x.ErrorMessage).ToList()[0];
            Assert.AreSame(expectedMessage, actualMessage);
        }


        [Test]
        public void when_get_request_arrives_IWillSend_has_valid_type_then_no_msg_should_be_sent()
        {
            _getParameters = new GetParameters();
            _getParameters.IwillSend = "NameAndAddress";
            _getParameterValidator.ShouldNotHaveValidationErrorFor(x => x.IwillSend, _getParameters);
        }

        [Test]
        public void
            when_get_request_arrives_IWillSend_has_NAME_AND_ADDRESS_and_not_parameters_provided_then_msg_should_be_sent()
        {
            _getParameters = new GetParameters();
            _getParameters.IwillSend = "NameAndAddress";
            _getParameters.NameAndAddress = null;
            var output = _getParameterValidator.ShouldHaveValidationErrorFor(x => x.NameAndAddress, _getParameters);
            var expectedMessage = "NameAndAddress cannot be empty.";
            var actualMessage = output.Select(x => x.ErrorMessage).ToList()[0];
            Assert.AreSame(expectedMessage, actualMessage);
        }

        [Test]
        public void
            when_get_request_arrives_IWillSend_has_NAME_AND_ADDRESS_and_has_valid_parameters_provided_then_no_msg_should_be_sent
            ()
        {
            _getParameters = new GetParameters();
            _getParameters.IwillSend = "NameAndAddress";
            _getParameters.NameAndAddress = new NameAndAddress { Surname = "Doe", FirstName = "John" };
            _getParameterValidator.ShouldNotHaveValidationErrorFor(x => x.NameAndAddress, _getParameters);

        }

        [Test]
        public void
            when_get_request_arrives_IWillSend_has_KeyValuePair_and_no_parameters_provided_then_msg_should_be_sent()
        {
            _getParameters = new GetParameters();
            _getParameters.IwillSend = "KeyValuePair";
            _getParameters.KeyValue = null;
            var output = _getParameterValidator.ShouldHaveValidationErrorFor(x => x.KeyValue, _getParameters);
            var expectedMessage = "KeyValue cannot be empty.";
            var actualMessage = output.Select(x => x.ErrorMessage).ToList()[0];
            Assert.AreSame(expectedMessage, actualMessage);

        }

        [Test]
        public void when_get_request_arrives_ReturnMe_parameter_is_empty_then_msg_should_be_sent()
        {
            _getParameters = new GetParameters();
            _getParameters.ReturnMe = null;
            var output = _getParameterValidator.ShouldHaveValidationErrorFor(x => x.ReturnMe, _getParameters);
            var expectedMessage = "ReturnMe cannot be empty.";
            var actualMessage = output.Select(x => x.ErrorMessage).ToList()[0];
            Assert.AreSame(expectedMessage, actualMessage);
        }

    }
}
