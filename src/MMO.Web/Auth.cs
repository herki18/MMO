using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;
using System.Web.UI.WebControls;
using Microsoft.ApplicationInsights.Core;
using System.Data.Entity;
using MMO.Data;
using MMO.Data.Entities;

namespace MMO.Web
{
    public static class Auth {
        private const string UserKey = "MMO.Web.Auth.UserKey";

        public static User User {
            get {
                if (!HttpContext.Current.User.Identity.IsAuthenticated) {
                    return null;
                }

                var user = HttpContext.Current.Items[UserKey] as User;
                if (user == null) {
                    var formsId = HttpContext.Current.User.Identity as FormsIdentity;
                    if (formsId == null) {
                        return null;
                    }

                    int userId;
                    if (!int.TryParse(formsId.Ticket.UserData, out userId)) {
                        return null;
                    }

                    using (var database = new MMODatabseContext()) {
                        user = database.Users.Include(t=>t.Roles).SingleOrDefault(t => t.Id == userId);
                    }

                    HttpContext.Current.Items[UserKey] = user;
                }

                return user;
            }
            set {
                var authCookie = FormsAuthentication.GetAuthCookie(value.UserName, true);
                var oldTicket = FormsAuthentication.Decrypt(authCookie.Value);
                var newTicket = new FormsAuthenticationTicket(oldTicket.Version, oldTicket.Name, oldTicket.IssueDate, oldTicket.Expiration,
                    oldTicket.IsPersistent, value.Id.ToString());
                authCookie.Value = FormsAuthentication.Encrypt(newTicket);
                HttpContext.Current.Response.Cookies.Add(authCookie);

            }
        }

        public static void LogOut() {
            FormsAuthentication.SignOut();
        }

    }
}