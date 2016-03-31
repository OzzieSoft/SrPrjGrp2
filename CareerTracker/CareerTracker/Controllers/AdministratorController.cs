using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CareerTracker.Models;
using CareerTracker.DAL;
using CareerTracker.Security;
using Microsoft.AspNet.Identity;
using System.Security.Claims;

namespace CareerTracker.Controllers
{
    public class AdministratorController : Controller
    {
        private CTContext db = new CTContext();

        //
        //// GET: /Administrator/
		[Authorize]
        public ActionResult Index()
        {
			var userId = (ClaimsIdentity)User.Identity;
			var claims = userId.Claims;
			var roleClaimType = userId.RoleClaimType;
			var roles = claims.Where(x => x.Type == ClaimTypes.Role).ToList();
			if(roles.Count == 0)
			{
				return RedirectToAction("Index", "Home");
			}
			UserManager manager = new UserManager();
            return View(db.Users.ToList());
        }

        ////
        //// GET: /Administrator/Edit/5
		[Authorize]
		public ActionResult Edit(string id) {
			UserManager manager = new UserManager();
			User user = manager.findById(id);
			if (user == null) {
				return HttpNotFound();
			}

			return View(user);
		}

        ////
        //// POST: /Administrator/Edit/5

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(User input) {
            UserManager manager = new UserManager();
            User user = manager.findById(input.Id);
            if (ModelState.IsValid) {
                //db.Users.Attach(userprofile);
                //db.Entry(userprofile).Property(x => x.Active).IsModified = true;
                //db.SaveChanges();

                user.Active = input.Active;
                manager.Update(user);

				return RedirectToAction("Index");
			}
			return View(user);
		}

		[AllowAnonymous]
		public ActionResult AdminProfileView(string id) {
			User prof;
			UserManager manager = new UserManager();
			prof = manager.FindById(id);
			return View(prof);
		}

		protected override void Dispose(bool disposing) {
			db.Dispose();
			base.Dispose(disposing);
		}
    }
}