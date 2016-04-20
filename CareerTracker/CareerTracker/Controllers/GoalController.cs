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
            if(User.Identity.IsAuthenticated){
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
                return View(new GoalView());
            }
            return RedirectToAction("NotLoggedIn", "Home");
        }

        //
        // POST: /Goal/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GoalView goalView)
        {
            if (ModelState.IsValid)
            {
                int checkYear = 1900;

                int inYear = goalView.GoalObj.DueDate.Year;
                if (inYear < checkYear)
                {
                    ViewBag.DateValidation = "Please enter a date after 1900";
                    return View(goalView);
                }
                List<Category> cats = db.Categories.ToList();           // get the category list from the db only once.
                goalView.GoalObj.Categories = new List<Category>();   // initialize the Goal's Category List, since it starts null.
                foreach (string k in goalView.CategoriesList.Keys)
                {
                    // if the user selected this key name
                    if (goalView.CategoriesList[k])
                    {
                        Category cat = cats.First(c => c.Name == k);
                        // add the category matching the key name
                        goalView.GoalObj.Categories.Add(cat);
                        cat.Goals.Add(goalView.GoalObj);
                        db.Entry(cat).State = EntityState.Modified;
                    }
                }

                UserManager manager = new UserManager(db);
                goalView.GoalObj.User = manager.findByUserName(User.Identity.Name);
                db.Goals.Add(goalView.GoalObj);
                db.SaveChanges();
                return RedirectToAction("Edit", new { id = goalView.GoalObj.ID });
            }

            return View(goalView);
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

                Session["goal"] = id;
                return View(new GoalView(goal));
            }
            return RedirectToAction("NotLoggedIn", "Home");
        }

        //
        // POST: /Goal/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(GoalView goalView)
        {
            if (ModelState.IsValid)
            {
                DateTime oldDate = db.Goals.Find(goalView.GoalObj.ID).DueDate;
                int checkYear = 1900;
                int inYear = goalView.GoalObj.DueDate.Year;
                if (inYear < checkYear)
                {
                    // reset the date if an invalid one is entered, then return the view with a validation error.
                    goalView.GoalObj.DueDate = oldDate;
                    ViewBag.DateValidation = "Please enter a date after " + checkYear + ".";
                    return View(goalView);
                }

                List<Category> cats = db.Categories.ToList();           // get the category list from the db only once.
                goalView.GoalObj.Categories = new List<Category>();   // initialize the Goal's Category List, since it starts null.
                foreach (string k in goalView.CategoriesList.Keys)
                {
                    // if the user selected this key name
                    if (goalView.CategoriesList[k])
                    {
                        Category cat = cats.First(c => c.Name == k);
                        // add the category matching the key name
                        goalView.GoalObj.Categories.Add(cat);
                        cat.Goals.Add(goalView.GoalObj);
                        db.Entry(cat).State = EntityState.Modified;
                    }
                }
                
                db.Entry(goalView.GoalObj).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(goalView);
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