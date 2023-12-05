using System.ComponentModel.DataAnnotations;

namespace Web_API.Models
{
    public class UserModel
    {
        [Key] public string Email { get; set; }
        [Required] public string Username { get; set; }
        [Required] public string Password { get; set; }
    }
}
