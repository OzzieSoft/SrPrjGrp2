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

namespace CareerTracker.Controllers
{
    public class GoalController : Controller
    {
        private CTContext db = new CTContext();

        //
        // GET: /Goal/
        [Authorize]
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                UserManager manager = new UserManager();
                List<Goal> returnList = new List<Goal>();
                try
                {
                    foreach (Goal g in db.Goals.ToList())
                    {
                        if (g.User.Id.Equals(manager.getIdFromUsername(User.Identity.Name)))
                        {
                            returnList.Add(g);
                        }
                    }
                }
                catch (NullReferenceException e) { }
                //ViewBag.Cats = db.Categories.ToList();
                return View(returnList);
            }
            return RedirectToAction("NotLoggedIn", "Home");
        }

        //
        // GET: /Goal/Details/5
        [Authorize]
        public ActionResult Details(int id = 0)
        {
            if (User.Identity.IsAuthenticated)
            {
                Goal goal = db.Goals.Find(id);
                if (goal == null)
                {
                    return HttpNotFound();
                }
                return View(goal);
            }
            return RedirectToAction("NotLoggedIn", "Home");
        }

        //
        // GET: /Goal/Create
        [Authorize]
        public ActionResult Create()
        {
            if (User.Identity.IsAuthenticated)
            {
                //ViewBag.Cats = db.Categories.ToList();
                return View();
            }
            return RedirectToAction("NotLoggedIn", "Home");
        }

        //
        // POST: /Goal/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Goal goal)
        {
            if (ModelState.IsValid)
            {
                int checkYear = 1900;

                int inYear = goal.DueDate.Year;
                if (inYear < checkYear)
                {
                    ViewBag.DateValidation = "Please enter a date between 1900 and now.";
                    return View();
                }
                UserManager manager = new UserManager(db);
                goal.User = manager.findByUserName(User.Identity.Name);
                db.Goals.Add(goal);
                db.SaveChanges();
                return RedirectToAction("Edit", new { id = goal.ID });
            }

            return View(goal);
        }

        //
        // GET: /Goal/Edit/5
        [Authorize]
        public ActionResult Edit(int id = 0)
        {
            if (User.Identity.IsAuthenticated)
            {
                Goal goal = db.Goals.Find(id);
                if (goal == null)
                {
                    return HttpNotFound();
                }

                //ViewBag.Cats = db.Categories.ToList();
                Session["goal"] = id;
                return View(goal);
            }
            return RedirectToAction("NotLoggedIn", "Home");
        }

        //
        // POST: /Goal/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Goal goal)
        {
            //Goal oldGoal = db.Goals.Find(goal.ID);
            if (ModelState.IsValid)
            {

                int checkYear = 1900;

                int inYear = goal.DueDate.Year;
                if (inYear < checkYear)
                {
                    ViewBag.DateValidation = "Please enter a date between 1900 and now.";
                    return View(goal);
                }
                db.Entry(goal).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(goal);
        }

        //
        // GET: /Goal/Delete/5
        [Authorize]
        public ActionResult Delete(int id = 0)
        {
            if (User.Identity.IsAuthenticated)
            {
                Goal goal = db.Goals.Find(id);
                if (goal == null)
                {
                    return HttpNotFound();
                }
                return View(goal);
            }
            return RedirectToAction("NotLoggedIn", "Home");
        }

        //
        // POST: /Goal/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Goal goal = db.Goals.Find(id);
            db.Goals.Remove(goal);
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