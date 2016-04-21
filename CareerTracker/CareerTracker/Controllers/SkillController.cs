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
                    foreach (Skill s in db.Skills.ToList()) {
                        if (s.User.Id.Equals(manager.getIdFromUsername(User.Identity.Name))) {
                            returnList.Add(s);
                        }
                    }
                }
                catch (NullReferenceException e) { }

                return View(returnList);
            }
            return RedirectToAction("NotLoggedIn", "Home");
        }
		// Post: /Skill/?category=
		[Authorize]
		[HttpPost]
		public ActionResult Index(string category) {
			Category cat = db.Categories.FirstOrDefault(c => c.Name == category);
			if (cat == null) {return Index();}	// run the default index if the given category doesn't exist.

			if (User.Identity.IsAuthenticated) {
				UserManager manager = new UserManager();
				List<Skill> returnList = new List<Skill>();
				try {
					if (true) {
						foreach (Skill s in db.Skills.ToList()) {
							if (s.User.Id.Equals(manager.getIdFromUsername(User.Identity.Name)) && hasCat(s.Categories, cat)) {
								returnList.Add(s);
							}
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
                // null Skill object, but populated category list
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
                List<Category> cats = db.Categories.ToList();           // get the category list from the db only once.
                //skill.Categories = new List<Category>();   // initialize the Skill's Category List, since it starts null.
                //foreach (string k in skill.CategoriesList.Keys)
                //{
                //    // if the user selected this key name
                //    if (skill.CategoriesList[k]) {
                //        Category cat = cats.First(c => c.Name == k);
                //        // add the category matching the key name
                //        skill.Categories.Add(cat);
                //        cat.Skills.Add(skill);
                //        db.Entry(cat).State = EntityState.Modified;
                //    }
                //}

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
                // get the category list from the db only once.
                List<Category> cats = db.Categories.ToList();
                //// initialize the Skill's Category List. We're adding any that are still selected anyway, and don't want ones that were deselected to stick around.
                //skill.Categories = new List<Category>();
                //foreach (string k in skill.CategoriesList.Keys)
                //{
                //    // if the user selected this key name
                //    if (skill.CategoriesList[k])
                //    {
                //        Category cat = cats.First(c => c.Name == k);
                //        // add the category matching the key name
                //        skill.Categories.Add(cat);
                //        // also add the skill to the category. gotta make sure the foreign keys are set properly.
                //        cat.Skills.Add(skill);
                //        db.Entry(cat).State = EntityState.Modified;
                //    }
                //}
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

		private bool hasCat(ICollection<Category> cats, Category cat) {
			bool flag = false;
			foreach (Category c in cats) {
				if (c.Name.Equals(cat.Name)) {
					flag = true;
					break;
				}
			}
			return flag;
		}
    }

}