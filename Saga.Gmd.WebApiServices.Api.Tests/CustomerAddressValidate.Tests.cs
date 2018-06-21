using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Saga.Gmd.WebApiServices.Api.Models.ParameterModels;
using Saga.Gmd.WebApiServices.Models.Customer;

namespace Saga.Gmd.WebApiServices.Api.Tests
{
    [TestFixture]
    public class CustomerAddressValidate
    {
        private CustomerAddressValidate _customerAddressValidate;
        private CustomerAddress _customerAddress;

        [OneTimeSetUp]
        public void Setup()
        {
            _customerAddressValidate = new CustomerAddressValidate();
            _customerAddress = new CustomerAddress();
        }

        public void when_post_request_arrives_Address1_length_is_too_big()
        {
            //_customerAddress.Address1 =
            //    "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nunc consectetur posuere mauris eu volutpat.";
            //var expectedMessage = "";
            //var output = 
        }

    }
}
