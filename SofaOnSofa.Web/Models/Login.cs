using System.ComponentModel.DataAnnotations;

namespace SofaOnSofa.Web.Models
{
    public class Login
    {
        [Display(Name = "Почта")]
        public string Email { get; set; }
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}