namespace Saga.Gmd.WebApiServices.Models.Security
{
    public class ModuleAccess 
    {
        public  GroupCode  Key { get; set; }
        public bool HasAccess { get; set; }
    }
}