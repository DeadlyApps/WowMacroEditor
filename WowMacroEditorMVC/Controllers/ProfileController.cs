using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WowMacroEditorMVC.Models.Profile;
using WowMacroEditorMVC.Models;

namespace WowMacroEditorMVC.Controllers
{
    public class ProfileController : Controller
    {
        public ActionResult Index(int userIntID, string profileName)
        {
            WowMacroEditorEntities db = new WowMacroEditorEntities();
            var completeUser = db.GetCompleteUser(userIntID);
            if (string.Compare(completeUser.DisplayName.ToFriendlyUrl(), profileName, true) != 0)
            {
                return RedirectPermanent(Url.Content("~/User/" + userIntID + "/" + completeUser.DisplayName.ToFriendlyUrl()));
            }

            IndexViewModel model = new IndexViewModel(userIntID, AuthUtil.GetUserID());

            return View(model);
        }
        
        public ActionResult Tag(Guid userID)
        {
            TagViewModel model = new TagViewModel(userID);
            return PartialView(model);
        }

        public string UpdateDisplayName(string value)
        {
            WowMacroEditorEntities db = new WowMacroEditorEntities();
            var userProfile = db.GetUserProfile(AuthUtil.GetUserID().Value);
            userProfile.DisplayName = value;
            db.SaveChanges();
            return value;
            
        }

        public string UpdateEmail(string value)
        {
            WowMacroEditorEntities db = new WowMacroEditorEntities();
            var userProfile = db.GetUserProfile(AuthUtil.GetUserID().Value);
            userProfile.Email = value;
            db.SaveChanges();
            return value;
        }

        public string UpdateGuildWebsite(string value)
        {
            WowMacroEditorEntities db = new WowMacroEditorEntities();
            var userProfile = db.GetUserProfile(AuthUtil.GetUserID().Value);
            userProfile.GuildWebsite = value;
            db.SaveChanges();
            return value;
        }

        public string UpdateBirthDate(string value)
        {
            string returnValue = value;
            WowMacroEditorEntities db = new WowMacroEditorEntities();
            var userProfile = db.GetUserProfile(AuthUtil.GetUserID().Value);
            if (String.IsNullOrWhiteSpace(value))
                userProfile.BirthDate = null;
            else
            {
                DateTime birthDay;
                if (!DateTime.TryParse(value, out birthDay))
                {
                    return "Enter your birthday as MM/DD/YYYY";
                }

                userProfile.BirthDate = DateTime.Parse(value);
                returnValue = IndexViewModel.TimeToText(userProfile.BirthDate);

            }
            db.SaveChanges();
            return returnValue;
        }

        

        public string UpdateDescription(string value)
        {
            WowMacroEditorEntities db = new WowMacroEditorEntities();
            var userProfile = db.GetUserProfile(AuthUtil.GetUserID().Value);
            userProfile.Description = value;
            db.SaveChanges();
            return value;
        }

        public string UpdateLocation(string value)
        {
            WowMacroEditorEntities db = new WowMacroEditorEntities();
            var userProfile = db.GetUserProfile(AuthUtil.GetUserID().Value);
            userProfile.Location = value;
            db.SaveChanges();
            return value;
        }

    }
}
