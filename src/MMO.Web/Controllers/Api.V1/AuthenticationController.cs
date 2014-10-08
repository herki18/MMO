using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using MMO.Base.Api.V1;
using MMO.Data;
using MMO.Data.Services;


namespace MMO.Web.Controllers.Api.V1
{
    [RoutePrefix("api/v1/authentication")]
    public class AuthenticationController : ApiController
    {
        private readonly MMODatabseContext _database = new MMODatabseContext();

        [Route("validate"), HttpPost]
        public HttpResponseMessage ValidateCredentials([FromBody]AuthValidateRequest request) {
            if (request == null || string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password)) {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new AuthValidateResponse(false));
            }
            var settingService = new MMOSettingService(_database);
            var user = _database.Users.SingleOrDefault(t => t.UserName == request.Username);
            if (user == null || !user.CheckPassword(request.Password) || !settingService.IsGameEnabledForuser(user)) {
                return Request.CreateResponse(HttpStatusCode.Unauthorized, new AuthValidateResponse(false));
            }

            return Request.CreateResponse(new AuthValidateResponse(true));
        }

        [Route("login"), HttpPost]
        public HttpResponseMessage GenerateToken([FromBody]AuthGenerateTokenRequest request) {
            if (request == null || string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new AuthGenerateTokenResponse(false, null));
            }
            var settingService = new MMOSettingService(_database);
            var user = _database.Users.SingleOrDefault(t => t.UserName == request.Username);
            if (user == null || !user.CheckPassword(request.Password) || !settingService.IsGameEnabledForuser(user))
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized, new AuthGenerateTokenResponse(false, null));
            }
            var tokenSerive = new ClientAuthenticationTokenService(_database);
            var requestIp = ((HttpContextWrapper)Request.Properties["MS_HttpContext"]).Request.UserHostAddress;
            return Request.CreateResponse(new AuthGenerateTokenResponse(true, tokenSerive.GenerateTokenFor(requestIp, user)));
        }
    }
}