using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Saga.Gmd.WebApiServices.Models;
using Saga.Gmd.WebApiServices.Models.MailingHistory;


namespace Saga.Gmd.WebApiServices.BLL
{
    public class DataShapedObject
    {
        public dynamic Create(MailingHistoryResult mailingHistory, List<string> listofFields)
        {
            List<string> lstOfFieldsToWorkWith = new List<string>(listofFields);

            if (!lstOfFieldsToWorkWith.Any())
            {
                return mailingHistory;
            }
            else
            {
                ExpandoObject objectToReturn = new ExpandoObject();
                foreach (var field in lstOfFieldsToWorkWith)
                {
                    if (field == "")
                    {
                        return mailingHistory;
                    }
                    var propertyInfo = mailingHistory.GetType()
                        .GetProperty(field, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    if (propertyInfo != null)
                    {
                        var fieldValue = propertyInfo
                            .GetValue(mailingHistory, null);


                        ((IDictionary<String, Object>)objectToReturn).Add(field, fieldValue);

                    }
                }
                return objectToReturn;
            }
        }
    }
}
