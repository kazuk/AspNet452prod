using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using MvcApp.Properties;

namespace MvcApp.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Property_EMail" , ResourceType = typeof(Resources))]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Property_Code", ResourceType = typeof(Resources))]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Property_RememberBrowser", ResourceType = typeof(Resources))]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Property_EMail", ResourceType = typeof(Resources))]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Property_EMail", ResourceType = typeof(Resources))]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Property_Password", ResourceType = typeof(Resources))]
        public string Password { get; set; }

        [Display(Name = "Property_RememberMe", ResourceType = typeof(Resources))]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Property_EMail", ResourceType = typeof(Resources))]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessageResourceType = typeof (Resources), ErrorMessageResourceName = "ErrorMessage_Password_LengthError", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Property_Password", ResourceType = typeof(Resources))]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Property_PasswordConfirm", ResourceType = typeof(Resources))]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessageResourceType = typeof (Resources), ErrorMessageResourceName = "ErrorMessage_PasswordMissmatch")]
        public string ConfirmPassword { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Property_EMail", ResourceType = typeof(Resources))]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessageResourceType = typeof (Resources), ErrorMessageResourceName = "ErrorMessage_Password_LengthError", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Property_Password", ResourceType = typeof(Resources))]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(ResourceType = typeof (Resources), Name = "Property_PasswordConfirm")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessageResourceType = typeof (Resources), ErrorMessageResourceName = "ErrorMessage_PasswordMissmatch")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Property_EMail", ResourceType = typeof(Resources))]
        public string Email { get; set; }
    }
}
