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
    public class GoalStepController : Controller
    {
        private CTContext db = new CTContext();

        //
        // GET: /GoalStep/

        public ActionResult Index()
        {
            return View(db.GoalSteps.ToList());
        }

        //
        // GET: /GoalStep/Details/5

        public ActionResult Details(int id = 0)
        {
            GoalStep goalstep = db.GoalSteps.Find(id);
            if (goalstep == null)
            {
                return HttpNotFound();
            }
            return View(goalstep);
        }

        //
        // GET: /GoalStep/Create

        public ActionResult Create(int goalid = 0)
        {
            //Session["goal"] = goalid;
            return View();
        }

        //
        // POST: /GoalStep/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GoalStep goalstep)
        {
            if (ModelState.IsValid)
            {
                int goalid = int.Parse(Session["goal"].ToString());
                Goal goal = db.Goals.Find(goalid);
                goal.Steps.Add(goalstep);
                db.GoalSteps.Add(goalstep);
                db.SaveChanges();
                return RedirectToAction("edit", "Goal", new { id = goalid });
            }

            return View(goalstep);
        }

        //
        // GET: /GoalStep/Edit/5

        public ActionResult Edit(int id = 0)
        {
            GoalStep goalstep = db.GoalSteps.Find(id);
            if (goalstep == null)
            {
                return HttpNotFound();
            }
            return View(goalstep);
        }

        //
        // POST: /GoalStep/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(GoalStep goalstep)
        {
            if (ModelState.IsValid)
            {
                db.Entry(goalstep).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(goalstep);
        }

        //
        // GET: /GoalStep/Delete/5

        public ActionResult Delete(int id = 0)
        {
            GoalStep goalstep = db.GoalSteps.Find(id);
            if (goalstep == null)
            {
                return HttpNotFound();
            }
            return View(goalstep);
        }

        //
        // POST: /GoalStep/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GoalStep goalstep = db.GoalSteps.Find(id);
            Goal goal = goalstep.Goal;
            goal.Steps.Remove(goalstep);
            db.GoalSteps.Remove(goalstep);
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