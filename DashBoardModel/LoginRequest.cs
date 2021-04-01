using System.ComponentModel.DataAnnotations;

namespace DashBoardModel
{
    public class LoginRequest
    {
        [Required]
        [StringLength(50)]
        public string UserId { get; set; }

        [Required]
        [StringLength(50)]
        public string Password { get; set; }
    }
}
