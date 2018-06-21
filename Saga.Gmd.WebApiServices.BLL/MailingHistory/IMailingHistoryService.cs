using System.Collections.Generic;
using Saga.Gmd.WebApiServices.Models;


namespace Saga.Gmd.WebApiServices.BLL.MailingHistory
{
   public interface IMailingHistoryService
    {
        dynamic GetMailingHistoryList( Models.MailingHistory.MailingHistory mailingHistory, NameAndAddress nameAndAddress = null,IEnumerable<int> customerIdList = null);

    }
}
