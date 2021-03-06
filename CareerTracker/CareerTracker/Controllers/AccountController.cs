﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using CareerTracker.Filters;
using CareerTracker.Models;
using CareerTracker.DAL;
using CareerTracker.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System.Web.Helpers;
using System.Security.Claims;
using System.Threading.Tasks;
/*
 * This is the account controller, containing methods related to profiles and logging in/registering.
 * Anything involving claims can be found within the user manger class found in the security folder.
 * There is pre generated code within here, it will be marked. We did not use the pregenerated code, but did not delete it just in case someone works on this in the future. 
 */





namespace CareerTracker.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class AccountController : Controller
    {
        private CTContext db = new CTContext();

        //This method gets the information needed to log in by finding the user name through the name passed into the viewbag.
        [AllowAnonymous]
        public ActionResult ViewProfile()
        {
            UserManager manager = new UserManager();
            User prof;
			//ViewBag.Cats = db.Categories.ToList();
            prof = manager.findByUserName(User.Identity.Name);
            return View(prof);
        }

        //
        // GET: /Account/Login

        //This method returns the log in process to the home page and sends a cookie in the process.
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            SetANewRequestVerificationTokenManuallyInCookieAndOnTheForm();
            return View();
        }

        //
        // POST: /Account/Login

        //For the login page
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            //Creates the usermanager and finds the user name and password
            UserManager userManager = new UserManager();
            User user = userManager.Find(model.UserName, model.Password);
            //If there is a user
            if (user != null)
            {
                //If the user is inactive, redirects them to the warning
				if (!user.Active) 
				{
					return RedirectToAction("NotActive", "Home");
				}
                //Authenticates the user and signs them in, checking for the user being a admin or teacher and redirects them to the home page
                var authManager = System.Web.HttpContext.Current.GetOwinContext().Authentication;
                var userIdentity = userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                authManager.SignIn(new AuthenticationProperties() { IsPersistent = false }, userIdentity);
				if (userManager.hasClaim(user.Id, ClaimTypes.Role, "admin", false)) {
					ViewBag.admin = true;
				}
				if (userManager.hasClaim(user.Id, ClaimTypes.Role, "teacher", false)) {
					ViewBag.teacher = true;
				}
                return RedirectToAction("Index", "Home");
            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "The user name or password provided is incorrect.");
            return View(model);
        }


        //Pregend code, have not used.
        private void SetANewRequestVerificationTokenManuallyInCookieAndOnTheForm()
        {
            if (Response == null)
                return;

            string cookieToken, formToken;
            AntiForgery.GetTokens(null, out cookieToken, out formToken);
            SetCookie("__RequestVerificationToken", cookieToken);
            ViewBag.FormToken = formToken;
        }
        //Pregened code.
        private void SetCookie(string name, string value)
        {
            if (Response.Cookies.AllKeys.Contains(name))
                Response.Cookies[name].Value = value;
            else
                Response.Cookies.Add(new HttpCookie(name, value));
        }

        //
        // POST: /Account/LogOff

        [HttpPost]
        //[ValidateAntiForgeryToken]
        //Gets the post for log off, uses pregenerated code.
        public ActionResult LogOff()
        {
            System.Web.HttpContext.Current.GetOwinContext().Authentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register
        //Get for register, just returns the form.
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register

        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //The post method for registering a user, the exact details can be found below.
        public ActionResult Register(RegisterModel model)
        {
            //checks if the model is valid
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                try
                {
                    UserManager manager = new UserManager();

                    // checks to see if the username exists.
                    if (manager.FindByName(model.UserName) != null)
                    {
                        ViewBag.UserNameValidation = "That username is taken!";
                        return View(model);
                    }

                    //Checks that they are born after 1900 and before the current year minus 15.
					int checkYear = 1900;
                    int checkYear2 =  DateTime.Now.Year - 15;

					int inYear = model.DateOfBirth.Year;
					if (inYear < checkYear || checkYear2 < inYear) {
                            ViewBag.DateValidation = "Please enter a date between 1900 and " + checkYear2 + ".";
                            return View(model);
					}

                    //Sets the user's infomation
                    
                    User user = new User() {
                        UserName = model.UserName,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        DateOfBirth = model.DateOfBirth,
                        Email = model.Email,
                        Active = true
                    };
                    //And creates them in the manager
                    IdentityResult result = manager.Create(user, model.Password);

                    //if the user was created
                    if (result.Succeeded)
                    {
                        //ViewBag.Title = string.Format("User {0} was created successfully!", user.UserName);
                        
                        // for testing, this will be taken out.
                        if (model.UserName.Equals("SymAdmin"))
                        {
                            manager.AddClaim(manager.getIdFromUsername(model.UserName), new Claim(ClaimTypes.Role, "admin"));
                            ViewBag.message = manager.GetClaims(manager.getIdFromUsername(model.UserName)).FirstOrDefault();
                        }

                        // log in
                        System.Web.HttpContext.Current.GetOwinContext().Authentication.SignIn(
                            new AuthenticationProperties() { IsPersistent = false },
                            manager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie)
                        );
                    }
                    else
                    {
                        ViewBag.message = result.Errors.FirstOrDefault();
                    }

                    return RedirectToAction("Index", "Home");
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/Disassociate
        //Pregend code, did not use.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Disassociate(string provider, string providerUserId)
        {
            string ownerAccount = OAuthWebSecurity.GetUserName(provider, providerUserId);
            ManageMessageId? message = null;
            
            // Only disassociate the account if the currently logged in user is the owner
            if (ownerAccount == User.Identity.Name)
            {
                // Use a transaction to prevent the user from deleting their last login credential
                using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
                    if (hasLocalAccount || OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name).Count > 1)
                    {
                        OAuthWebSecurity.DeleteAccount(provider, providerUserId);
                        scope.Complete();
                        message = ManageMessageId.RemoveLoginSuccess;
                    }
                }
            }

            return RedirectToAction("Manage", new { Message = message });
        }

        //
        // GET: /Account/Manage
        //Pregend code.
        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : "";
            ViewBag.HasLocalPassword = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        //
        // POST: /Account/Manage
        //Pregend code to allow the user to change thier passwords.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(LocalPasswordModel model)
        {
            bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.HasLocalPassword = hasLocalAccount;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasLocalAccount)
            {
                if (ModelState.IsValid)
                {
                    // ChangePassword will throw an exception rather than return false in certain failure scenarios.
                    bool changePasswordSucceeded;
                    try
                    {
                        changePasswordSucceeded = WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
                    }
                    catch (Exception)
                    {
                        changePasswordSucceeded = false;
                    }

                    if (changePasswordSucceeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                    }
                }
            }
            else
            {
                // User does not have a local password so remove any validation errors caused by a missing
                // OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        WebSecurity.CreateAccount(User.Identity.Name, model.NewPassword);
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError("", String.Format("Unable to create local account. An account with the name \"{0}\" may already exist.", User.Identity.Name));
                    }
                }
            }

            return View(model);
        }

        //Searches for a user based on thier first name, last name and user name.
		[AllowAnonymous]
		public ActionResult SearchIndex(string searchString) 
		{
            UserManager manager = new UserManager();
			var users = from x in manager.Users select x;

			if (!String.IsNullOrEmpty(searchString)) 
			{
				users = users.Where(s => s.UserName.Contains(searchString) || s.FirstName.Contains(searchString) 
					|| s.LastName.Contains(searchString));
			}

			return View(users);
		}

		//Sets up the outside profile view.
		[AllowAnonymous]
		public ActionResult OutsideProfileView(string id) {
			User prof;
			//ViewBag.Cats = db.Categories.ToList();
            UserManager manager = new UserManager();
			prof = manager.FindById(id);
			return View(prof);
		}

        //The get for the editing of description.
		[Authorize]
		public ActionResult EditDesc(string id) {
			UserManager manager = new UserManager();
			User user = manager.findById(id);
			if (user == null) {
				return HttpNotFound();
			}

			return View(user);
		}

        //Posts the description when you edit it.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult EditDesc(User input) {
			UserManager manager = new UserManager();
			User user = manager.findById(input.Id);
			if (ModelState.IsValid) {
				user.Description = input.Description;
				manager.Update(user);

				return RedirectToAction("ViewProfile");
			}
			return View(user);
		}

        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

        internal class ExternalLoginResult : ActionResult
        {
            public ExternalLoginResult(string provider, string returnUrl)
            {
                Provider = provider;
                ReturnUrl = returnUrl;
            }

            public string Provider { get; private set; }
            public string ReturnUrl { get; private set; }

            public override void ExecuteResult(ControllerContext context)
            {
                OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
            }
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
