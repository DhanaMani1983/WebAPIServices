using Autofac.Features.Indexed;
using log4net;
using Saga.Gmd.WebApiServices.Api.Components.KeyValuePair;
using Saga.Gmd.WebApiServices.Api.Components.StrategyParts;
using Saga.Gmd.WebApiServices.Api.Models;
using Saga.Gmd.WebApiServices.BLL.Customer;
using Saga.Gmd.WebApiServices.BLL.MailingHistory;
using Saga.Gmd.WebApiServices.BLL.Membership;
using Saga.Gmd.WebApiServices.BLL.Permissions;

namespace Saga.Gmd.WebApiServices.Api.Components
{
    public class NameAndAddressStrategyV2 : NameAndAddressStrategy
    {

        public NameAndAddressStrategyV2(
            IMailingHistoryService mailingHistoryService,
            IMciRequestService mciRequestService, IClientScopeService clientScopeService,
            ILog logger,
            IPermissionService permissionService,
            IMembershipService membershipService,
            ICustomerMatchService customerMatchService,
            ICustomerDetailsService customerDetailsService,
            ITravelSummaryService travelSummaryService,
            ICustomerKeyProcess customerKeyProcess,
            IIndex<NameAndAddressStrategyReaderImplementations, INameAndAddressStrategyReaderPart> nameAndAddressReaders
        ) : base(
            mailingHistoryService,
            mciRequestService, 
            clientScopeService,
            logger,
            permissionService,
            membershipService,
            customerMatchService,
            customerDetailsService,
            travelSummaryService,
            customerKeyProcess,
            nameAndAddressReaders
            )
        {
            // Retrieve the V2 reader part for Membership Options 
            _membershipOptionsReader = _nameAndAddressReaders[NameAndAddressStrategyReaderImplementations.MembershipOptionsReaderPartV2];

        }

    }
}