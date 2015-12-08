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
    public class SkillController : Controller
    {
        private CTContext db = new CTContext();

        //
        // GET: /Skill/

        public ActionResult Index()
        {
            List<Skill> returnList = new List<Skill>();
            try
            {
                foreach (Skill s in db.Skills.ToList())
                {
                    if (s.User.UserId == db.UserProfiles.FirstOrDefault(u => u.UserName == User.Identity.Name).UserId)
                    {
                        returnList.Add(s);
                    }
                }
            }
            catch (NullReferenceException e) { }
            return View(returnList);
        }

        //
        // GET: /Skill/Details/5

        public ActionResult Details(int id = 0)
        {
            Skill skill = db.Skills.Find(id);
            if (skill == null)
            {
                return HttpNotFound();
            }
            return View(skill);
        }

        //
        // GET: /Skill/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Skill/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Skill skill)
        {
            int currID = int.Parse(Session["CurrentID"].ToString());
            if (ModelState.IsValid)
            {
                skill.User = db.UserProfiles.FirstOrDefault(u => u.UserName == User.Identity.Name);
                db.Skills.Add(skill);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(skill);
        }

        //
        // GET: /Skill/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Skill skill = db.Skills.Find(id);
            if (skill == null)
            {
                return HttpNotFound();
            }
            return View(skill);
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

        public ActionResult Delete(int id = 0)
        {
            Skill skill = db.Skills.Find(id);
            if (skill == null)
            {
                return HttpNotFound();
            }
            return View(skill);
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