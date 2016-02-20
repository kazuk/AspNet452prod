using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using MvcApp.Models;

namespace MvcApp.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        private static class ViewNames
        {
            public const string Index = "Index";
            public const string AddPhoneNumber = "AddPhoneNumber";
            public const string VerifyPhoneNumber = "VerifyPhoneNumber";
            public const string ChangePassword = "ChangePassword";
            public const string SetPassword = "SetPassword";
            public const string ManageLogins = "ManageLogins";
            public const string Error = "Error";
        }

        public static class ActionNames
        {
            public const string LinkLogin = "LinkLogin";
            public const string RemoveLogin = "RemoveLogin";
            public const string ChangePassword= "ChangePassword";
            public const string AddPhoneNumber = "AddPhoneNumber";
            public const string Index = "Index";
            public const string ManageLogins = "ManageLogins";
            public const string VerifyPhoneNumber = "VerifyPhoneNumber";
            public const string LinkLoginCallback = "LinkLoginCallback";
            public const string SetPassword = "SetPassword";
        }

        public ManageController()
        {
        }

        public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? ControllerContext.GetApplicationSignInManager();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? ControllerContext.GetUserManager();
            }
            private set
            {
                _userManager = value;
            }
        }

        private ActionResult IndexView(IndexViewModel model)
        {
            return View(ViewNames.Index, model);
        }

        private ActionResult AddPhoneNumberView()
        {
            return View(ViewNames.AddPhoneNumber);
        }

        private ActionResult AddPhoneNumberView(AddPhoneNumberViewModel model)
        {
            return View(ViewNames.AddPhoneNumber, model);
        }

        private ViewResult VerifyPhoneNumberView(string phoneNumber)
        {
            return View(ViewNames.VerifyPhoneNumber , new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
        }

        private ActionResult VerifyPhoneNumberView(VerifyPhoneNumberViewModel model)
        {
            return View(ViewNames.VerifyPhoneNumber, model);
        }

        private ActionResult ChangePasswordView()
        {
            return View(ViewNames.ChangePassword);
        }

        private ActionResult ChangePasswordView(ChangePasswordViewModel model)
        {
            return View( ViewNames.ChangePassword, model);
        }

        private ActionResult SetPasswordView()
        {
            return View(ViewNames.SetPassword);
        }

        private ActionResult SetPasswordView(SetPasswordViewModel model)
        {
            return View(ViewNames.SetPassword, model);
        }

        private ActionResult ManageLoginsView(ManageLoginsViewModel model)
        {
            return View( ViewNames. ManageLogins, model);
        }

        private ActionResult ErrorView()
        {
            return View(ViewNames.Error);
        }

        private ActionResult RedirectoToIndex()
        {
            return RedirectToAction( ActionNames.Index, ControllerNames.Manage);
        }

        private ActionResult RedirectToIndex(ManageMessageId messageId)
        {
            return RedirectToAction(ActionNames.Index, new {Message = messageId});
        }

        private RedirectToRouteResult RedirectToManageLogins()
        {
            return RedirectToAction(ActionNames.ManageLogins);
        }

        private ActionResult RedirectToManageLogins(ManageMessageId message)
        {
            return RedirectToAction(ActionNames.ManageLogins, new {Message = message});
        }

        private ActionResult RedirectToVerifyPhoneNumber(AddPhoneNumberViewModel model)
        {
            return RedirectToAction(ActionNames.VerifyPhoneNumber, new {PhoneNumber = model.Number});
        }

        private string LinkLoginCallbackUrl()
        {
            return Url.Action(ActionNames.LinkLoginCallback, ControllerNames.Manage);
        }

        private string UserId => User.Identity.GetUserId();

        private static IndexViewModel IndexViewModelFromUser(ApplicationUser user, bool browserRemembered)
        {
            return new IndexViewModel
            {
                HasPassword = user.PasswordHash != null,
                PhoneNumber = user.PhoneNumber,
                TwoFactor = user.TwoFactorEnabled,
                Logins = user.Logins.Select(l => new UserLoginInfo(l.LoginProvider, l.ProviderKey)).ToList(),
                BrowserRemembered = browserRemembered
            };
        }

        private async Task SignInAsync(ApplicationUser user)
        {
            await SignInManager.SignInAsync(user, false, false);
        }

        private static string GetStatusMessage(ManageMessageId message)
        {
            switch (message)
            {
                case ManageMessageId.ChangePasswordSuccess:
                    return Properties.Resources.Manage_Status_ChangePasswordSuccess;
                case ManageMessageId.SetPasswordSuccess:
                    return Properties.Resources.Manage_Status_SetPasswordSuccess;
                case ManageMessageId.SetTwoFactorSuccess:
                    return Properties.Resources.Manage_Status_SetTwoFactorSuccess;
                case ManageMessageId.Error:
                    return Properties.Resources.Manage_Status_Error;
                case ManageMessageId.AddPhoneSuccess:
                    return Properties.Resources.Manage_Status_AddPhoneSuccess;
                case ManageMessageId.RemovePhoneSuccess:
                    return Properties.Resources.Manage_Status_RemovePhoneSuccess;
                case ManageMessageId.RemoveLoginSuccess:
                    return Properties.Resources.Manage_Status_RemoveLoginSuccess;
                default:
                    return "";
            }
        }


        //
        // GET: /Manage/Index
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            ViewBag.StatusMessage = message==null ? "" : GetStatusMessage(message.Value);

            var user = await UserManager.FindByIdAsync(UserId);
            var browserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(UserId);
            var model = user == null
                ? new IndexViewModel { BrowserRemembered = browserRemembered }
                : IndexViewModelFromUser(user, browserRemembered);
            return IndexView(model);
        }

        //
        // POST: /Manage/RemoveLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            var result = await UserManager.RemoveLoginAsync(UserId, new UserLoginInfo(loginProvider, providerKey));
            if (!result.Succeeded)
            {
                return RedirectToManageLogins(ManageMessageId.Error);
            }
            var user = await UserManager.FindByIdAsync(UserId);
            if (user == null)
            {
                return RedirectToManageLogins(ManageMessageId.RemoveLoginSuccess);
            }
            await SignInAsync(user);
            return RedirectToManageLogins(ManageMessageId.RemoveLoginSuccess);
        }

        //
        // GET: /Manage/AddPhoneNumber
        public ActionResult AddPhoneNumber()
        {
            return AddPhoneNumberView();
        }

        //
        // POST: /Manage/AddPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return AddPhoneNumberView(model);
            }
            // トークンを生成して送信します。
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(UserId, model.Number);
            if (UserManager.SmsService == null)
                return RedirectToVerifyPhoneNumber(model);
            var message = new IdentityMessage
            {
                Destination = model.Number,
                Body = "あなたのセキュリティ コード: " + code
            };
            await UserManager.SmsService.SendAsync(message);
            return RedirectToVerifyPhoneNumber(model);
        }

        //
        // POST: /Manage/EnableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(UserId, true);
            var user = await UserManager.FindByIdAsync(UserId);
            if (user != null)
            {
                await SignInAsync(user);
            }
            return RedirectoToIndex();
        }

        //
        // POST: /Manage/DisableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DisableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(UserId, false);
            var user = await UserManager.FindByIdAsync(UserId);
            if (user != null)
            {
                await SignInAsync(user);
            }
            return RedirectoToIndex();
        }

        //
        // GET: /Manage/VerifyPhoneNumber
        public async Task<ActionResult> VerifyPhoneNumber(string phoneNumber)
        {
            // FIXME: code unused
            // ReSharper disable once UnusedVariable
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(UserId, phoneNumber);
            // 電話番号を確認するために SMS プロバイダー経由で SMS を送信します。
            return phoneNumber == null 
                ? ErrorView() 
                : VerifyPhoneNumberView(phoneNumber);
        }

        //
        // POST: /Manage/VerifyPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return VerifyPhoneNumberView(model);
            }
            var result = await UserManager.ChangePhoneNumberAsync(UserId, model.PhoneNumber, model.Code);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", Properties.Resources.Manage_VerifyPhoneNumber_Failed);
                return VerifyPhoneNumberView(model);
            }
            var user = await UserManager.FindByIdAsync(UserId);
            if (user != null)
            {
                await SignInAsync(user);
            }
            return RedirectToIndex(ManageMessageId.AddPhoneSuccess);
        }

        //
        // GET: /Manage/RemovePhoneNumber
        public async Task<ActionResult> RemovePhoneNumber()
        {
            var result = await UserManager.SetPhoneNumberAsync(UserId, null);
            if (!result.Succeeded)
            {
                return RedirectToIndex(ManageMessageId.Error);
            }
            var user = await UserManager.FindByIdAsync(UserId);
            if (user != null)
            {
                await SignInAsync(user);
            }
            return RedirectToIndex(ManageMessageId.RemovePhoneSuccess);
        }

        //
        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword()
        {
            return ChangePasswordView();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return ChangePasswordView(model);
            }
            var result = await UserManager.ChangePasswordAsync(UserId, model.OldPassword, model.NewPassword);
            if (!result.Succeeded)
            {
                AddErrors(result);
                return ChangePasswordView(model);
            }
            var user = await UserManager.FindByIdAsync(UserId);
            if (user != null)
            {
                await SignInAsync(user);
            }
            return RedirectToIndex(ManageMessageId.ChangePasswordSuccess);
        }

        //
        // GET: /Manage/SetPassword
        public ActionResult SetPassword()
        {
            return SetPasswordView();
        }

        //
        // POST: /Manage/SetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return SetPasswordView(model);
            var result = await UserManager.AddPasswordAsync(UserId, model.NewPassword);
            if (!result.Succeeded)
            {
                AddErrors(result);

                // ここに到達した場合は何らかの問題が発生しているので、フォームを再表示します。
                return SetPasswordView(model);
            }
            var user = await UserManager.FindByIdAsync(UserId);
            if (user != null)
            {
                await SignInAsync(user);
            }
            return RedirectToIndex(ManageMessageId.SetPasswordSuccess);
        }

        //
        // GET: /Manage/ManageLogins
        public async Task<ActionResult> ManageLogins(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.RemoveLoginSuccess ? Properties.Resources.Manage_Status_RemoveLoginSuccess
                : message == ManageMessageId.Error ? Properties.Resources.Manage_Status_Error
                : "";
            var user = await UserManager.FindByIdAsync(UserId);
            if (user == null)
            {
                return ErrorView();
            }
            var userLogins = await UserManager.GetLoginsAsync(UserId);
            var otherLogins = AuthenticationManager.GetExternalAuthenticationTypes()
                .Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider))
                .ToList();
            ViewBag.ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1;
            var model = new ManageLoginsViewModel
            {
                CurrentLogins = userLogins,
                OtherLogins = otherLogins,
                ExternalAuthenticationTypes = AuthenticationManager.GetAuthenticationTypes()
            };
            return ManageLoginsView(model);
        }

        //
        // POST: /Manage/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // 現在のユーザーのログインをリンクするために外部ログイン プロバイダーへのリダイレクトを要求します。
            return new ChallengeResult(provider, LinkLoginCallbackUrl(), UserId);
        }


        //
        // GET: /Manage/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, UserId);
            if (loginInfo == null)
            {
                return RedirectToManageLogins(ManageMessageId.Error);
            }
            var result = await UserManager.AddLoginAsync(User, loginInfo);
            return result.Succeeded 
                ? RedirectToManageLogins() 
                : RedirectToManageLogins(ManageMessageId.Error);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

// 外部ログインの追加時に XSRF の防止に使用します
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager => ControllerContext.Authentication();

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }
    }
}