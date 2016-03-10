﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CareerTracker.Models;
using CareerTracker.DAL;
using CareerTracker.Security;

namespace CareerTracker.Controllers
{
    public class AdministratorController : Controller
    {
        private CTContext db = new CTContext();

        //
        //// GET: /Administrator/

        public ActionResult Index()
        {
			UserManager manager = new UserManager();
            return View(db.Users.ToList());
        }

        ////
        //// GET: /Administrator/Edit/5

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
		public ActionResult Edit(User userprofile) {
			if (ModelState.IsValid) {
				db.Users.Attach(userprofile);
				db.Entry(userprofile).Property(x => x.Active).IsModified = true;
				db.SaveChanges();
				return RedirectToAction("Index");
			}
			return View(userprofile);
		}


		protected override void Dispose(bool disposing) {
			db.Dispose();
			base.Dispose(disposing);
		}
    }
}