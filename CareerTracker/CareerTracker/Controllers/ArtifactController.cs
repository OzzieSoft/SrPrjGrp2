using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CareerTracker.Models;
using CareerTracker.DAL;
using CareerTracker.DataRepository;
using System.IO;

namespace CareerTracker.Controllers
{
    public class ArtifactController : Controller
    {
        private CTContext db = new CTContext();

        //
        // GET: /Artifact/

        public ActionResult Index()
        {
            if(User.Identity.IsAuthenticated)
            {
                List<Artifact> returnList = ArtifactRepo.getUserArtifacts(User.Identity.Name.ToString());
                return View(returnList);
            }
            return RedirectToAction("PleaseLogIn", "Home");
        }

        //
        // GET: /Artifact/Details/5

        public ActionResult Details(int id = 0)
        {
            if (User.Identity.IsAuthenticated)
            {
                Artifact artifact = db.Artifacts.FirstOrDefault(a => a.ID == id);
                string username = User.Identity.Name.ToString();
                if (artifact.User.UserName != username)
                {
                    artifact = null;
                }
                if (artifact == null)
                {
                    return HttpNotFound();
                }
                return View(artifact);
            }
            return RedirectToAction("PleaseLogIn", "Home");
        }

        //
        // GET: /Artifact/Create

        public ActionResult Create()
        {
            if(User.Identity.IsAuthenticated)
            {
                return View();
            }
            return RedirectToAction("NotLoggedIn", "Home");
        }

        //
        // POST: /Artifact/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Artifact artifact, HttpPostedFileBase file)
        {
             if (ModelState.IsValid)
            {
                 if (file != null && file.ContentLength > 0) {
                    try
                    {
                        string usr = User.Identity.Name.ToString();
                        string test = Path.GetFileNameWithoutExtension(file.FileName);
                        var fileName = test + usr + System.IO.Path.GetExtension(file.FileName);
                        string path = Path.Combine(Server.MapPath("/Artifacts"), fileName);
                        file.SaveAs(path);
                        artifact.Location = fileName;
                        ArtifactRepo.createArtifact(artifact, User.Identity.Name.ToString());
                        return RedirectToAction("Index");
                        //ViewBag.Message = "File uploaded successfully";
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Message = "ERROR:" + ex.Message.ToString();
                    }
                }
            }
            return View(artifact);
        }

        //
        // GET: /Artifact/Edit/5

        public ActionResult Edit(int id = 0)
        {
            if (User.Identity.IsAuthenticated)
            {
                //Artifact artifact = ArtifactRepo.getArtifact(id,User.Identity.Name.ToString());
                Artifact artifact = db.Artifacts.FirstOrDefault(a => a.ID == id);
                string username = User.Identity.Name.ToString();
                if (artifact.User.UserName != username)
                {
                    artifact = null;
                }
                if (artifact == null)
                {
                    return HttpNotFound();
                }
                return View(artifact);
            }
            return RedirectToAction("NotLoggedIn", "Home");
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
            if (User.Identity.IsAuthenticated)
            {
                Artifact artifact = db.Artifacts.FirstOrDefault(a => a.ID == id);
                string username = User.Identity.Name.ToString();
                if (artifact.User.UserName != username)
                {
                    artifact = null;
                }; //ArtifactRepo.getArtifact(id, User.Identity.Name.ToString());
                if (artifact == null)
                {
                    return HttpNotFound();
                }
                return View(artifact);
            }
            return RedirectToAction("NotLoggedIn", "Home");
        }

        //
        // POST: /Artifact/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ArtifactRepo.deleteArtifact(id, User.Identity.Name.ToString());
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}