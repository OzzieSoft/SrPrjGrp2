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

/*
 * This class controls the admin class, there are no pregenerated code in here. 
 * The methods here allow the admin to create and remove teachers, disable users, and and view people complete profiles.
 */

namespace CareerTracker.Controllers
{
    public class AdministratorController : Controller
    {
        private CTContext db = new CTContext();

        //The get method for the implementing teachers
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
        //The post method for adding a teacher claim.
        //Adds the teacher claim to users.
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

        //Get method to remove teachers.
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
        // Allows the admin to remove the teacher tag.

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
        //Gets the index
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
            return View(db.Users.ToList());
        }

        ////
        //// GET: /Administrator/Edit/5
        //Allows the administrator to turn off a user account so they can't access the system.
		[Authorize]
		public ActionResult Edit(string id) {
			UserManager manager = new UserManager();
			User user = manager.findById(id);
			bool role = manager.hasClaim(id, ClaimTypes.Role, "admin", false);
			if (role) {
				//ViewBag.AdminLockout = "Do not try to lock yourself out!";
				return RedirectToAction("Index", "Administrator");
			}
			if (user == null) {
				return HttpNotFound();
			}

			return View(user);
		}

        ////
        //// POST: /Administrator/Edit/5
        //Allows the admin to edit a user to allow them to not access the application.
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

        //allows the admin to view any persons profile with nothing hidden.
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