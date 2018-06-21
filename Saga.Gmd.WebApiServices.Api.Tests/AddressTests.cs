using FluentValidation.TestHelper;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Saga.Gmd.WebApiServices.Api.Models;
using Saga.Gmd.WebApiServices.Api.Models.Validators;
using Saga.Gmd.WebApiServices.Models;
using System.Linq;
using Saga.Gmd.WebApiServices.Models.MailingHistory;

namespace Saga.Gmd.WebApiServices.Api.Tests
{
    [TestFixture]
    public class AddressTests
    {
        private AddressValidator _addressValidator;
        private Address _address;
        [OneTimeSetUp]
        public void Setup()
        {
            _addressValidator = new AddressValidator();
        }

        [Test]
        public void when_get_request_arrives_with_NAME_AND_ADDRESS_without_postcode_then_msg_should_be_sent()
        {
            _address = new Address();
            _address.Address1 = "line 1";
            var expectedMessage = "Postcode cannot be empty.";
            var output = _addressValidator.ShouldHaveValidationErrorFor(x => x.Postcode, _address);
            var actualMessage = output.Select(x => x.ErrorMessage).ToList()[0];
            Assert.AreSame(expectedMessage, actualMessage);
        }

        [Test]
        public void when_Get_request_arrives_Address1_is_too_long_then_msg_should_be_shown()
        {
            _addressValidator = new AddressValidator();
            var address = new Address();
            address.Postcode = "CT10 4TY";
            address.Address1 = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aliquam non feugiat nunc, eu fermentum nisl volutpat.";
            string expected = "Address line1 is too long.";
            var output = _addressValidator.ShouldHaveValidationErrorFor(x => x.Address1, address);
            var actualMessage = output.Select(x => x.ErrorMessage).ToList()[0];
            Assert.AreSame(expected, actualMessage);
        }
        [Test]
        public void when_Get_request_arrives_Address2_is_too_long_then_msg_should_be_shown()
        {
            _addressValidator = new AddressValidator();
            var address = new Address();
            address.Postcode = "CT10 4TY";
            address.Address2 =
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aliquam non feugiat nunc, eu fermentum nisl volutpat.";
            string expected = "Address line2 is too long.";
            var output = _addressValidator.ShouldHaveValidationErrorFor(x => x.Address2, address);
            var actualMessage = output.Select(x => x.ErrorMessage).ToList()[0];
            Assert.AreSame(expected, actualMessage);
        }

        [Test]
        public void when_Get_request_arrives_Address3_is_too_long_then_msg_should_be_shown()
        {
            _addressValidator = new AddressValidator();
            var address = new Address();
            address.Postcode = "CT10 4TY";
            address.Address3 =
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aliquam non feugiat nunc, eu fermentum nisl volutpat.";
            string expected = "Address line3 is too long.";
            var output = _addressValidator.ShouldHaveValidationErrorFor(x => x.Address3, address);
            var actualMessage = output.Select(x => x.ErrorMessage).ToList()[0];
            Assert.AreSame(expected, actualMessage);
        }

        [Test]
        public void when_Get_request_arrives_Address4_is_too_long_then_msg_should_be_shown()
        {
            _addressValidator = new AddressValidator();
            var address = new Address();
            address.Postcode = "CT10 4TY";
            address.Address4 =
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aliquam non feugiat nunc, eu fermentum nisl volutpat.";
            string expected = "Address line4 is too long.";
            var output = _addressValidator.ShouldHaveValidationErrorFor(x => x.Address4, address);
            var actualMessage = output.Select(x => x.ErrorMessage).ToList()[0];
            Assert.AreSame(expected, actualMessage);
        }

        [Test]
        public void when_get_request_arrives_with_Name_And_Address_with_postcode_lessthan_6_characters_then_message_should_be_sent()
        {
            _address = new Address();
            _address.Postcode = "QA";
            var expectedMessage = "Postcode is invalid.";
            var output = _addressValidator.ShouldHaveValidationErrorFor(x => x.Postcode, _address);
            var actualMessage = output.Select(x => x.ErrorMessage).ToList()[0];
            Assert.AreSame(expectedMessage, actualMessage);
        }

        [Test]
        public void when_get_request_arrives_with_Name_And_Address_with_postcode_greaterthan_10_characters_then_message_should_be_sent()
        {
            _address = new Address();
            _address.Postcode = "CT10 4TYZ11A";
            var expectedMessage = "Postcode is invalid.";
            var output = _addressValidator.ShouldHaveValidationErrorFor(x => x.Postcode, _address);
            var actualMessage = output.Select(x => x.ErrorMessage).ToList()[0];
            Assert.AreSame(expectedMessage, actualMessage);
        }
    }
}
