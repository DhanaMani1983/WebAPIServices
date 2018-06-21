using Autofac;
using Saga.Gmd.WebApiServices.Api.Components.StrategyParts;
using Saga.Gmd.WebApiServices.Models.Membership;

namespace Saga.Gmd.WebApiServices.Api.DI
{
    public class StrategyWriterPartModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {

            // Membership strategy writers : <MembershipDataInput, int, int, string>
            builder.RegisterType<StrategyMembershipProductSoldWriterPart<MembershipDataInput, int, int, string>>()
                .Keyed<IStrategyWriterPart<MembershipDataInput, int, int, string>>(StrategyWriterPartImplementations.ProductSoldMembershipWriterPart)
                .InstancePerDependency();

            builder.RegisterType<StrategyCancelMembershipWriterPart<MembershipDataInput, int, int, MembershipDetails>>()
                .Keyed<IStrategyWriterPart<MembershipDataInput, int, int, MembershipDetails>>(StrategyWriterPartImplementations.CancellMembershipWriterPart)
                .InstancePerDependency();


            base.Load(builder);
        }

    }
}