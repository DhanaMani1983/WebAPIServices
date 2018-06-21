using FluentValidation.TestHelper;
using NUnit.Framework;
using Saga.Gmd.WebApiServices.Api.Models;
using Saga.Gmd.WebApiServices.Api.Models.Validators;
using System.Linq;
using Saga.Gmd.WebApiServices.Models;
using Saga.Gmd.WebApiServices.Models.MailingHistory;

namespace Saga.Gmd.WebApiServices.Api.Tests
{
    [TestFixture]
    public class NamdAndAddressTests
    {
        private NameAndAddressValidator _nameAndAddressValidator;
        private NameAndAddress _nameAndAddress;
        [OneTimeSetUp]
        public void Setup()
        {
            _nameAndAddressValidator = new NameAndAddressValidator();
            _nameAndAddress = new NameAndAddress();
        }

        [Test]
        public void when_Get_request_arrives_and_paramtertype_NAME_AND_ADDRESS_and_no_surname_provided_message_should_be_sent()
        {
           
            var expectedMessage = "Surname cannot be empty.";
            var output = _nameAndAddressValidator.ShouldHaveValidationErrorFor(x => x.Surname, _nameAndAddress);
            var actualMessage = output.Select(x => x.ErrorMessage).ToList()[0];
            Assert.AreEqual(expectedMessage, actualMessage);

        }

        [Test]
        public void when_request_arrives_with_NAME_AND_ADDRESS_but_no_address_provided_then_msg_should_be_sent()
        {
          
            _nameAndAddress.Address = null;
            var expectedMessage = "Address cannot be empty.";
            var output = _nameAndAddressValidator.ShouldHaveValidationErrorFor(x => x.Address, _nameAndAddress);
            var actualMessage = output.Select(x => x.ErrorMessage).ToList()[0];
            Assert.AreEqual(expectedMessage, actualMessage);
        }

        [Test]
        public void when_get_request_arrives_with_NAME_AND_ADDRESS_without_single_line_of_address_then_msg_should_be_sent()
        {
           
            var address = new Address();
            var expectedMessage = "At least one line of address should be provided.";
            var output = _nameAndAddressValidator.ShouldHaveValidationErrorFor(x => x.Address, address);
            var actualMessage = output.Select(x => x.ErrorMessage).ToList()[0];
            Assert.AreEqual(expectedMessage, actualMessage);
        }

        [Test]
        public void when_get_request_arrives_with_Name_And_Address_with_surname_greaterthan_30_characters_then_message_should_be_sent()
        {
          
            _nameAndAddress.Surname = "123456789123456789123456789123456789";
            var expectedMessage = "Surname is too long.";
            var output = _nameAndAddressValidator.ShouldHaveValidationErrorFor(x => x.Surname, _nameAndAddress);
            var actualMessage = output.Select(x => x.ErrorMessage).ToList()[0];
            Assert.AreEqual(expectedMessage, actualMessage);
        }

        [Test]
        public void when_get_request_arrives_with_Name_And_Address_with_title_greaterthan_30_characters_then_message_should_be_sent()
        {
         
            _nameAndAddress.Title = "123456789123456789123456789123456789";
            var expectedMessage = "Title is too long.";
            var output = _nameAndAddressValidator.ShouldHaveValidationErrorFor(x => x.Title, _nameAndAddress);
            var actualMessage = output.Select(x => x.ErrorMessage).ToList()[0];
            Assert.AreEqual(expectedMessage, actualMessage);
        }

        [Test]
        public void when_get_request_arrives_with_Name_And_Address_with_firstname_greaterthan_50_characters_then_message_should_be_sent()
        {
           
            _nameAndAddress.FirstName = "123456789123456789123456789123456789123456789123456789";
            var expectedMessage = "First name is too long.";
            var output = _nameAndAddressValidator.ShouldHaveValidationErrorFor(x => x.FirstName, _nameAndAddress);
            var actualMessage = output.Select(x => x.ErrorMessage).ToList()[0];
            Assert.AreEqual(expectedMessage, actualMessage);
        }

        [Test]
        public void when_get_request_arrives_with_Name_And_Address_with_phone_greaterthan_15_characters_then_message_should_be_sent()
        {
           
            _nameAndAddress.Phone = "123456789123456789123456789123456789";
            var expectedMessage = "Phone number is too long.";
            var output = _nameAndAddressValidator.ShouldHaveValidationErrorFor(x => x.Phone, _nameAndAddress);
            var actualMessage = output.Select(x => x.ErrorMessage).ToList()[0];
            Assert.AreEqual(expectedMessage, actualMessage);
        }

        [Test]
        public void when_get_request_arrives_with_Name_And_Address_with_email_greaterthan_40_characters_then_message_should_be_sent()
        {
            
            _nameAndAddress.Email = "123456789123456789123456789123456789123456789123456789123456789123456789123456789";
            var expectedMessage = "Email is too long.";
            var output = _nameAndAddressValidator.ShouldHaveValidationErrorFor(x => x.Email, _nameAndAddress);
            var actualMessage = output.Select(x => x.ErrorMessage).ToList()[0];
            Assert.AreEqual(expectedMessage, actualMessage);
        }

        [Test]
        public void when_get_request_arrives_and_there_is_email_with_wrong_format_then_msg_should_be_sent()
        {
            
            _nameAndAddress.Email = "dfgmial.com";
            var output = _nameAndAddressValidator.ShouldHaveValidationErrorFor(x => x.Email, _nameAndAddress);
            var expectedMessage = "Email is invalid.";
            var actualMessage = output.Select(x => x.ErrorMessage).ToList()[0];
            Assert.AreEqual(expectedMessage, actualMessage);

        }

        [Test]
        public void NameAndAddress_validator_should_have_AddressValdator_set()
        {
            _nameAndAddressValidator.ShouldHaveChildValidator(x => x.Address,typeof(AddressValidator));
        }
    }
}
