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


		[Authorize]
		public ActionResult TeacherEdit(string id) {
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
		public ActionResult TeacherEdit(User input) {
			UserManager manager = new UserManager();
			User user = manager.findById(input.Id);
			if (ModelState.IsValid) {
				manager.AddClaim(input.Id, new Claim(ClaimTypes.Role, "teacher")); 
				return RedirectToAction("Index");
			}
			return View(user);
		}

		[Authorize]
		public ActionResult TeacherRemove(string id) {
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
		public ActionResult TeacherRemove(User input) {
			UserManager manager = new UserManager();
			User user = manager.findById(input.Id);
			if (ModelState.IsValid) {
				manager.RemoveClaim(input.Id, new Claim(ClaimTypes.Role, "teacher"));
				return RedirectToAction("Index");
			}
			return View(user);
		} 

        //
        //// GET: /Administrator/
		[Authorize]
        public ActionResult Index()
        {
			UserManager manager = new UserManager();
			bool role = manager.hasClaim(User.Identity.Name, ClaimTypes.Role, "admin");
			if(!role)
			{
				return RedirectToAction("Index", "Home");
			}
			//UserManager manager = new UserManager();
			List<UserView> Determination = new List<UserView>();
			foreach (var user in db.Users.ToList())
			{
				Determination.Add(new UserView(user));
			}
            return View(Determination);
        }

        ////
        //// GET: /Administrator/Edit/5
		[Authorize]
		public ActionResult Edit(string id) {
			UserManager manager = new UserManager();
			User user = manager.findById(id);
			bool role = manager.hasClaim(id, ClaimTypes.Role, "admin", false);
			if (role) {
				ViewBag.AdminLockout = "Do not try to lock yourself out!";
				return RedirectToAction("Index", "Administrator");
			}
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
			ViewBag.Role = db.Categories.ToList();
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