using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DemoApplication.Atribute
{
    public class IsAuthenticated : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                RedirectToDashboard(context);
            }
        }

        private void RedirectToDashboard(ActionExecutingContext context)
        {
            var targetRedirect = new RouteValueDictionary
            {
                    {"action", "Dashboard"},
                  {"controller", "Account"}
            };

            context.Result = new RedirectToRouteResult(targetRedirect);
        }
    }
}
