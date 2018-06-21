namespace Saga.Gmd.WebApiServices.Models.Security
{
    public class ClientScopes
    {
        public int SecScopeId { get; set; }
        public bool IsActive { get; set; }
        public string Code { get; set; }
        public string Scope { get; set; }
    }
}
