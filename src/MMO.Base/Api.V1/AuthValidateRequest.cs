using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MMO.Base.Api.V1
{
    public class AuthValidateRequest
    {
        public string Username { get; private set; }
        public string Password { get; private set; }

        public AuthValidateRequest(string username, string password) {
            Username = username;
            Password = password;
        }
    }
}
