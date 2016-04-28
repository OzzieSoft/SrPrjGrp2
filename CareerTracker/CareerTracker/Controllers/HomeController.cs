using CareerTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CareerTracker.Controllers
{
    //Literally nothing
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = ""; 

            return View(new AdminTeacherCheck(User.Identity.Name));
        }

        public ActionResult About()
        {
            ViewBag.Message = "";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "";

            return View();
        }

        public ActionResult PleaseLogIn() 
        {
            ViewBag.Message = "";
            return View();
        }

		public ActionResult NotActive() 
		{
			ViewBag.Message = "";
			return View();
		}
    }
}
