using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WowMacroEditorMVC.Models
{
    public partial class WowMacroEditorEntities
    {
        public CompleteUser GetCompleteUser(int userIntID)
        {
            var completeUser = this.CompleteUsers.FirstOrDefault(p => p.UserIntID == userIntID);
            if(completeUser == null)
                throw new Exception("Unable to find user with user int ID of " + userIntID);
            return completeUser;
        }
        
        public CompleteUser GetCompleteUser(Guid userID)
        {
            var completeUser = this.CompleteUsers.FirstOrDefault(p => p.UserID == userID);
            
            if (completeUser == null)
                throw new Exception("Unable to find user with userID of " + userID.ToString());
            return completeUser;
        }

        public UserProfile GetUserProfile(Guid userID)
        {
            var userProfile = this.UserProfiles.FirstOrDefault(p => p.UserID == userID);
            if (userProfile == null)
            {
                userProfile = new UserProfile() { UserID = userID };
                this.UserProfiles.AddObject(userProfile);
                this.SaveChanges();
            }
            return userProfile;
        }



    }
}