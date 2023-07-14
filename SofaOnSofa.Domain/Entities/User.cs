using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace SofaOnSofa.Domain.Entities
{
    public class User
    {
        public int UserId { get; set; }
        [Display(Name = "Имя")]
        public string UserName { get; set; }
        [Display(Name = "Пароль")]
        public string Password { get; set; }
        [Display(Name = "Почта")]
        public string Email { get; set; }
        public int RoleId { get; set; }

        [ForeignKey("RoleId")]
        public UserRole Role { get; set; }

        public User()
        {
            RoleId = 1;
        }
    }
}
