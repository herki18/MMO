using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using MMO.Data;
using MMO.Data.Entities;
using MMO.Web.Mailers;
using MMO.Web.ViewModels;
using System.Data.Entity;

namespace MMO.Web.Controllers
{
    public class RegisterController : Controller
    {
        private readonly MMODatabseContext _databse = new MMODatabseContext();
        // GET: Register
        public ActionResult Create()
        {
            return View(new RegisterCreate());
        }

        [HttpPost]
        public ActionResult Create(RegisterCreate form) {
            if (_databse.Users.Any(t => t.UserName == form.Username))
            {
                ModelState.AddModelError("Username", "Usernames must be unique");
            }
            if (_databse.Users.Any(t => t.Email == form.Email))
            {
                ModelState.AddModelError("Email", "Emails must be unique");
            }

            if (!ModelState.IsValid) {
                return View(form);
            }

            var user = new User() {
                UserName = form.Username,
                Email = form.Email,
            };

            user.SetPassword(form.Password);

            user.GenerateEmailVerificationToken();

            // Send email with verify email token
            var mailer = new UserMailer();
            mailer.VerifyEmail(user).Send();

            _databse.Users.Add(user);
            _databse.SaveChanges();
            Auth.User = user;



            return RedirectToAction("Success");
        }

        [Authorize]
        public ActionResult Success() {
            return View(new RegisterSuccess {
                Username = Auth.User.UserName
            });
        }

        public ActionResult Verify(string token) {
            var user = _databse.Users.Include(t=>t.Roles).SingleOrDefault(t => t.VerifyEmailToken == token);
            if (user == null || user.VerifyEmailToken == null || !user.VerifyEmailToken.Equals(token, StringComparison.OrdinalIgnoreCase)) {
                return View(new RegisterVerify {
                    DidVerify = false
                });
            }

            user.Roles.Add(_databse.Roles.Single(t=>t.Name == "registered"));
            user.VerifyEmailToken = null;
            _databse.SaveChanges();

            return View(new RegisterVerify
            {
                DidVerify = true
            }); 
        }
    }
}