using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CareerTracker.Models;
using CareerTracker.DAL;

namespace CareerTracker.Controllers
{
    public class AdminController : Controller
    {
        private CTContext db = new CTContext();

        //
        // GET: /Admin/

        public ActionResult Index()
        {
            return View(db.UserProfiles.ToList());
        }

        //
        // GET: /Admin/Details/5

        public ActionResult Details(int id = 0)
        {
            UserProfile userprofile = db.UserProfiles.Find(id);
            if (userprofile == null)
            {
                return HttpNotFound();
            }
            return View(userprofile);
        }



        //
        // GET: /Admin/Edit/5

        public ActionResult Edit(int id = 0)
        {
            UserProfile userprofile = db.UserProfiles.Find(id);
            if (userprofile == null)
            {
                return HttpNotFound();
            }
            return View(userprofile);
        }

        //
        // POST: /Admin/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserProfile userprofile)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userprofile).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(userprofile);
        }

        //
        // GET: /Admin/Delete/5

        public ActionResult Delete(int id = 0)
        {
            UserProfile userprofile = db.UserProfiles.Find(id);
            if (userprofile == null)
            {
                return HttpNotFound();
            }
            return View(userprofile);
        }

        //
        // POST: /Admin/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserProfile userprofile = db.UserProfiles.Find(id);
            db.UserProfiles.Remove(userprofile);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Deactivate(int id = 0) 
        {
            UserProfile userprofile = db.UserProfiles.Find(id);
            if(userprofile == null)
            {
                return HttpNotFound();
            }
            return View(userprofile);
        }

        [HttpPost, ActionName("Deactivate")]
        [ValidateAntiForgeryToken]
        public ActionResult DeactivateConfirmed(int id) 
        {
            UserProfile userprofile = db.UserProfiles.Find(id);
            userprofile.active = false; 
            db.Entry(userprofile).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("DeactivatedIndex");
        }

        public ActionResult DeactivatedIndex() 
        {
            List<UserProfile> usrs = db.UserProfiles.Where(x => x.active == false).ToList();
            return View(usrs);
        }

        public ActionResult Activate(int id = 0)
        {
            UserProfile userprofile = db.UserProfiles.Find(id);
            if (userprofile == null)
            {
                return HttpNotFound();
            }
            return View(userprofile);
        }

        [HttpPost, ActionName("Activate")]
        [ValidateAntiForgeryToken]
        public ActionResult ActivateConfirmed(int id)
        {
            UserProfile userprofile = db.UserProfiles.Find(id);
            userprofile.active = true;
            db.Entry(userprofile).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("ActivatedIndex");
        }

        public ActionResult ActivatedIndex()
        {
            List<UserProfile> usrs = db.UserProfiles.Where(x => x.active == true).ToList();
            return View(usrs);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}