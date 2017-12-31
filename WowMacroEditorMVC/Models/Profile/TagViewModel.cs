using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WowMacroEditorMVC.Models.Profile
{
    public class TagViewModel
    {
        public string DisplayName { get; set; }
        public string GravatarID { get; set; }
        public int UserIntID { get; set; }
        

        public TagViewModel(Guid userID)
        {
            WowMacroEditorEntities db = new WowMacroEditorEntities();
            var completeUser = db.GetCompleteUser(userID);
            Setup(completeUser);
        }

        public void Setup(CompleteUser completeUser)
        {
            UserIntID = completeUser.UserIntID.Value;
            DisplayName = completeUser.DisplayName;
            GravatarID = completeUser.GravatarID;
        }



    }
}