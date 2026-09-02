using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.Models
{
    public class ChangePasswordModel
    {
        [Required(ErrorMessage = "Current Password is required")]
        [DataType(DataType.Password)]
        public string? CurrentPassword { get; set; }

        [Required(ErrorMessage = "New Password is required")]
        [MinLength(6, ErrorMessage = "New Password must be at least 6 characters")]
        [DataType(DataType.Password)]
        public string? NewPassword { get; set; }

        [Required(ErrorMessage = "Please confirm your new password")]
        [Compare("NewPassword", ErrorMessage = "New password and confirmation password do not match")]
        [DataType(DataType.Password)]
        public string? ConfirmPassword { get; set; }
    }
}
