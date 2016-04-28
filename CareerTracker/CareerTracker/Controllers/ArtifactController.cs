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

/*
 * Artifact controller, comments will be found on the most complicated methods, most are simple getters and setters.
 */
namespace CareerTracker.Controllers
{
    public class ArtifactController : Controller
    {
        private CTContext db = new CTContext();

        //
        // GET: /Artifact/
        /// <summary>
        /// Gets a list of the current artifacts
        /// </summary>
        /// <returns></returns>
		[Authorize]
        public ActionResult Index()
        {

			//ViewBag.Cats = db.Categories.ToList();
            List<Artifact> returnList = ArtifactRepo.getUserArtifacts(User.Identity.Name.ToString());
            return View(returnList);
        }

        //
        // GET: /Artifact/Details/5
        /// <summary>
        /// get method of the artifact details page
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
        /// <summary>
        /// This is the first method you should come to for failure in the artifact controller.
        /// This controls uploading and saving artifacts and is one of the more complicated methods, as such there will be comments explaining everything within the method
        /// </summary>
        /// <param name="artifact">The artifact to be added</param>
        /// <param name="file">The file to be uploaded</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Artifact artifact, HttpPostedFileBase file)
        {
            //If the model state is valid
            if (ModelState.IsValid)
            {
                //If the file is not null and the files is not empty
                if (file != null && file.ContentLength > 0)
                {
                    try
                    {
                        //Creates a directory if one does not exist, otherwise saves the name of the directory for later use
                        var dirPath = Server.MapPath("/Artifacts/" + User.Identity.Name + "/");
                        Directory.CreateDirectory(dirPath);
                        //Combines the filename and the directory path
                        string path = Path.Combine(dirPath, file.FileName);
                        //If the user attempts to upload a exe or bat file, returns them to the index with a javascript alert
                        if ((System.IO.Path.GetExtension(file.FileName)).ToString().Equals(".exe") || (System.IO.Path.GetExtension(file.FileName)).ToString().Equals(".bat"))
                        {
                            Response.Write(@"<script language='javascript'>alert('Please do not upload .exe or .bat files.');</script>");
                            throw new InvalidDataException("bat and exe files can't be uploaded.");
                        }
                        //If the file exists, warn the user that the file has already been uploaded.
                        if(System.IO.File.Exists(path))
                        {
                            Response.Write(@"<script language='javascript'>alert('A file with that name has already been uploaded by you. Please update your artifact through the edit link, if you wish to update your file.');</script>");
                            throw new Exception("File already uploaded");
                        }
                        //Saves the file to the server, and sets its location for the database
                        file.SaveAs(path);
                        artifact.Location = file.FileName;
                        //calls the repo to create the database. If it got this far, the complicated part is done.
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
            //If the model state becomes invalid, returns the model state to a debug line
            System.Diagnostics.Debug.WriteLine(ModelState.IsValid);
            return View(artifact);
        }


        public FileResult Download(string fileName)
        {
            //Returns the file for download
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
        /// <summary>
        /// Very similar method to the uploading an artifact code, if any issues, check the comments there
        /// </summary>
        /// <param name="artifact"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Artifact artifact, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                //Deletes the artifact on the server first, before going into uploading
                FileDeletion(Session["location"].ToString());
                Session["location"] = null;
                if (file != null && file.ContentLength > 0)
                {
                    try
                    {
                        var dirPath = Server.MapPath("/Artifacts/" + User.Identity.Name + "/");
                        Directory.CreateDirectory(dirPath);                        
                        string path = Path.Combine(dirPath, file.FileName);
                        if ((System.IO.Path.GetExtension(file.FileName)).ToString().Equals(".exe") || (System.IO.Path.GetExtension(file.FileName)).ToString().Equals(".bat"))
                        {
                            Response.Write(@"<script language='javascript'>alert('Please do not upload .exe or .bat files.');</script>");
                            throw new InvalidDataException("bat and exe files can't be uploaded.");
                        }
                        //IT SHOULD NEVER HIT THIS, but if it somehow does, it is included.
                        if (System.IO.File.Exists(path))
                        {
                            Response.Write(@"<script language='javascript'>alert('A file with that name has already been uploaded by you. Please update your artifact through the edit link, if you wish to update your file.');</script>");
                            throw new Exception("File already uploaded");
                        }
                        file.SaveAs(path);
                        artifact.Location = file.FileName;
                        return RedirectToAction("Index");
                        //ViewBag.Message = "File uploaded successfully";
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Message = "ERROR:" + ex.Message.ToString();
                    }
                }

                db.Entry(artifact).State = EntityState.Modified;
                db.SaveChanges();
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
        //See artifact repo for any issues
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

        //Deletes the file if it exists.
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