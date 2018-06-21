using Autofac;
using Saga.Gmd.WebApiServices.Api.Components;

namespace Saga.Gmd.WebApiServices.Api.DI
{
    public class GetRequestProcessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // AE: Feb 15 : Added DI injection of strategy class interface IGetRequestProcess -
            // Index keyed registration of multiple IGetPrequestProcess (strategy) implementations - Instances 
            // are identiified using enum members
            //
            // KeyValuePair
            builder.RegisterType<KeyValuePairStrategy>()
                .Keyed<IGetRequestProcess>(GetRequestProcessImplementations.KeyValuePair).InstancePerDependency();
            // V2 Implementation
            builder.RegisterType<KeyValuePairStrategyV2>()
                .Keyed<IGetRequestProcess>(GetRequestProcessImplementations.KeyValuePairV2).InstancePerDependency();

            // NameAndAddress
            builder.RegisterType<NameAndAddressStrategy>()
                .Keyed<IGetRequestProcess>(GetRequestProcessImplementations.NameAndAddress).InstancePerDependency();
            // V2 Implementation 
            builder.RegisterType<NameAndAddressStrategyV2>()
                .Keyed<IGetRequestProcess>(GetRequestProcessImplementations.NameAndAddressV2).InstancePerDependency();


            builder.RegisterType<PermissionsKeyValuePairStrategy>()
                .Keyed<IGetRequestProcess>(GetRequestProcessImplementations.PermissionsKeyValuePair).InstancePerDependency();

            base.Load(builder);
        }

    }
}