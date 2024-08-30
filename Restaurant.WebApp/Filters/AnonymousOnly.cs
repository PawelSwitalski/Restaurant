using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json.Serialization;
using Restaurant.WebApp.Validation;
using System.Security.Claims;
using System.Security.Principal;

namespace Restaurant.WebApp.Filters
{
    public class AnonymousOnly : Attribute, IAsyncActionFilter
    {
        private readonly string actionName;
        private readonly string controllerName;

        public AnonymousOnly(
            string actionName,
            string controllerName)
        {
            this.actionName = actionName;
            this.controllerName = controllerName;
        }

        /// <summary>
        /// Redirect to specified Controller and Action if user is logged.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        /// <exception cref="AnonymousOnlyException"></exception>
        public async Task OnActionExecutionAsync(
            ActionExecutingContext context,
            ActionExecutionDelegate next)
        {
            IIdentity? identity = context.HttpContext.User.Identity;

            if (identity == null)
                throw new AnonymousOnlyException();

            if (identity.IsAuthenticated)
            {

                var controller = context.Controller as ControllerBase;
                if (controller != null)
                {
                    context.Result = controller
                        .RedirectToAction(actionName, controllerName);
                }
            }
            else
            {
                await next();
            }

        }

    }
}
