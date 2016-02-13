using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;

namespace MvcApp.Controllers
{
    public static class ControllerContextExtension
    {
        public static IOwinContext GetOwinContext(this ControllerContext context)
        {
            return context.HttpContext.GetOwinContext();
        }

        public static IAuthenticationManager Authentication(this ControllerContext context)
        {
            return context.GetOwinContext().Authentication;
        }

        public static ApplicationSignInManager GetApplicationSignInManager(this ControllerContext controllerContext)
        {
            return controllerContext.GetOwinContext().Get<ApplicationSignInManager>();
        }

        public static ApplicationUserManager GetUserManager(this ControllerContext controllerContext)
        {
            return controllerContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
        }
    }
}