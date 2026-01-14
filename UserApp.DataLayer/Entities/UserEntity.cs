using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserApp.DataLayer.Entities
{
    public class UserEntity : BaseEntity
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public Common.Enums.UserRole Role { get; set; }
        public string PasswordHash { get; set; }
    }
}
