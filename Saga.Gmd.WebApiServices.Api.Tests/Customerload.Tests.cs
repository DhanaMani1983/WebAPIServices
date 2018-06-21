using System.Linq;
using FluentValidation.TestHelper;
using NUnit.Framework;

using Saga.Gmd.WebApiServices.Api.Models.Validators;
using Saga.Gmd.WebApiServices.Models.Customer;

namespace Saga.Gmd.WebApiServices.Api.Tests
{
    [TestFixture]
    public class Customerload
    {
        private CustomerLoadValidator _customerloadValidator;
        private CustomerLoad  _customer;

       private CustomerAddressValidator _customerAddressValidator;
        private CustomerAddress _cusotmerAddress;
        [OneTimeSetUp]
        public void Setup()
        {
            _customerloadValidator = new CustomerLoadValidator();
            _customer = new CustomerLoad();

            _customerAddressValidator = new CustomerAddressValidator();
            _cusotmerAddress = new CustomerAddress();
        }

        [Test]
        public void when_customer_load_request_arrives_SystemId_cannot_be_empty()
        {
            //var expectedMessage = "SystemId cannot be empty.";
            //var output = _customerloadValidator.ShouldHaveValidationErrorFor(x => x.SystemId, _customer);
            //var actualMessage = output.Select(x => x.ErrorMessage).ToList()[0];
            //Assert.AreSame(expectedMessage, actualMessage);
        }
        [Test]
        public void when_customer_load_request_arrives_SystemSource_cannot_be_empty()
        {
            //var expectedMessage = "System Source cannot be empty.";
            //var output = _customerloadValidator.ShouldHaveValidationErrorFor(x => x.SystemSource, _customer);
            //var actualMessage = output.Select(x => x.ErrorMessage).ToList()[0];
            //Assert.AreSame(expectedMessage, actualMessage);
        }
        [Test]
        public void when_customer_load_request_arrives_MarketingSource_cannot_be_empty()
        {
            //var expectedMessage = "MarketingSource cannot be empty.";
            //var output = _customerloadValidator.ShouldHaveValidationErrorFor(x => x.MarketingSource, _customer);
            //var actualMessage = output.Select(x => x.ErrorMessage).ToList()[0].ToString();
            //Assert.AreEqual(expectedMessage, actualMessage);
        }

        [Test]
        public void when_customer_load_request_arrives_TransactionType_cannot_be_empty()
        {
            //var expectedMessage = "Transaction Type cannot be empty.";
            //var output = _customerloadValidator.ShouldHaveValidationErrorFor(x => x.TransactionType, _customer);
            //var actualMessage = output.Select(x => x.ErrorMessage).ToList()[0];
            //Assert.AreSame(expectedMessage, actualMessage);
        }

        [Test]
        public void when_customer_load_request_arrives_TransactionBrand_cannot_be_empty()
        {
            //var expectedMessage = "Transaction Brand cannot be empty";
            //var output = _customerloadValidator.ShouldHaveValidationErrorFor(x => x.TransactionBrand, _customer);
            //var actualMessage = output.Select(x => x.ErrorMessage).ToList()[0];
            //Assert.AreSame(expectedMessage, actualMessage);
        }

        [Test]
        public void when_customer_load_request_arrives_with_address_then_postcode_cannot_be_empty()
        {

          
            //_cusotmerAddress.Address1 = "Line1";
            //_cusotmerAddress.Address2 = "Line2";
            //_cusotmerAddress.TownCity = "Folkstone";
            _cusotmerAddress.County = "Kent";

            var expectedMessage = "Postcode cannot be empty.";
            var output = _customerAddressValidator.ShouldHaveValidationErrorFor(x => x.Postcode, _cusotmerAddress);
            var actualMessage = output.Select(x => x.ErrorMessage).ToList()[0];
            Assert.AreSame(expectedMessage, actualMessage);
        }

        [Test]
        public void when_customer_load_request_arrives_with_address_with_postcode_no_message_should_be_shown()
        {

            _cusotmerAddress = new CustomerAddress();
            //_cusotmerAddress.Address1 = "Line1";
            //_cusotmerAddress.Address2 = "Line2";
            //_cusotmerAddress.TownCity = "Folkstone";
            _cusotmerAddress.County = "Kent";
            _cusotmerAddress.Postcode = "CT10 4FN";
            _customerAddressValidator.ShouldNotHaveValidationErrorFor(x => x.Postcode, _cusotmerAddress);
        }

        [Test] //CustomerAddressValidator
        public void CustomerLoadValidator_validator_should_have_AddressValdator_set()
        {
            _customerloadValidator.ShouldHaveChildValidator(x => x.Address, typeof(CustomerAddressValidator));
        }
    }
}
