using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using log4net;
using Saga.Gmd.WebApiServices.Api.Infrastructure;
using Saga.Gmd.WebApiServices.Api.Models;
using Saga.Gmd.WebApiServices.BLL.Customer;
using Saga.Gmd.WebApiServices.DAL.Customer;
using Saga.Gmd.WebApiServices.Api.WebClients;
using System;
using System.Configuration;
using Saga.Gmd.WebApiServices.Api.Components;
using Saga.Gmd.WebApiServices.Api.Components.KeyValuePair;
using Saga.Gmd.WebApiServices.Api.Components.StrategyParts;
using Saga.Gmd.WebApiServices.DAL.DataAccessHandlers;
using Saga.Gmd.WebApiServices.Api.DI;
using Saga.Gmd.WebApiServices.DAL.Infrastructure;
using AutoMapper;

namespace Saga.Gmd.WebApiServices.Api.App_Start
{


    public class AutofacConfig
    {
        public static void Configure()
        {
            var builder = new ContainerBuilder();

            var config = GlobalConfiguration.Configuration;
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterType<ConnectionFactory>().As<IConnectionFactory>().InstancePerDependency();

            // Register all XXXDataAccess providers - to replace all individual DataAccess interface registrations
            var dataAccessAssy = Assembly.GetAssembly(typeof(CustomerDetailsDataAccess));
            builder.RegisterAssemblyTypes(dataAccessAssy)
                .Where(t => t.Name.EndsWith("DataAccess"))
                .AsImplementedInterfaces().InstancePerDependency();
            // Specifies that a type from a scanned assembly is registered as providing all of its implemented interfaces.
            // Ie: Public classes matching the where condition should be registered as providing all the interfaces they implement.

            // Register all XXXServices providers - to replace all individual Service interface registrations
            var bllAssy = Assembly.GetAssembly(typeof(CustomerDetailsService));
            builder.RegisterAssemblyTypes(bllAssy)
                .Where(t => t.Name.Contains("Service"))
                .AsImplementedInterfaces().InstancePerDependency();

            builder.RegisterType<ClientScopeService>().As<IClientScopeService>().InstancePerDependency();

            //builder.RegisterGeneric(typeof(LogService<>)).As(typeof(ILogService<>));
            builder.RegisterModule(new Log4NetInjectModule()); 
            builder.Register(c => LogManager.GetLogger(typeof(object))).As<ILog>().InstancePerRequest(); 

            builder.RegisterType<CustomerMatchDataAccessHandler>().As<ICustomerMatchDataAccessHandler>()
                .WithParameters(
                    new[]
                    {
                        new NamedParameter("mciConnectionString", ConfigurationManager.AppSettings["MciCrConnection"]),
                        new NamedParameter("writeParameterValuesToLog", Convert.ToBoolean(ConfigurationManager.AppSettings["EnableObjectDump"]))
                    }
                 )
                .InstancePerDependency();


            builder.RegisterType<KeyValueMembershipFlagStrategyReaderPart>().As<IKeyValueStrategyReaderPart>()
                .WithParameters(
                    new[]
                    {
                        new NamedParameter("logParameterValues", Convert.ToBoolean(ConfigurationManager.AppSettings["EnableObjectDump"]))
                    }
                )
                .InstancePerDependency();



            /*
            // AE: Feb 15 : Added DI injection of strategy class interface IGetRequestProcess -
            // Index keyed registration of multiple IGetPrequestProcess (strategy) implementations - Instances 
            // are identiified using enum members
            builder.RegisterType<KeyValuePairStrategy>()
                .Keyed<IGetRequestProcess>(GetRequestProcessImplementations.KeyValuePair ).InstancePerDependency();
            builder.RegisterType<NameAndAddressStrategy>()
                .Keyed<IGetRequestProcess>(GetRequestProcessImplementations.NameAndAddress).InstancePerDependency();
            builder.RegisterType<PermissionsKeyValuePairStrategy>()
                .Keyed<IGetRequestProcess>(GetRequestProcessImplementations.PermissionsKeyValuePair).InstancePerDependency();
                */

            builder.RegisterModule<GetRequestProcessModule>();

            // GITCS-1 - INameAndAddressStrategyReaderPart indexed registrations
            builder.RegisterType<NameAndAddressStrategyMembershipDetailsReaderPart>()
                .Keyed<INameAndAddressStrategyReaderPart>( NameAndAddressStrategyReaderImplementations.MembershipDetailsReaderPart )
                .WithParameters(
                    new[]
                    {
                        new NamedParameter("logParameterValues", Convert.ToBoolean(ConfigurationManager.AppSettings["EnableObjectDump"]))
                    }
                    )
                .InstancePerDependency();
            //
            builder.RegisterType<NameAndAddressStrategyMembershipOptionsReaderPart>()
                .Keyed<INameAndAddressStrategyReaderPart>(NameAndAddressStrategyReaderImplementations.MembershipOptionsReaderPart)
                .WithParameters(
                    new[]
                    {
                        new NamedParameter("logParameterValues", Convert.ToBoolean(ConfigurationManager.AppSettings["EnableObjectDump"]))
                    }
                )
                .InstancePerDependency();
            //
            builder.RegisterType<NameAndAddressStrategyMembershipOptionsReaderPartV2>()
                .Keyed<INameAndAddressStrategyReaderPart>(NameAndAddressStrategyReaderImplementations.MembershipOptionsReaderPartV2)
                .WithParameters(
                    new[]
                    {
                        new NamedParameter("logParameterValues", Convert.ToBoolean(ConfigurationManager.AppSettings["EnableObjectDump"]))
                    }
                )
                .InstancePerDependency();


            /*
            // GITCS-1
            builder.RegisterType<NameAndAddressStrategyMembershipReaderPart>().As<INameAndAddressStrategyReaderPart>()
                .WithParameters(
                    new[]
                    {
                        new NamedParameter("logParameterValues", Convert.ToBoolean(ConfigurationManager.AppSettings["EnableObjectDump"]))
                    }
                )
                .InstancePerDependency();
                */

            // AE Feb 18 : Register other component process classes 
            builder.RegisterType<CustomerDetailsProcess>().As<ICustomerDetailsProcess>().InstancePerDependency();
            builder.RegisterType<CustomerKeyProcess>().As<ICustomerKeyProcess>().InstancePerDependency();

            builder.RegisterModule<StrategyWriterPartModule>();

            // Bind multiple ctor params ... 
            builder.RegisterType<AfeWebApiClient>().As<IAfeWebApiClient>()
                .UsingConstructor(typeof(Uri), typeof(string))
                .WithParameters(
                    new[]
                    {
                        new NamedParameter("baseUri", new Uri(ConfigurationManager.AppSettings["AFEWebApiBaseUri"])),
                        new NamedParameter("targetActionName", ConfigurationManager.AppSettings["AFEWebApiTargetAction"] )
                    }
                )
				.InstancePerDependency();
				;

            // Register IMapper as the single static instance 
            builder.Register(c => Mapper.Instance).As<IMapper>().SingleInstance();

            var container = builder.Build();


            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

        }

    }
}