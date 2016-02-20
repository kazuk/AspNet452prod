using System.Web.Mvc;
using System.Web.Mvc.Html;
using MvcApp.Controllers;

namespace MvcApp.Views.Shared
{
    public static class ViewHelpers
    {
        public static MvcHtmlString ApplicationRootLink(this HtmlHelper html, object routeValues, object htmlAttributes)
        {
            return html.ActionLink("アプリケーション名", HomeController.ActionNames.Index, ControllerNames.Home, routeValues, htmlAttributes);
        }

        public static MvcHtmlString LinkHomeIndex(this HtmlHelper html)
        {
            return html.ActionLink("ホーム", HomeController.ActionNames.Index, ControllerNames.Home);
        }

        public static MvcHtmlString LinkHomeAbout(this HtmlHelper<dynamic> html)
        {
            return html.ActionLink("詳細", HomeController.ActionNames.About, ControllerNames.Home);
        }

        public static MvcHtmlString LinkHomeContact(this HtmlHelper<dynamic> html)
        {
            return html.ActionLink("連絡先", HomeController.ActionNames.Contact, ControllerNames.Home);
        }

        public static MvcForm AccountLogOffForm(this HtmlHelper html, object htmlAttributes)
        {
            return html.BeginForm(AccountController.ActionNames.LogOff, ControllerNames.Account, FormMethod.Post, htmlAttributes);
        }

        public static MvcHtmlString LinkManageIndex(this HtmlHelper html, string userName, object htmlAttributes)
        {
            return html.ActionLink("こんにちは、" + userName + "さん", ManageController.ActionNames.Index, ControllerNames.Manage,
                routeValues: null, htmlAttributes: htmlAttributes);
        }

        public static MvcHtmlString LinkAccountRegister(this HtmlHelper<dynamic> html, object htmlAttributes)
        {
            return html.ActionLink("登録", AccountController.ActionNames.Register, ControllerNames.Account, routeValues: null, htmlAttributes: htmlAttributes);
        }

        public static MvcHtmlString LinkAccountLogin(this HtmlHelper<dynamic> html, object htmlAttributes)
        {
            return html.ActionLink("ログイン", AccountController.ActionNames.Login, ControllerNames.Account, routeValues: null, htmlAttributes: htmlAttributes);
        }
    }
}
