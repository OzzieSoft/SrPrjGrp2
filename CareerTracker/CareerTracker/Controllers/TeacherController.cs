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
    public class TeacherController : Controller
    {
        private CTContext db = new CTContext();

		[Authorize]
		public ActionResult Index() 
		{
			UserManager manager = new UserManager();
			var userId = (ClaimsIdentity)User.Identity;
			bool teacher = manager.hasClaim(User.Identity.Name, ClaimTypes.Role, "teacher");
			bool admin = manager.hasClaim(User.Identity.Name, ClaimTypes.Role, "admin");
			if (!teacher && !admin) {
				return RedirectToAction("Index", "Home");
			}
			return View(db.Users.ToList());
		}

		[AllowAnonymous]
		public ActionResult TeacherProfileView(string id) {
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