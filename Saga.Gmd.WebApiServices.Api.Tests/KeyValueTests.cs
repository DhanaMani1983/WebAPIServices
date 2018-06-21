using FluentValidation.TestHelper;
using NUnit.Framework;
using Saga.Gmd.WebApiServices.Api.Models.Validators;

using System.Linq;
using Saga.Gmd.WebApiServices.Api.Models.ParameterModels;
using Saga.Gmd.WebApiServices.Models;

namespace Saga.Gmd.WebApiServices.Api.Tests
{
    [TestFixture]
    public class KeyValueTests
    {
        private KeyValueParameter _keyValuePar; 
        private KeyValueParameterValidator _keyValueParameterValidator;
        private KeyValueValidator _keyValueValidator;
        private KeyValue _keyValue;


        [OneTimeSetUp]
        public void Setup()
        {
            _keyValueParameterValidator = new KeyValueParameterValidator();
            _keyValuePar = new KeyValueParameter();
            _keyValueValidator = new KeyValueValidator();
            _keyValue = new KeyValue();
        }


        [Test]
        public void when_get_request_arrives_with_KeyValuePair_and_key_is_not_present_msg_should_be_sent()
        {
           
            _keyValue.Key = null;
            var expectedMessage = "Key cannot be empty.";
            var output = _keyValueValidator.ShouldHaveValidationErrorFor(x => x.Key, _keyValue);
            var actualMessage = output.Select(x => x.ErrorMessage).ToList()[0];
            Assert.AreEqual(expectedMessage, actualMessage);
        }

        [Test]
        public void when_get_request_arrives_with_KeyValuePair_and_key_is_SSON_and_value_is_non_GUID_then_message_sent()
        {
         
            _keyValue.Key = "SSON";
            _keyValue.Value = "1234";
            var expectedMessage = "For SSON value must be GUID.";
            var output = _keyValueValidator.ShouldHaveValidationErrorFor(x => x.Value, _keyValue);
            var actualMessage = output.Select(x => x.ErrorMessage).ToList()[0];
            Assert.AreEqual(expectedMessage, actualMessage);
        }

        [Test]
        public void when_get_request_arrives_with_KeyValuePair_and_key_and_value_is_valid_msg_should_not_be_sent()
        {

            _keyValuePar.KeyValue.Key = "SSON";
            _keyValuePar.KeyValue.Value = "0507880d-da45-4d35-ba25-6639cea7b253";
            _keyValueParameterValidator.ShouldNotHaveValidationErrorFor(x => x.KeyValue, _keyValuePar);
        }


        [Test]
        public void when_get_request_arrives_with_keyValuePair_when_key_is_SSON_then_value_should_be_GUID_else_msg_sent()
        {

            _keyValue.Key = "SSON";
            _keyValue.Value = null;
            var expectedMessage = "Value cannot be empty.";
            var output = _keyValueValidator.ShouldHaveValidationErrorFor(x => x.Value, _keyValue);
            var actualMessage = output.Select(x => x.ErrorMessage).ToList()[0];
            Assert.AreEqual(expectedMessage, actualMessage);
        }


        [Test]
        public void when_get_request_arrives_with_KeyValuePair_and_key_is_invalid_msg_should_be_sent()
        {

            _keyValue.Key = "Test";
            _keyValue.Value = "1234";
            var expectedMessage = "Provided Key is invalid.";
            var output = _keyValueValidator.ShouldHaveValidationErrorFor(x => x.Key, _keyValue);
            var actualMessage = output.Select(x => x.ErrorMessage).ToList()[0];
            Assert.AreEqual(expectedMessage, actualMessage);
        }

        [Test]
        public void when_get_request_arrives_with_KeyValuePair_and_key_is_wrong_value_then_msg_should_be_sent()
        {

            _keyValue.Key = "WrongKey";
            var expectedMessage = "Provided Key is invalid.";
            var output = _keyValueValidator.ShouldHaveValidationErrorFor(x => x.Key, _keyValue);
            var actualMessage = output.Select(x => x.ErrorMessage).ToList()[0];
            Assert.AreEqual(expectedMessage, actualMessage);
        }

        [Test]
        public void when_get_reqeuest_arrives_with_KeyValuePair_and_key_is_non_SSON_and_value_is_non_integer_then_msg_should_be_sent()
        {

            _keyValue.Key = "TIAK";
            _keyValue.Value = "abc";
            var expectedMessage = "Value must be an integer type.";
            var output = _keyValueValidator.ShouldHaveValidationErrorFor(x => x.Value, _keyValue);
            var actualMessage = output.Select(x => x.ErrorMessage).ToList()[0];
            Assert.AreEqual(expectedMessage, actualMessage);
        }
    }
}
