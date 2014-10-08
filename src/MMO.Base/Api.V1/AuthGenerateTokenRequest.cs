using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MMO.Base.Api.V1
{
    public class AuthGenerateTokenRequest
    {
        public string Username { get; private set; }
        public string Password { get; private set; }

        public AuthGenerateTokenRequest(string username, string password) {
            Username = username;
            Password = password;
        }
    }
}
