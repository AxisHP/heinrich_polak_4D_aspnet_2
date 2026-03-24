using System.ComponentModel.DataAnnotations;
using Common.Enums;

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

    public class ApiCreateUserRequest
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

        [Required]
        public UserRole Role { get; set; }
    }

    public class ApiUpdateUserRequest
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
        public UserRole Role { get; set; }
    }

    public class ApiResetPasswordRequest
    {
        [Required]
        [MinLength(6)]
        public string NewPassword { get; set; }
    }

    public class ApiCreateOrUpdateItemRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue)]
        public int StockQuantity { get; set; }

        [Required]
        public Guid CategoryId { get; set; }
    }

    public class ApiCreateOrUpdateCategoryRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }
    }

    public class ApiUpdateOrderStatusRequest
    {
        [Required]
        public OrderStatus Status { get; set; }
    }
}
