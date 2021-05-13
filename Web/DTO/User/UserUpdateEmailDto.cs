using System.ComponentModel.DataAnnotations;

namespace Web.DTO.User
{
    public class UserUpdateEmailDto
    {
        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string Email{ get; set; }
    }
}
