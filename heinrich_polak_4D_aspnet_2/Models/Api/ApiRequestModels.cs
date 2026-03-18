using System.ComponentModel.DataAnnotations;

namespace heinrich_polak_4D_aspnet_2.Models.Api
{
    public class ApiLoginRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }

    public class ApiRegisterRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public DateOnly DateOfBirth { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }
    }

    public class ApiAddToCartRequest
    {
        [Required]
        public Guid ItemPublicId { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; } = 1;
    }

    public class ApiUpdateCartItemRequest
    {
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }
}
