namespace Saga.Gmd.WebApiServices.Models.Security
{
    public class CustomerKeyAccess  
    {
        public GroupCode Key { get; set; }
        public bool HasAccess { get; set; }
    }
}