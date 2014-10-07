using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Http.Filters;
using MMO.Data;
using MMO.Data.Entities;

namespace MMO.Web.Infrastructure
{
    public class AuthorizeDeployTokenAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext) {
            
            IEnumerable<string> tokenHeaderValues;

            if (!actionContext.Request.Headers.TryGetValues("deploy-token", out tokenHeaderValues)) {
                actionContext.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "deploy-token header is not present");
                return;
            }

            var token = tokenHeaderValues.FirstOrDefault();


            using (var database = new MMODatabseContext()) {
                var httpContext = (HttpContextWrapper) actionContext.Request.Properties["MS_HttpContext"];
                
                var deployToke = database.DeployTokens.SingleOrDefault(t => t.Token == token);

                if (deployToke == null || deployToke.IpAddress != httpContext.Request.UserHostAddress) {
                    actionContext.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "invalid deploy-token");
                }
            }
        }
    }
} 