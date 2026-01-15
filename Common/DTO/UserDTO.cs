namespace Common.DTO
{
    public class UserDTO
    {
        public Guid PublicId { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public Common.Enums.UserRole Role { get; set; }
        public string Password { get; set; }
    }
}
