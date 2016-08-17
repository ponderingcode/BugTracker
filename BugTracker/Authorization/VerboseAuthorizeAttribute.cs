using System;
using System.Web.Mvc;

namespace BugTracker.Authorization
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class VerboseAuthorizeAttribute : AuthorizeAttribute
    {
        public string View { get; set; }

        public VerboseAuthorizeAttribute()
        {
            View = "AccessDenied";
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
            IsUserAuthorized(filterContext);
        }

        private void IsUserAuthorized(AuthorizationContext filterContext)
        {
            if (null == filterContext.Result)
            {
                return;
            }
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                ViewResult viewResult = new ViewResult();
                viewResult.ViewName = View;

                filterContext.Result = viewResult;
            }
        }
    }
}