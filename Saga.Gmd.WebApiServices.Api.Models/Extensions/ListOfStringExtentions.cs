using System;
using System.Collections.Generic;
using Saga.Gmd.WebApiServices.Common;
using Saga.Gmd.WebApiServices.Models;
using Saga.Gmd.WebApiServices.Models.Security;

namespace Saga.Gmd.WebApiServices.Api.Models.Extensions
{
    public static class ListOfStringExtentions
    {
        public static List<ModuleAccess> ToGroupCodeEnum(this List<string> values)
        {
            List<ModuleAccess> module = new List<ModuleAccess>();
            foreach (var codeItem in values)
            {
                var item = codeItem.ToLower();
                if (item == "mailinghistory")
                {
                    module.Add(new ModuleAccess {HasAccess = false, Key = GroupCode.MAILH});
                }
                else if (item == "permissions")
                {
                    module.Add(new ModuleAccess {HasAccess = false, Key = GroupCode.CPCK});
                }
                else if (item == "membership")
                {
                    module.Add(new ModuleAccess {HasAccess = false, Key = GroupCode.MEMB});
                } 
                else if (item == "cload")
                {
                    module.Add(new ModuleAccess {HasAccess = false,Key = GroupCode.CLOAD});
                }
                else if (item == "customerdetails")
                {
                    module.Add(new ModuleAccess {HasAccess = false, Key= GroupCode.GCUST});
                }
            }
            return module;
        }




        public static List<CustomerKeyAccess> ToCustomerKeyGroupCodeEnum(this List<string> values)
        {
            List<CustomerKeyAccess> codes = new List<CustomerKeyAccess>();
            foreach (var item in values)
            {
                GroupCode code;
                var isValidCode = Enum.TryParse(item, true, out code);
                if (isValidCode)
                {
                    codes.Add(new CustomerKeyAccess { HasAccess = false, Key = code });
                }
            }
            return codes;
        }

        public static AddressType? ToAddressType(this string value)
        {
            AddressType addressType;
            var isValid = Enum.TryParse(value, true, out addressType);
            if (isValid)
                return addressType;
            return null;
        }
    }
}