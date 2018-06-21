using FluentValidation.TestHelper;
using NUnit.Framework;
using System.Linq;
using Saga.Gmd.WebApiServices.Api.Models.Validators;
using Saga.Gmd.WebApiServices.Models.MailingHistory;

namespace Saga.Gmd.WebApiServices.Api.Tests
{
    [TestFixture]
    public class MailingHistoryTests
    {
        private MailingHistoryValidator _mailingHistoryValidator;
        private MailingHistory _mailingHistory;

        [OneTimeSetUp]
        public void Setup()
        {
            _mailingHistoryValidator = new MailingHistoryValidator();
        }

        [Test]
        public void when_Get_request_arrives_and_and_to_date_is_in_invalid_format_provided_message_should_be_sent()
        {
            _mailingHistory = new MailingHistory();
            _mailingHistory.ToDate = System.DateTime.MaxValue;
            var expectedMessage = "To date is invalid.";
            var output = _mailingHistoryValidator.ShouldHaveValidationErrorFor(x => x.ToDate, _mailingHistory);
            var actualMessage = output.Select(x => x.ErrorMessage).ToList()[0];
            Assert.AreEqual(expectedMessage, actualMessage);
        }

        [Test]
        public void when_Get_request_arrives_and_and_from_date_is_in_invalid_format_provided_message_should_be_sent()
        {
            _mailingHistory = new MailingHistory();
            _mailingHistory.FromDate = System.DateTime.MaxValue;
            var expectedMessage = "From date is invalid.";
            var output = _mailingHistoryValidator.ShouldHaveValidationErrorFor(x => x.FromDate, _mailingHistory);
            var actualMessage = output.Select(x => x.ErrorMessage).ToList()[0];
            Assert.AreEqual(expectedMessage, actualMessage);
        }

        [Test]
        public void when_Get_request_arrives_and_and_from_date_is_greater_than_to_date_is_provided_message_should_be_sent()
        {
            _mailingHistory = new MailingHistory();
            _mailingHistory.ToDate = new System.DateTime(2017, 03, 31);
            _mailingHistory.FromDate = new System.DateTime(2017, 04, 30);
            var expectedMessage = "ToDate must be greater than FromDate.";
            var output = _mailingHistoryValidator.Validate(_mailingHistory);
            var actualMessage = output.Errors[0].ToString();
            Assert.AreEqual(expectedMessage, actualMessage);
        }
    }
}
