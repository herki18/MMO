using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MMO.Web.ViewModels
{
    public enum PasswordResetError
    {
        TokenNotFound,
        TokeExpired
    }
}