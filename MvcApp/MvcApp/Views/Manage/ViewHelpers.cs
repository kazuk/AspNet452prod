using System.Web.Mvc;
using System.Web.Mvc.Html;
using MvcApp.Controllers;
using MvcApp.Models;

namespace MvcApp.Views.Manage
{
    public static class ViewHelpers
    {
        public static MvcForm ManageAddPhoneNumberForm(this HtmlHelper html, object htmlAttributes)
        {
            return html.BeginForm( ManageController.ActionNames.AddPhoneNumber , ControllerNames.Manage, FormMethod.Post, htmlAttributes);
        }
        public static MvcForm ManageChangePasswordForm(this HtmlHelper html, object htmlAttributes)
        {
            return html.BeginForm(ManageController.ActionNames.ChangePassword, ControllerNames.Manage, FormMethod.Post, htmlAttributes);
        }

        public static MvcHtmlString LinkManageChangePassword(this HtmlHelper html)
        {
            return html.ActionLink("パスワードの変更", ManageController.ActionNames.ChangePassword, ControllerNames.Manage);
        }

        public static MvcHtmlString LinkManageSetPassword(this HtmlHelper<IndexViewModel> html)
        {
            return html.ActionLink("作成", ManageController.ActionNames.SetPassword, ControllerNames.Manage);
        }

        public static MvcHtmlString LinkManageManageLogins(this HtmlHelper<IndexViewModel> html)
        {
            return html.ActionLink("管理", ManageController.ActionNames.ManageLogins, ControllerNames.Manage);
        }

        public static MvcForm ManageRemoveLoginForm(this HtmlHelper html)
        {
            return html.BeginForm(ManageController.ActionNames.RemoveLogin, ControllerNames.Manage);
        }

        public static MvcForm ManageLinkLoginForm(this HtmlHelper<ManageLoginsViewModel> html)
        {
            return html.BeginForm(ManageController.ActionNames.LinkLogin, ControllerNames.Manage);
        }

        public static MvcForm ManageSetPasswordForm(this HtmlHelper html, object htmlAttributes)
        {
            return html.BeginForm(ManageController.ActionNames.SetPassword, ControllerNames.Manage, FormMethod.Post, htmlAttributes);
        }

        public static MvcForm ManageVerifyPhoneNumberForm(this HtmlHelper html, object htmlAttributes)
        {
            return html.BeginForm(ManageController.ActionNames.VerifyPhoneNumber, ControllerNames.Manage, FormMethod.Post, htmlAttributes);
        }
    }
}
