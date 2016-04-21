using CareerTracker.DAL;
using CareerTracker.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
/*
 * This file contains view models for Goals, Skills, and Artifacts.
 * They are identical, so the comments on the GoalView apply to the other two as well.
 */
namespace CareerTracker.Models {
	public class GoalView {
		// used for the GET methods/views. Only pupolates the list of categories so they can be selected by the user.
		public GoalView() {
			CategoriesList = new Dictionary<string, bool>();
			foreach (Category cat in new CTContext().Categories.ToList()) {
				CategoriesList[cat.Name] = false;
			}
		}
		public GoalView(Goal g) {
			this.GoalObj = g;
			CategoriesList = new Dictionary<string, bool>();
			// we have an existing goal. get the categories already assigned to it.
			List<Category> currCats = g.Categories.ToList();
			foreach (Category cat in new CTContext().Categories.ToList()) {
				CategoriesList[cat.Name] = (currCats.Contains(cat));
			}
		}
		// The Goal object being worked on.
		public Goal GoalObj { get; set; }
		// The list of categories and whether the user has checked them. Dynamically built on each request
		// based on the categories defined by the admin.
		public Dictionary<string, bool> CategoriesList { get; set; }
	}

	public class SkillView {
		public SkillView() {
			CategoriesList = new Dictionary<string, bool>();
			foreach (Category cat in new CTContext().Categories.ToList()) {
				CategoriesList.Add(cat.Name, false);
			}
		}
		public SkillView(Skill g) {
			this.SkillObj = g;
			CategoriesList = new Dictionary<string, bool>();
			List<Category> currCats = g.Categories.ToList();
			foreach (Category cat in new CTContext().Categories.ToList()) {
				CategoriesList[cat.Name] = (currCats.Contains(cat));
			}
		}
		public Skill SkillObj { get; set; }
		public Dictionary<string, bool> CategoriesList { get; set; }
	}

	public class ArtifactView {
		public ArtifactView() {
			CategoriesList = new Dictionary<string, bool>();
			foreach (Category cat in new CTContext().Categories.ToList()) {
				CategoriesList[cat.Name] = false;
			}
		}
		public ArtifactView(Artifact g) {
			CategoriesList = new Dictionary<string, bool>();
			this.ArtifactObj = g;
			List<Category> currCats = g.Categories.ToList();
			foreach (Category cat in new CTContext().Categories.ToList()) {
				CategoriesList[cat.Name] = (currCats.Contains(cat));
			}
		}
		public Artifact ArtifactObj { get; set; }
		public Dictionary<string, bool> CategoriesList { get; set; }
	}

	public class UserView {

		public User Users { get; set; }
		public string userRole { get; set; }

		public UserView(User user) {
			UserManager manager = new UserManager();
			Users = user;
			if (manager.hasClaim(user.UserName, "teacher")) {
				userRole = "Teacher";
			}
			else if (manager.hasClaim(user.UserName, "admin")) {
				userRole = "Admin";
			}
			else {
				userRole = "Student";
			}

		}
	}
}