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
    public class SkillController : Controller
    {
        private CTContext db = new CTContext();

        //
        // GET: /Skill/
		[Authorize]
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                UserManager manager = new UserManager();
                List<Skill> returnList = new List<Skill>();
                try
                {
                    foreach (Skill s in db.Skills.ToList())
                    {
                        if (s.User.Id.Equals(manager.getIdFromUsername(User.Identity.Name)))
                        {
                            returnList.Add(s);
                        }
                    }
                }
                catch (NullReferenceException e) { }
                return View(returnList);
            }
            return RedirectToAction("NotLoggedIn", "Home");
        }

        //
        // GET: /Skill/Details/5
		[Authorize]
        public ActionResult Details(int id = 0)
        {
            if (User.Identity.IsAuthenticated)
            {
                Skill skill = db.Skills.Find(id);
                if (skill == null)
                {
                    return HttpNotFound();
                }
                return View(skill);
            }
            return RedirectToAction("NotLoggedIn", "Home");
        }

        //
        // GET: /Skill/Create
		[Authorize]
        public ActionResult Create()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View();
            }
            return RedirectToAction("NotLoggedIn", "Home");
        }

        //
        // POST: /Skill/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Skill skill)
        {
            if (ModelState.IsValid)
            {
                UserManager manager = new UserManager(db);
                skill.User = manager.findByUserName(User.Identity.Name);
                db.Skills.Add(skill);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(skill);
        }

        //
        // GET: /Skill/Edit/5
		[Authorize]
        public ActionResult Edit(int id = 0)
        {
            if (User.Identity.IsAuthenticated)
            {
                Skill skill = db.Skills.Find(id);
                if (skill == null)
                {
                    return HttpNotFound();
                }
                return View(skill);
            }
            return RedirectToAction("NotLoggedIn", "Home");
        }

        //
        // POST: /Skill/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Skill skill)
        {
            if (ModelState.IsValid)
            {
                db.Entry(skill).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(skill);
        }

        //
        // GET: /Skill/Delete/5
		[Authorize]
        public ActionResult Delete(int id = 0)
        {
            if (User.Identity.IsAuthenticated)
            {
                Skill skill = db.Skills.Find(id);
                if (skill == null)
                {
                    return HttpNotFound();
                }
                return View(skill);
            }
            return RedirectToAction("NotLoggedIn", "Home");
        }

        //
        // POST: /Skill/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Skill skill = db.Skills.Find(id);
            db.Skills.Remove(skill);
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