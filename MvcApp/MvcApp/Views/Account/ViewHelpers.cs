using System.Web.Mvc;
using System.Web.Mvc.Html;
using MvcApp.Controllers;

namespace MvcApp.Views.Account
{
    public static class ViewHelpers
    {
        public static MvcHtmlString AccountLoginLink(this HtmlHelper html, object htmlAttributes)
        {
            return html.ActionLink("ログインするにはここをクリック", AccountController.ActionNames.Login,  ControllerNames.Account, 
                routeValues: null, htmlAttributes: htmlAttributes);
        }

        public static MvcForm AccountExternalLoginForm(this HtmlHelper html, string returnUrl)
        {
            return html.BeginForm(AccountController.ActionNames.ExternalLogin, ControllerNames.Account, new { ReturnUrl = returnUrl });
        }

        public static MvcForm AccountExternalLoginConfirmationForm(this HtmlHelper html,
            string returnUrl, object htmlAttributes)
        {
            return html.BeginForm(
                AccountController.ActionNames.ExternalLoginConfirmation, 
                ControllerNames.Account,
                new {ReturnUrl = returnUrl}, FormMethod.Post,
                htmlAttributes);
        }

        public static MvcForm AccountForgotPasswordForm(this HtmlHelper html,
            object htmlAttributes)
        {
            return html.BeginForm(AccountController.ActionNames.ForgotPassword, ControllerNames.Account, FormMethod.Post,
                htmlAttributes);
        }

        public static MvcForm AccountLoginForm(this HtmlHelper html, string returnUrl, object htmlAttributes)
        {
            return html.BeginForm(AccountController.ActionNames.Login, ControllerNames.Account,
                new { ReturnUrl = returnUrl }, FormMethod.Post,
                htmlAttributes);
        }

        public static MvcHtmlString AccountRegisterLink(HtmlHelper htmlHelper)
        {
            return htmlHelper.ActionLink("新しいユーザーとして登録する", AccountController.ActionNames.Register, ControllerNames.Account);
        }

        public static MvcForm AccountRegisterForm(this HtmlHelper html, object htmlAttributes)
        {
            return html.BeginForm(AccountController.ActionNames.Register, ControllerNames.Account, FormMethod.Post,
                htmlAttributes);
        }

        public static MvcForm AccountResetPasswordForm(this HtmlHelper html, object htmlAttributes)
        {
            return html.BeginForm(AccountController.ActionNames.ResetPassword, ControllerNames.Account, FormMethod.Post,
                htmlAttributes);
        }

        public static MvcForm AccountSendCodeForm(this HtmlHelper html, string returnUrl, object htmlAttributes)
        {
            return html.BeginForm(AccountController.ActionNames.SendCode, ControllerNames.Account, new { ReturnUrl = returnUrl }, FormMethod.Post, htmlAttributes);
        }

        public static MvcForm AccountVerifyCodeForm(this HtmlHelper html, string returnUrl, object htmlAttributes)
        {
            return html.BeginForm(AccountController.ActionNames.VerifyCode, ControllerNames.Account, new { ReturnUrl = returnUrl }, FormMethod.Post, htmlAttributes);
        }
    }
}