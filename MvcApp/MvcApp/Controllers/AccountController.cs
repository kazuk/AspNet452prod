using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using MvcApp.Models;

namespace MvcApp.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public static class ActionNames
        {
            public const string Login = "Login";
            public const string SendCode = "SendCode";
            public const string VerifyCode = "VerifyCode";
            public const string ExternalLoginCallback = "ExternalLoginCallback";
            public const string ResetPasswordConfirmation = "ResetPasswordConfirmation";
        }

        public static class ViewNames
        {
            public const string ExternalLoginFailure = "ExternalLoginFailure";
            public const string ExternalLoginConfirmation = "ExternalLoginConfirmation";
            public const string SendCode = "SendCode";
            public const string ResetPasswordConfirmation = "ResetPasswordConfirmation";
            public const string ResetPassword = "ResetPassword";
            public const string ForgotPasswordConfirmation = "ForgotPasswordConfirmation";
            public const string ForgotPassword = "ForgotPassword";
            public const string ConfirmEmail = "ConfirmEmail";
            public const string Register = "Register";
            public const string Error = "Error";
            public const string VerifyCode = "VerifyCode";
            public const string Lockout = "Lockout";
            public const string Login = "Login";
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

        // View Results
        private ActionResult LoginView()
        {
            ViewData["LoginProviders"] = ControllerContext.Authentication().GetExternalAuthenticationTypes();
            return View(ViewNames.Login);
        }

        private ActionResult LoginView(LoginViewModel model)
        {
            ViewData["LoginProviders"] = ControllerContext.Authentication().GetExternalAuthenticationTypes();
            return View(ViewNames.Login, model);
        }

        private ActionResult LockoutView()
        {
            return View(ViewNames.Lockout);
        }

        private ActionResult VerifyCodeView(VerifyCodeViewModel model)
        {
            return View(ViewNames.VerifyCode, model);
        }

        private ActionResult ErrorView()
        {
            return View(ViewNames.Error);
        }

        private ActionResult RegisterView()
        {
            return View(ViewNames.Register);
        }

        private ActionResult RegisterView(RegisterViewModel model)
        {
            return View(ViewNames.Register, model);
        }

        private ActionResult ConfirmEmailView()
        {
            return View(ViewNames.ConfirmEmail);
        }

        private ActionResult ForgotPasswordView()
        {
            return View(ViewNames.ForgotPassword);
        }

        private ActionResult ForgotPasswordView(ForgotPasswordViewModel model)
        {
            return View(ViewNames.ForgotPassword, model);
        }

        private ActionResult ForgotPasswordConfirmationView()
        {
            return View(ViewNames.ForgotPasswordConfirmation);
        }

        private ViewResult ResetPasswordView()
        {
            return View(ViewNames.ResetPassword);
        }

        private ActionResult ResetPasswordView(ResetPasswordViewModel model)
        {
            return View(ViewNames.ResetPassword, model);
        }

        private ActionResult ResetPasswordConfirmationView()
        {
            return View(ViewNames.ResetPasswordConfirmation);
        }

        private string ExternalLoginCallbackActionUrl(string returnUrl)
        {
            return Url.Action(ActionNames.ExternalLoginCallback, ControllerNames.Account, new { ReturnUrl = returnUrl });
        }

        private ActionResult SendCodeView()
        {
            return View(ViewNames.SendCode);
        }

        private ActionResult SendCodeView(SendCodeViewModel model)
        {
            return View(ViewNames.SendCode, model);
        }

        private ActionResult RedirectToVerifyCode(SendCodeViewModel model)
        {
            return RedirectToAction(ActionNames.VerifyCode,
                new { Provider = model.SelectedProvider, model.ReturnUrl, model.RememberMe });
        }

        private ActionResult ExternalLoginConfirmationView(ExternalLoginConfirmationViewModel model)
        {
            return View(ViewNames.ExternalLoginConfirmation, model);
        }

        private ActionResult ExternalLoginFailureView()
        {
            return View(ViewNames.ExternalLoginFailure);
        }

        // redirection results

        private RedirectToRouteResult RedirectToResetPasswordConfirmaion()
        {
            return RedirectToAction(ActionNames.ResetPasswordConfirmation, ControllerNames.Account);
        }

        private ActionResult RedirectToSendCode(string returnUrl)
        {
            return RedirectToAction(ActionNames.SendCode, new { ReturnUrl = returnUrl, RememberMe = false });
        }

        private ActionResult RedirectToSendCode(LoginViewModel model, string returnUrl)
        {
            return RedirectToAction(ActionNames.SendCode, new { ReturnUrl = returnUrl, model.RememberMe });
        }

        private ActionResult RedirectoToLogin()
        {
            return RedirectToAction(ActionNames.Login);
        }

        private ActionResult RedirectToManageIndex()
        {
            return RedirectToAction("Index", ControllerNames.Manage);
        }

        private ActionResult RedirectToHomeIndex()
        {
            return RedirectToAction("Index", ControllerNames.Home);
        }

        // actions

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return LoginView();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return LoginView(model);
            }

            // これは、アカウント ロックアウトの基準となるログイン失敗回数を数えません。
            // パスワード入力失敗回数に基づいてアカウントがロックアウトされるように設定するには、shouldLockout: true に変更してください。
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return LockoutView();
                case SignInStatus.RequiresVerification:
                    return RedirectToSendCode(model, returnUrl);
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", Properties.Resources.Account_Login_InvalidLoginAttempt); //  "無効なログイン試行です。"
                    return LoginView(model);
            }
        }


        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // ユーザーがユーザー名/パスワードまたは外部ログイン経由でログイン済みであることが必要です。
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return ErrorView();
            }
            var model = new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe };
            return VerifyCodeView(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return VerifyCodeView(model);
            }

            // 次のコードは、2 要素コードに対するブルート フォース攻撃を防ぎます。
            // ユーザーが誤ったコードを入力した回数が指定の回数に達すると、ユーザー アカウントは
            // 指定の時間が経過するまでロックアウトされます。
            // アカウント ロックアウトの設定は IdentityConfig の中で構成できます。
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent:  model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return LockoutView();
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", Properties.Resources.Account_VerifyCode_InvalidCode );
                    return VerifyCodeView( model);
            }
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            return RegisterView();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return RegisterView(model);
            var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
            var result = await UserManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);
                    
                // アカウント確認とパスワード リセットを有効にする方法の詳細については、http://go.microsoft.com/fwlink/?LinkID=320771 を参照してください
                // このリンクを含む電子メールを送信します
                // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                // await UserManager.SendEmailAsync(user.Id, "アカウントの確認", "このリンクをクリックすることによってアカウントを確認してください <a href=\"" + callbackUrl + "\">こちら</a>");

                return RedirectToHomeIndex();
            }
            AddErrors(result);

            // ここで問題が発生した場合はフォームを再表示します
            return RegisterView(model);
        }

        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return  ErrorView();
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return result.Succeeded 
                ? ConfirmEmailView() 
                : ErrorView();
        }

        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return ForgotPasswordView();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid) return ForgotPasswordView(model);
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
            {
                // ユーザーが存在しないことや未確認であることを公開しません。
                return ForgotPasswordConfirmationView();
            }

            // アカウント確認とパスワード リセットを有効にする方法の詳細については、http://go.microsoft.com/fwlink/?LinkID=320771 を参照してください
            // このリンクを含む電子メールを送信します
            // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
            // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
            // await UserManager.SendEmailAsync(user.Id, "パスワード", "のリセット <a href=\"" + callbackUrl + "\">こちら</a> をクリックして、パスワードをリセットしてください");
            // return RedirectToAction("ForgotPasswordConfirmation", "Account");

            // ここで問題が発生した場合はフォームを再表示します
            return ForgotPasswordView(model);
        }

        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return ForgotPasswordConfirmationView();
        }

        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? ErrorView() : 
                ResetPasswordView();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return ResetPasswordView(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // ユーザーが存在しないことを公開しません。
                return RedirectToResetPasswordConfirmaion();
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToResetPasswordConfirmaion();
            }
            AddErrors(result);
            return ResetPasswordView();
        }

        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return ResetPasswordConfirmationView();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // 外部ログイン プロバイダーへのリダイレクトを要求します
            return new ChallengeResult(provider, ExternalLoginCallbackActionUrl(returnUrl));
        }


        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return ErrorView();
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return SendCodeView(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }


        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return SendCodeView();
            }

            // トークンを生成して送信します。
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return ErrorView();
            }
            return RedirectToVerifyCode(model);
        }


        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await ControllerContext.Authentication().GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectoToLogin();
            }

            // ユーザーが既にログインを持っている場合、この外部ログイン プロバイダーを使用してユーザーをサインインします
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return LockoutView();
                case SignInStatus.RequiresVerification:
                    return RedirectToSendCode(returnUrl);
                case SignInStatus.Failure:
                default:
                    // ユーザーがアカウントを持っていない場合、ユーザーにアカウントを作成するよう求めます
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    var model = new ExternalLoginConfirmationViewModel { Email = loginInfo.Email };
                    return ExternalLoginConfirmationView(model);
            }
        }


        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToManageIndex();
            }

            if (!ModelState.IsValid)
            {
                ViewBag.ReturnUrl = returnUrl;
                return ExternalLoginConfirmationView(model);
            }

            // 外部ログイン プロバイダーからユーザーに関する情報を取得します
            var info = await ControllerContext.Authentication().GetExternalLoginInfoAsync();
            if (info == null)
            {
                return ExternalLoginFailureView();
            }
            var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
            var result = await UserManager.CreateAsync(user);
            if (result.Succeeded)
            {
                result = await UserManager.AddLoginAsync(user.Id, info.Login);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    return RedirectToLocal(returnUrl);
                }
            }
            AddErrors(result);
            ViewBag.ReturnUrl = returnUrl;
            return ExternalLoginConfirmationView(model);
        }


        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            ControllerContext.Authentication().SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToHomeIndex();
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return ExternalLoginFailureView();
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }


        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            return Url.IsLocalUrl(returnUrl) 
                ? Redirect(returnUrl) 
                : RedirectToHomeIndex();
        }
    }
}