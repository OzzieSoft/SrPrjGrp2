using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OzzieSoftCareerTracker.Models;

namespace OzzieSoftCareerTracker.Controllers
{
    public class ArtifactController : Controller
    {
        private OzzieSoftCareerTrackerContext db = new OzzieSoftCareerTrackerContext();

        //
        // GET: /Artifact/

        public ActionResult Index()
        {
            return View(db.Artifacts.ToList());
        }

        //
        // GET: /Artifact/Details/5

        public ActionResult Details(int id = 0)
        {
            Artifact artifact = db.Artifacts.Find(id);
            if (artifact == null)
            {
                return HttpNotFound();
            }
            return View(artifact);
        }

        //
        // GET: /Artifact/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Artifact/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Artifact artifact)
        {
            if (ModelState.IsValid)
            {
                db.Artifacts.Add(artifact);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(artifact);
        }

        //
        // GET: /Artifact/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Artifact artifact = db.Artifacts.Find(id);
            if (artifact == null)
            {
                return HttpNotFound();
            }
            return View(artifact);
        }

        //
        // POST: /Artifact/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Artifact artifact)
        {
            if (ModelState.IsValid)
            {
                db.Entry(artifact).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(artifact);
        }

        //
        // GET: /Artifact/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Artifact artifact = db.Artifacts.Find(id);
            if (artifact == null)
            {
                return HttpNotFound();
            }
            return View(artifact);
        }

        //
        // POST: /Artifact/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Artifact artifact = db.Artifacts.Find(id);
            db.Artifacts.Remove(artifact);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}