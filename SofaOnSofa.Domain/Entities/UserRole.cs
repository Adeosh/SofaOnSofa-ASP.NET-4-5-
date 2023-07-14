using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SofaOnSofa.Domain.Entities
{
    public class UserRole
    {
        [Key]
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public List<User> Users { get; set; }
    }
}
