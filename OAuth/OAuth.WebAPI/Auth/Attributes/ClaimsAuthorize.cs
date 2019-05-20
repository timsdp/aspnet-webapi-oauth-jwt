namespace OAuth.WebAPI.Auth.Attributes
{
    using System;
    using System.Linq;
    using System.Security.Claims;
    using System.Web;
    using System.Web.Http;
    using System.Web.Http.Controllers;

    public class ClaimsAuthorize : AuthorizeAttribute
    {
        public int MinAge { get; set; }

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            ClaimsIdentity claimsIdentity;
            var httpContext = HttpContext.Current;
            if (!(httpContext.User.Identity is ClaimsIdentity))
            {
                return false;
            }

            claimsIdentity = httpContext.User.Identity as ClaimsIdentity;
            var ageClaim = claimsIdentity.FindFirst("Age");
            if (ageClaim == null)
            {
                return false;
            }

            int ageValue = Convert.ToInt32(ageClaim.Value);

            if (ageValue < this.MinAge)
            {
                return false;
            }

            //Continue with the regular Authorize check
            return base.IsAuthorized(actionContext);
        }
    }
}