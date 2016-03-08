using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CareerTracker.Models;
using CareerTracker.DAL;

namespace CareerTracker.DataRepository
{
    public static class ArtifactRepo
    {
        //private static CTContext db = new CTContext();

        public static List<Artifact> getUserArtifacts(string username)
        {
            CTContext db = new CTContext();
            return db.Artifacts.Where(a => a.User.UserName == username).ToList();
        }

        public static Artifact getArtifact(int id, string username)
        {
            CTContext db = new CTContext();
            Artifact art = db.Artifacts.FirstOrDefault(a => a.ID == id);
            if (art.User.UserName != username)
            {
                art = null;
            }
            return art;
        }
        public static void createArtifact(Artifact artifact, string username)
        {
            CTContext db = new CTContext();
            artifact.User = db.Users.FirstOrDefault(u => u.UserName == username);
            db.Artifacts.Add(artifact);
            db.SaveChanges();
        }
        public static void deleteArtifact(int id, string username)
        {
            CTContext db = new CTContext();
            Artifact artifact = db.Artifacts.Find(id);
            if (artifact.User.UserName.Equals(username))
            {
                FileDeletion(artifact.Location, username);
                db.Artifacts.Remove(artifact);
                db.SaveChanges();
            }
        }

        public static void FileDeletion(string fileName, string userName)
        {
            var Request = HttpContext.Current.Request;
            string fullPath = Request.MapPath("~/Artifacts/" + userName + "/" + fileName);
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
        }

        /**
         * Checks whether the given user, identified by the usr string parameter,
         * has the requested type of access to the given artifact.
         * 
         * Parameters:
         *  art         : The artifact in question
         *  usr         : The username of the user requesting access
         *  accessType  : whether write/delete access is being requested.
         **/
        public static bool checkArtifactPermissions(Artifact art, string usr, bool accessType)
        {
            bool returnValue = false;

            return returnValue;
        }
    }
}