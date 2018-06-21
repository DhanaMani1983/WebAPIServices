using Dapper;
using Saga.Gmd.WebApiServices.DAL.Mapping;
using Saga.Gmd.WebApiServices.Models.Membership;

namespace Saga.Gmd.WebApiServices.DAL.Infrastructure
{
    public class DapperConfig
    {
        static readonly object _object = new object();
        private static bool dapperConfigured = false;

        public static void Configure()
        {
            lock (_object)
            {
                if (!dapperConfigured)
                {
                    // Set custom mapper for when MembershipDetails is mapped 
                    SqlMapper.SetTypeMap(typeof(MembershipDetails), MembershipDetailsTypeMap.GetMap() );

                    // Set custom mapper for when MembershipDetailsV2 is mapped 
                    // SqlMapper.SetTypeMap(typeof(MembershipDetailsV2), MembershipDetailsV2TypeMap.GetMap());
                    

                    // Also MembershipOptions 
                    // SqlMapper.SetTypeMap(typeof(MembershipOptions), MembershipOptionsTypeMap.GetMap());

                    
                    dapperConfigured = true;
                }
            }
        }

    }
}