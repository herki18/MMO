﻿using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using MMO.Base.Api.V1;
using MMO.Data;
using MMO.Data.Services;
using System.Data.Entity;
using Serilog;

namespace MMO.Web.Controllers.Api.V1
{
    [RoutePrefix("api/v1/authentication")]
    public class AuthenticationController : ApiController {
        //private static readonly Serilog.ILogger Log = Serilog.Log.ForContext<AuthenticationController>(); 
        private readonly MMODatabseContext _database = new MMODatabseContext();

        [Route("validate"), HttpPost]
        public HttpResponseMessage ValidateCredentials([FromBody]AuthValidateRequest request) {
            Log.Debug("Validate");
            try {
                if (request == null || string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new AuthValidateResponse(false));
                }
                Log.Debug("Request was correct");
                var settingService = new MMOSettingService(_database);
                Log.Debug("Setting service was correct");
                var user = _database.Users.Include(t => t.Roles).SingleOrDefault(t => t.UserName == request.Username);
                Log.Debug(user != null ? "Got user data from database" : "User is null");
                if (user == null || !user.CheckPassword(request.Password) || !settingService.IsGameEnabledForUser(user))
                {
                    return Request.CreateResponse(HttpStatusCode.Unauthorized, new AuthValidateResponse(false));
                }
                Log.Debug("User check was correct");
                
            }
            catch (Exception e) {     
                Log.Error("Exception was throwen {0}, Stack {1}", e.Message, e.StackTrace);
            }
            return Request.CreateResponse(new AuthValidateResponse(true));
        }

        [Route("login"), HttpPost]
        public HttpResponseMessage GenerateToken([FromBody]AuthGenerateTokenRequest request) {
            try {
                if (request == null || string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password)) {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new AuthGenerateTokenResponse(false, null));
                }
                var settingService = new MMOSettingService(_database);
                var user = _database.Users.Include(t => t.Roles).SingleOrDefault(t => t.UserName == request.Username);
                if (user == null || !user.CheckPassword(request.Password) || !settingService.IsGameEnabledForUser(user)) {
                    return Request.CreateResponse(HttpStatusCode.Unauthorized, new AuthGenerateTokenResponse(false, null));
                }
                var tokenSerive = new ClientAuthenticationTokenService(_database);
                var requestIp = ((HttpContextWrapper) Request.Properties["MS_HttpContext"]).Request.UserHostAddress;

                return Request.CreateResponse(new AuthGenerateTokenResponse(true, tokenSerive.GenerateTokenFor(requestIp, user)));
            }
            catch (Exception e) {
                Log.Error("Exception was throwen {0}, Stack {1}", e.Message, e.StackTrace);
            }

            return Request.CreateResponse(HttpStatusCode.InternalServerError, new AuthGenerateTokenResponse(false, null));
        }
    }
}