using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CareerTracker.Models;
using CareerTracker.DAL;

namespace CareerTracker.DataRepository
{
    public class ArtifactRepo
    {
        private static CTContext db = new CTContext();

        public static List<Artifact> getUserArtifacts(int userID)
        {
            return db.Artifacts.Where(a => a.User.UserId == userID).ToList();
        }

        public static Artifact getArtifact(int id, string username)
        {
            Artifact art = db.Artifacts.FirstOrDefault(a => a.ID == id);
            UserProfile user = db.UserProfiles.FirstOrDefault(u => u.UserName == username);
            if (art.User.UserId != user.UserId)
            {
                art = null;
            }
            return art;
        }
    }
}