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
            return View();
        }

        //
        // POST: /GoalStep/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GoalStep goalstep, int goalid)
        {
            if (ModelState.IsValid)
            {
                Goal goal = db.Goals.Find(goalid);
                goalstep.Goal = goal;
                goal.Steps.Add(goalstep);
                db.GoalSteps.Add(goalstep);
                db.SaveChanges();
                return RedirectToAction("edit", "Goal", new { id = goalid });
            }

            return View(goalstep);
        }

        //
        // GET: /GoalStep/Edit/5

        public ActionResult Edit(int id = 0, int goalID = 0)
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
        public ActionResult Edit(GoalStep goalstep, int goalID)
        {
            if (ModelState.IsValid)
            {   
                db.Entry(goalstep).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction(
                    actionName:"Edit",
                    controllerName:"Goal",
                    routeValues: new { id = goalID});
            }
            return View(goalstep);
        }

        //
        // GET: /GoalStep/Delete/5

        public ActionResult Delete(int id = 0, int returnto = -1)
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
        public ActionResult DeleteConfirmed(int id, int returnto)
        {
            GoalStep goalstep = db.GoalSteps.Find(id);
            Goal goal = goalstep.Goal;
            goal.Steps.Remove(goalstep);
            db.GoalSteps.Remove(goalstep);
            db.SaveChanges();

            ActionResult redirect;
            if (returnto == -1)
            {
                redirect = RedirectToAction("Index", "Goal");
            }
            else
            {
                redirect = RedirectToAction("Edit", "Goal", new { id = returnto });
            }
            return redirect;
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}