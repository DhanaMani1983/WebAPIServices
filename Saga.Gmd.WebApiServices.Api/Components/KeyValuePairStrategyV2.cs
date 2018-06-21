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
    public class KeyValuePairStrategyV2 : KeyValuePairStrategy
    {
        public KeyValuePairStrategyV2(
            IMailingHistoryService mailingHistoryService,
            IMciRequestService mciRequestService,
            IClientScopeService clientScopeService,
            ILog logger,
            IPermissionService permissionsService,
            IMembershipService membershipService,
            ICustomerDetailsService customerDetailsService,
            ITravelSummaryService travelSummaryService,
            ICustomerMatchService customerMatchService,
            IKeyValueStrategyReaderPart membershipFlagsKeyValueReader,
            ICustomerDetailsProcess customerDetailsProcess,
            ICustomerKeyProcess customerKeyProcess,
            IIndex<NameAndAddressStrategyReaderImplementations, INameAndAddressStrategyReaderPart> nameAndAddressReaders

        ) : base(
            mailingHistoryService,
            mciRequestService,
            clientScopeService,
            logger,
            permissionsService,
            membershipService,
            customerDetailsService,
            travelSummaryService,
            customerMatchService,
            membershipFlagsKeyValueReader,
            customerDetailsProcess,
            customerKeyProcess,
            nameAndAddressReaders
            )
        {
            // Override Membership Options and details readers : Base class Execute() method will call the V2 implementation below
            _membershipOptionsReader =
                _nameAndAddressReaders[NameAndAddressStrategyReaderImplementations.MembershipOptionsReaderPartV2];
            // Set local ref to V2 reader 

        }


    }
}