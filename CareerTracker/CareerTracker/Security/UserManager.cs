using System;
using System.Linq;
using Microsoft.AspNet.Identity;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNet.Identity.EntityFramework;
using CareerTracker.DAL;
using CareerTracker.Models;
using System.Security.Claims;

namespace CareerTracker.Security {
	/*
	 * GETTING AND SETTING CLAIMS
	 * 
	 * await manager.AddClaimAsync(manager.getIdFromUsername(model.UserName), new Claim(ClaimTypes.Role, "admin"));
	 *      need the await keyword or it won't work.
	 *      
	 * 
	 * manager.GetClaims(manager.getIdFromUsername(model.UserName)).FirstOrDefault();
	 */

	public class UserManager : UserManager<User> {
		public UserManager() : this(new UserStore<User>(new CTContext())) { }

		public UserManager(CTContext context) : this(new UserStore<User>(context)) { }

		public UserManager(UserStore<User> store)
			: base(store) {
			this.PasswordHasher = new SQLPasswordHasher();
			store.AutoSaveChanges = true;
		}

		public User findByUserName(string name) {
			User returnval;
			returnval = Users.FirstOrDefault(u => u.UserName == name);
			return returnval;
		}
		public User findById(string id) {
			User returnval;
			returnval = Users.FirstOrDefault(u => u.Id == id);

			return returnval;
		}
		public string getIdFromUsername(string name) {
			string returnval;
			try {
				returnval = Users.FirstOrDefault(u => u.UserName == name).Id;
			}
			catch (NullReferenceException nre) {
				returnval = null;
			}
			return returnval;
		}

		public bool hasClaim(string name, string claim) {
			return hasClaim(name, ClaimTypes.Role, claim);
		}

		//Checks to see if the user, identified by name, has a claim of a certain claim type, will return true of false
		public bool hasClaim(string name, string claimType, string claimVal) {
			return hasClaim(name, claimType, claimVal, true);
		}
        /// <summary>
        /// Checks for claims based on id
        /// </summary>
        /// <param name="nameOrId">Enter a name or id here</param>
        /// <param name="claimType">Where the claims are stored(See already done implementation in the admin controller for an example)</param>
        /// <param name="claimVal">What claim you're looking for</param>
        /// <param name="name">True for name, false for id</param>
        /// <returns></returns>
		public bool hasClaim(string nameOrId, string claimType, string claimVal, bool name) {
			bool retval;
			Claim toSearch = new Claim(claimType, claimVal);
			string id = name ? getIdFromUsername(nameOrId) : nameOrId;
			retval = this.GetClaims(id).Where(c => (c.Type == claimType) && (c.Value == claimVal)).Count() > 0;
			return retval;
		}
	}

    /// <summary>
    /// Below is pregenerated code for password hashing
    /// </summary>
	public class SQLPasswordHasher : PasswordHasher {
		public override string HashPassword(string password) {
			return base.HashPassword(password);
		}

		public override PasswordVerificationResult VerifyHashedPassword(string hashedPassword, string providedPassword) {
			string[] passwordProperties = hashedPassword.Split('|');
			if (passwordProperties.Length != 3) {
				return base.VerifyHashedPassword(hashedPassword, providedPassword);
			}
			else {
				string passwordHash = passwordProperties[0];
				int passwordformat = 1;
				string salt = passwordProperties[2];
				if (String.Equals(EncryptPassword(providedPassword, passwordformat, salt), passwordHash, StringComparison.CurrentCultureIgnoreCase)) {
					return PasswordVerificationResult.SuccessRehashNeeded;
				}
				else {
					return PasswordVerificationResult.Failed;
				}
			}
		}

		//This is copied from the existing SQL providers and is provided only for back-compat.
		private string EncryptPassword(string pass, int passwordFormat, string salt) {
			if (passwordFormat == 0) // MembershipPasswordFormat.Clear
				return pass;

			byte[] bIn = Encoding.Unicode.GetBytes(pass);
			byte[] bSalt = Convert.FromBase64String(salt);
			byte[] bRet = null;

			if (passwordFormat == 1) { // MembershipPasswordFormat.Hashed 
				HashAlgorithm hm = HashAlgorithm.Create("SHA1");
				if (hm is KeyedHashAlgorithm) {
					KeyedHashAlgorithm kha = (KeyedHashAlgorithm)hm;
					if (kha.Key.Length == bSalt.Length) {
						kha.Key = bSalt;
					}
					else if (kha.Key.Length < bSalt.Length) {
						byte[] bKey = new byte[kha.Key.Length];
						Buffer.BlockCopy(bSalt, 0, bKey, 0, bKey.Length);
						kha.Key = bKey;
					}
					else {
						byte[] bKey = new byte[kha.Key.Length];
						for (int iter = 0; iter < bKey.Length; ) {
							int len = Math.Min(bSalt.Length, bKey.Length - iter);
							Buffer.BlockCopy(bSalt, 0, bKey, iter, len);
							iter += len;
						}
						kha.Key = bKey;
					}
					bRet = kha.ComputeHash(bIn);
				}
				else {
					byte[] bAll = new byte[bSalt.Length + bIn.Length];
					Buffer.BlockCopy(bSalt, 0, bAll, 0, bSalt.Length);
					Buffer.BlockCopy(bIn, 0, bAll, bSalt.Length, bIn.Length);
					bRet = hm.ComputeHash(bAll);
				}
			}

			return Convert.ToBase64String(bRet);
		}
	}
}