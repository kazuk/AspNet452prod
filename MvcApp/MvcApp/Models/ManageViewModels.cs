﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using MvcApp.Properties;

namespace MvcApp.Models
{
    public class IndexViewModel
    {
        public bool HasPassword { get; set; }
        public IList<UserLoginInfo> Logins { get; set; }
        public string PhoneNumber { get; set; }
        public bool TwoFactor { get; set; }
        public bool BrowserRemembered { get; set; }
    }

    public class ManageLoginsViewModel
    {
        public IList<UserLoginInfo> CurrentLogins { get; set; }
        public IList<AuthenticationDescription> OtherLogins { get; set; }
    }

    public class FactorViewModel
    {
        public string Purpose { get; set; }
    }

    public class SetPasswordViewModel
    {
        [Required]
        [StringLength(100, ErrorMessageResourceType = typeof (Resources), ErrorMessageResourceName = "ErrorMessage_Password_LengthError", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof (Resources), Name = "Property_NewPassword")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(ResourceType = typeof (Resources), Name = "Property_NewPasswordConfirm")]
        [Compare("NewPassword", ErrorMessageResourceType = typeof (Resources), ErrorMessageResourceName = "ErrorMessage_PasswordMissmatch_New")]
        public string ConfirmPassword { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof (Resources), Name = "Property_CurrentPassword")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessageResourceType = typeof (Resources), ErrorMessageResourceName = "ErrorMessage_Password_LengthError", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof (Resources), Name = "Property_NewPassword")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(ResourceType = typeof (Resources), Name = "Property_NewPasswordConfirm")]
        [Compare("NewPassword", ErrorMessageResourceType = typeof (Resources), ErrorMessageResourceName = "ErrorMessage_PasswordMissmatch_New")]
        public string ConfirmPassword { get; set; }
    }

    public class AddPhoneNumberViewModel
    {
        [Required]
        [Phone]
        [Display(ResourceType = typeof (Resources), Name = "Property_PhoneNumber")]
        public string Number { get; set; }
    }

    public class VerifyPhoneNumberViewModel
    {
        [Required]
        [Display(ResourceType = typeof (Resources), Name = "Property_Code")]
        public string Code { get; set; }

        [Required]
        [Phone]
        [Display(ResourceType = typeof (Resources), Name = "Property_PhoneNumber")]
        public string PhoneNumber { get; set; }
    }

    public class ConfigureTwoFactorViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
    }
}