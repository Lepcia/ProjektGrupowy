using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace ProjektGrupowyGis.Models
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Login")]
        public string Login { get; set; }

        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage ="{0} is required!")]
        [Display(Name = "Login")]
        public string Login { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "{0} is required!!")]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(100, ErrorMessage = "{0} musi mieć przynajmniej {2} znaków", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}