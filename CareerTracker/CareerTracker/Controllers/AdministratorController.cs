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



/*This is the admin controller, involving anything that the admin can do
 * This is currently limited to making someone a teacher and deactivating user accounts
 * They can also view profiles with private information show.
 * Anything involving claims can be found within the user manger class found in the security folder.
 */

namespace CareerTracker.Controllers {
	public class AdministratorController : Controller {
		private CTContext db = new CTContext();

        //Get method for creating a teacher
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
        //Post method for making a user a teacher
		[HttpPost]
		[ValidateAntiForgeryToken]
        //Accepts a user for input,
		public ActionResult TeacherEdit(User input) {
			UserManager manager = new UserManager();
            //Finds the user by thier ID and then if the model state is valid adds a teacher claim to them.
			User user = manager.findById(input.Id);
			if (ModelState.IsValid) {
				manager.AddClaim(input.Id, new Claim(ClaimTypes.Role, "teacher"));
				return RedirectToAction("Index");
			}
			return View(user);
		}

        //Get method for teacher removal
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
        /// <summary>
        /// Removes a teacher claim from a user
        /// </summary>
        /// <param name="input">
        /// The user to remove the teacher claim from</param>
        /// <returns>To the index if the model is valid, to the page if not</returns>
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
        /// <summary>
        /// Returns the user to the home page if they are not the admin, otherwise displays a list containing the users
        /// </summary>
        /// <returns></returns>
		[Authorize]
		public ActionResult Index() {
			UserManager manager = new UserManager();
			bool role = manager.hasClaim(User.Identity.Name, ClaimTypes.Role, "admin");
			if (!role) {
				return RedirectToAction("Index", "Home");
			}
			//UserManager manager = new UserManager();
			List<UserView> Determination = new List<UserView>();//Blame Kyle for all bad variable names.
			foreach (var user in db.Users.ToList()) {
				Determination.Add(new UserView(user));
			}
			return View(Determination);
		}

		////
		//// GET: /Administrator/Edit/5
        /// <summary>
        /// Get method for locking out users, prevents admins from locking themselves out
        /// </summary>
        /// <param name="id">The id of the user to be edited passed as a string</param>
        /// <returns>To the index if succesful, to the page if not</returns>
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
        /// <summary>
        /// Updates a users information based on if they are locked out or not
        /// </summary>
        /// <param name="input">The user to be changed</param>
        /// <returns>To the index</returns>
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

        /// <summary>
        /// Allows the admin to view the profile view of all users, including private information
        /// </summary>
        /// <param name="id">The user to view</param>
        /// <returns></returns>
		[AllowAnonymous]
		public ActionResult AdminProfileView(string id) {
			User prof;
			ViewBag.Role = db.Categories.ToList();
			UserManager manager = new UserManager();
			prof = manager.FindById(id);
			return View(prof);
		}

        //Auto generated code, not touching it
		protected override void Dispose(bool disposing) {
			db.Dispose();
			base.Dispose(disposing);
		}
	}
}