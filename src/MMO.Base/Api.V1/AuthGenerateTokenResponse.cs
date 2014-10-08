using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MMO.Base.Api.V1
{
    public class AuthGenerateTokenResponse
    {
        public bool CredentailsAreValid { get; private set; }
        public string Token { get; private set; }

        public AuthGenerateTokenResponse(bool credentailsAreValid, string token) {
            CredentailsAreValid = credentailsAreValid;
            Token = token;
        }
    }
}
