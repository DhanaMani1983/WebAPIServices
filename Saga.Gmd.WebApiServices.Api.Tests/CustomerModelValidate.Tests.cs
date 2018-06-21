using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.TestHelper;
using NUnit.Framework;
using Saga.Gmd.WebApiServices.Api.Models.Parameters;
using Saga.Gmd.WebApiServices.Api.Models.Validators;

namespace Saga.Gmd.WebApiServices.Api.Tests
{
    [TestFixture]
    public class CustomerModelValidate
    {
        private CustomerValidator _customerValidator;
        private Customer _customer;

        [OneTimeSetUp]
        public void Setup()
        {
            _customerValidator = new CustomerValidator();
            _customer = new Customer();
        }
        [Test]
        public void when_post_request_arrives_SystemSource_cannot_be_empty()
        {
            var expectedMessage = "System Source cannot be empty.";
            var output = _customerValidator.ShouldHaveValidationErrorFor(x => x.SystemSource, _customer);
            var actualMessage = output.Select(x => x.ErrorMessage).ToList()[0];
            Assert.AreSame(expectedMessage, actualMessage);

        }

        [Test]
        public void when_post_request_arrives_email_should_be_valid()
        {
            _customer.Email = "test.com";
            var expectedMessage = "Email is invalid.";
            var output = _customerValidator.ShouldHaveValidationErrorFor(x => x.Email, _customer);
            var actualMessage = output.Select(x => x.ErrorMessage).ToList()[0];
            Assert.AreSame(expectedMessage, actualMessage);

        }

        [Test]
        public void when_post_request_arrives_email_should_be_valid_length()
        {
            _customer.Email = "test.com12345678901234567890123456789012345678901234567890123456789012345678901234567890";
            var expectedMessage = "Email is too long.";
            var output = _customerValidator.ShouldHaveValidationErrorFor(x => x.Email, _customer);
            var actualMessage = output.Select(x => x.ErrorMessage).ToList()[0];
            Assert.AreSame(expectedMessage, actualMessage);

        }
        [Test]
        public void when_post_request_arrives_Gender_should_be_valid_value()
        {
            _customer.Gender = 'X';
            var expectedMessage = "Gender has invalid value.";
            var output = _customerValidator.ShouldHaveValidationErrorFor(x => x.Gender, _customer);
            var actualMessage = output.Select(x => x.ErrorMessage).ToList()[0];
            Assert.AreSame(expectedMessage, actualMessage);

        }
        [Test]
        public void when_post_request_arrives_Title_should_not_be_empty()
        {
            _customer.Title = String.Empty;
            var expectedMessage = "Title cannot be empty.";
            var output = _customerValidator.ShouldHaveValidationErrorFor(x => x.Title, _customer);
            var actualMessage = output.Select(x => x.ErrorMessage).ToList()[0];
            Assert.AreSame(expectedMessage, actualMessage);

        }

        [Test]
        public void when_post_request_arrives_Title_should_be_valid_length()
        {
            _customer.Title = "XXXXXXXXXXX";
            var expectedMessage = "Title value is too long.";
            var output = _customerValidator.ShouldHaveValidationErrorFor(x => x.Title, _customer);
            var actualMessage = output.Select(x => x.ErrorMessage).ToList()[0];
            Assert.AreSame(expectedMessage, actualMessage);

        }

        [Test]
        public void when_post_request_arrives_Telephone_should_be_valid_length()
        {
            _customer.Telephone = "12345678901234561";
            var expectedMessage = "Telephone value is too long.";
            var output = _customerValidator.ShouldHaveValidationErrorFor(x => x.Telephone, _customer);
            var actualMessage = output.Select(x => x.ErrorMessage).ToList()[0];
            Assert.AreSame(expectedMessage, actualMessage);

        }

    }
}
