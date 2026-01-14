namespace heinrich_polak_4D_aspnet_2.Models
{
    public class ResetPasswordModel
    {
        public Guid UserPublicId { get; set; }
        public string NewPassword { get; set; }
    }
}
