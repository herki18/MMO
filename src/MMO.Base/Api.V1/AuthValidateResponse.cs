using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MMO.Base.Api.V1
{
    public class AuthValidateResponse
    {
        public bool CredentailsAreValid { get; private set; }

        public AuthValidateResponse(bool credentailsAreValid) {
            CredentailsAreValid = credentailsAreValid;
        }
    }
}
