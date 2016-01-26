﻿using System;
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
    public class GoalController : Controller
    {
        private CTContext db = new CTContext();

        //
        // GET: /Goal/

        public ActionResult Index()
        {
            List<Goal> returnList = new List<Goal>();
            try
            {
                foreach (Goal g in db.Goals.ToList()) 
                {
                    if (g.User.UserId == db.UserProfiles.FirstOrDefault(u => u.UserName == User.Identity.Name).UserId)
                    {
                        returnList.Add(g);
                    }
                }
            }
            catch (NullReferenceException e) { }
            return View(returnList);
        }

        //
        // GET: /Goal/Details/5

        public ActionResult Details(int id = 0)
        {
            Goal goal = db.Goals.Find(id);
            if (goal == null)
            {
                return HttpNotFound();
            }
            return View(goal);
        }

        //
        // GET: /Goal/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Goal/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Goal goal)
        {
            if (ModelState.IsValid)
            {
                goal.User = db.UserProfiles.FirstOrDefault(u => u.UserName == User.Identity.Name);
                db.Goals.Add(goal);
                db.SaveChanges();
                return RedirectToAction("Edit", new { id = goal.ID });
            }

            return View(goal);
        }

        //
        // GET: /Goal/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Goal goal = db.Goals.Find(id);
            if (goal == null)
            {
                return HttpNotFound();
            }
            Session["goal"] = id;
            return View(goal);
        }

        //
        // POST: /Goal/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Goal goal)
        {
            if (ModelState.IsValid)
            {
                db.Entry(goal).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(goal);
        }

        //
        // GET: /Goal/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Goal goal = db.Goals.Find(id);
            if (goal == null)
            {
                return HttpNotFound();
            }
            return View(goal);
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