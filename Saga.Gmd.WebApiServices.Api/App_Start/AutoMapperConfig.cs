using AutoMapper;
using Saga.Gmd.WebApiServices.Common;
using Saga.Gmd.WebApiServices.Models;
using Saga.Gmd.WebApiServices.Models.Customer; 
using Saga.Gmd.WebApiServices.Models.Transaction;



namespace Saga.Gmd.WebApiServices.Api.App_Start
{
    public class AutoMapperConfig
    {
        public static void Configure()
        {
        
            Mapper.Initialize(cfg => cfg.AddProfile<CustomerloadProfile>());
        }
    }


    public class CustomerloadProfile : Profile
    {
        public CustomerloadProfile()
        {
            CreateMap<WebApiServices.Api.Models.ParameterModels.PermissionsCustomerLoad, WebApiServices.Models.Gdpr.PermissionsCustomerLoadModel>();
            CreateMap<WebApiServices.Api.Models.ParameterModels.ChannelFlagsPost, WebApiServices.Models.Gdpr.ChannelFlags>();
            CreateMap<WebApiServices.Api.Models.ParameterModels.CustomerLoad, WebApiServices.Models.Customer.CustomerLoad>();
            CreateMap<WebApiServices.Models.Transaction.TransactionCustomerLoad, WebApiServices.Models.Transaction.TransactionCustomerLoad>();
            CreateMap<WebApiServices.Api.Models.ParameterModels.TransactionParamter, WebApiServices.Models.Transaction.TransactionParamter>();
            CreateMap<CustomerAddress, WebApiServices.Models.Customer.CustomerAddress>();
            CreateMap<Transaction, WebApiServices.Models.Transaction.Transaction>();
            CreateMap<Api.Models.ParameterModels.CustomerLoadOrMatch, WebApiServices.Models.Customer.CustomerLoadOrMatch>();
            CreateMap<Saga.Gmd.WebApiServices.Api.Models.ParameterModels.MembershipDataInputModel, WebApiServices.Models.Membership.MembershipDataInput>();
            CreateMap<Saga.Gmd.WebApiServices.Api.Models.ParameterModels.CustomerKeyMoveParameter, WebApiServices.Models.Customer.CustomerKeyMove>();

            CreateMap<Saga.Gmd.WebApiServices.Api.Models.ParameterModels.MembershipCancellationParameterModel, WebApiServices.Models.Membership.MembershipDataInput>()
                .ForMember(dest => dest.UpdatedStatus, opt => opt.UseValue<string>(MembershipActionType.Cancel.ToString()))
                ;
            // GITCS-9 : Map custom cancellation param onto standard membership model 

            // GITCS-187 : Signal Membership Data input is a proxy for a ProductSold model...
            CreateMap<Saga.Gmd.WebApiServices.Api.Models.ParameterModels.MembershipProductSoldParameterModel,
                    WebApiServices.Models.Membership.MembershipDataInput>()
                .ForMember(dest => dest.UpdatedStatus, opt => opt.UseValue<string>(MembershipActionType.ProductSold.ToString()))
                ;


        }
    }
}