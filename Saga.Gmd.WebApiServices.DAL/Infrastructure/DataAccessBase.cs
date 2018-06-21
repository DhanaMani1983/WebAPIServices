namespace Saga.Gmd.WebApiServices.DAL.Infrastructure
{
    public class DataAccessBase
    {
        static DataAccessBase()
        {
             // Common DA once-only setup tasts 
             DapperConfig.Configure();
        }

    }
}
