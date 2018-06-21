namespace Saga.Gmd.WebApiServices.Models.Membership
{

    public class MembershipCancellationModel
    {
        public MembershipCancellationModel()
        {
        }
        public long? MembershipNo { get; set; }
        public string EncryptedMembershipNo { get; set; }


    }

}
