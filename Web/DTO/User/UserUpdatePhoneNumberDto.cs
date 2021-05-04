using System.ComponentModel.DataAnnotations;

namespace BulletinBoardAPI.DTO.User
{
    public class UserUpdatePhoneNumberDto
    {
        [Phone]
        [Required(ErrorMessage = "Phone is required")]
        public string PhoneNumber { get; set; }
    }
}
