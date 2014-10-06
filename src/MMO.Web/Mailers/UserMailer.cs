using System.Web.UI.WebControls;
using MMO.Data.Entities;
using Mvc.Mailer;

namespace MMO.Web.Mailers
{ 
    public class UserMailer : MailerBase	
	{
		public UserMailer()
		{
			MasterName="_Layout";
		}
		
		public virtual MvcMailMessage VerifyEmail(User user)
		{
			ViewBag.Username = user.UserName;
		    ViewBag.Token = user.VerifyEmailToken;

			return Populate(x =>
			{
				x.Subject = "Please verify your email address to activate your MMO account";
				x.ViewName = "VerifyEmail";
				x.To.Add(user.Email);
			});
		}
 
		public virtual MvcMailMessage ResetPassword(User user)
		{
			ViewBag.Username = user.UserName;
		    ViewBag.Token = user.ResetPasswordToken;

			return Populate(x =>
			{
				x.Subject = "Reset your MMO account's password.";
				x.ViewName = "ResetPassword";
				x.To.Add(user.Email);
			});
		}
 	}
}