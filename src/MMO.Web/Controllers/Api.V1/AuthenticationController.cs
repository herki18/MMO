using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace MMO.Web.Controllers.Api.V1
{
    [RoutePrefix("api/v1/authentication")]
    public class AuthenticationController
    {
        [Route("validate"), HttpPost]
        public bool ValidateCredentials(string username, string password) {
            return false;
        }

        [Route("login"), HttpPost]
        public object GenerateToken(string username, string password) {
            return new {};
        }
    }
}