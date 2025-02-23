using System.ComponentModel.DataAnnotations;

namespace UserRoles.ViewModels
{
    public class LoginViewModel
    {

        [Required(ErrorMessage = "Email is requiresd.")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Paassword { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
