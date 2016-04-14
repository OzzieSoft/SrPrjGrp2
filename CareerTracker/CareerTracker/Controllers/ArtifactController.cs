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
		[Authorize]
        public ActionResult Index()
        {

			//ViewBag.Cats = db.Categories.ToList();
            List<Artifact> returnList = ArtifactRepo.getUserArtifacts(User.Identity.Name.ToString());
            return View(returnList);
        }

        //
        // GET: /Artifact/Details/5
		[Authorize]
        public ActionResult Details(int id = 0)
        {
            Artifact artifact = ArtifactRepo.getArtifact(id, User.Identity.Name.ToString());
            if (artifact == null)
            {
                return HttpNotFound();
            }
            return View(artifact);
        }

        //
        // GET: /Artifact/Create
		[Authorize]
        public ActionResult Create()
        {

			//ViewBag.Cats = db.Categories.ToList();
            return View();
        }

        //
        // POST: /Artifact/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Artifact artifact, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null && file.ContentLength > 0)
                {
                    try
                    {
                        //string usr = User.Identity.Name.ToString();
                        //string test = Path.GetFileNameWithoutExtension(file.FileName);
                        var dirPath = Server.MapPath("/Artifacts/" + User.Identity.Name + "/");
                        Directory.CreateDirectory(dirPath);
                        //var fileName = test + "-" + usr + System.IO.Path.GetExtension(file.FileName);
                        string path = Path.Combine(dirPath, file.FileName);
                        if ((System.IO.Path.GetExtension(file.FileName)).ToString().Equals(".exe") || (System.IO.Path.GetExtension(file.FileName)).ToString().Equals(".bat"))
                        {
                            Response.Write(@"<script language='javascript'>alert('Please do not upload .exe or .bat files.');</script>");
                            throw new InvalidDataException("bat and exe files can't be uploaded.");
                        }
                        if(System.IO.File.Exists(path))
                        {
                            Response.Write(@"<script language='javascript'>alert('A file with that name has already been uploaded by you. Please update your artifact through the edit link, if you wish to update your file.');</script>");
                            throw new Exception("File already uploaded");
                        }
                        file.SaveAs(path);
                        artifact.Location = file.FileName;
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


        public FileResult Download(string fileName)
        {
            var FileVirtualPath = "/Artifacts/" + User.Identity.Name +"/" + fileName;
            return File(FileVirtualPath, "application/force-download", Path.GetFileName(FileVirtualPath));
        }  

        //
        // GET: /Artifact/Edit/5
		[Authorize]
        public ActionResult Edit(int id = 0)
        {
            //Artifact artifact = ArtifactRepo.getArtifact(id,User.Identity.Name.ToString());
            Artifact artifact = db.Artifacts.FirstOrDefault(a => a.ID == id);
            Session["location"] = artifact.Location;
            string username = User.Identity.Name.ToString();
            if (artifact.User.UserName.ToLower() != username.ToLower())
            {
                artifact = null;
            }
            if (artifact == null)
            {
                return HttpNotFound();
            }

			//ViewBag.Cats = db.Categories.ToList();
            return View(artifact);
        }

        //
        // POST: /Artifact/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Artifact artifact, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                FileDeletion(Session["location"].ToString());
                Session["location"] = null;
                //artifact.Location = file.FileName;
                db.Entry(artifact).State = EntityState.Modified;
                db.SaveChanges();
                if (file != null && file.ContentLength > 0)
                {
                    try
                    {
                        //string usr = User.Identity.Name.ToString();
                        //string test = Path.GetFileNameWithoutExtension(file.FileName);
                        var dirPath = Server.MapPath("/Artifacts/" + User.Identity.Name + "/");
                        Directory.CreateDirectory(dirPath);
                        //var fileName = test + "-" + usr + System.IO.Path.GetExtension(file.FileName);
                        string path = Path.Combine(dirPath, file.FileName);
                        if ((System.IO.Path.GetExtension(file.FileName)).ToString().Equals(".exe") || (System.IO.Path.GetExtension(file.FileName)).ToString().Equals(".bat"))
                        {
                            Response.Write(@"<script language='javascript'>alert('Please do not upload .exe or .bat files.');</script>");
                            throw new InvalidDataException("bat and exe files can't be uploaded.");
                        }
                        file.SaveAs(path);
                        artifact.Location = file.FileName;
                        db.Entry(artifact).State = EntityState.Modified;
                        db.SaveChanges();
                        //ArtifactRepo.createArtifact(artifact, User.Identity.Name.ToString());
                        return RedirectToAction("Index");
                        //ViewBag.Message = "File uploaded successfully";
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Message = "ERROR:" + ex.Message.ToString();
                    }
                }
                return RedirectToAction("Index");
            }
            return View(artifact);
        }

        //
        // GET: /Artifact/Delete/5
		[Authorize]
        public ActionResult Delete(int id = 0)
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

        public void FileDeletion(string fileName)
        {
            string fullPath = Request.MapPath("~/Artifacts/" + User.Identity.Name + "/" + fileName);
            if(System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
        }
    }
}