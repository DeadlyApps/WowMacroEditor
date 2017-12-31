using System;
using System.Collections.Generic;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Web;

namespace WowMacroEditorMVC.Models.Profile
{
    public class IndexViewModel
    {
        public string HeaderName { get; set; }
        public string Description { get; set; }
        public string MemberFor { get; set; }
        public string LastLogin { get; set; }
        public string GravatarID { get; set; }
        public string GuildWebsite { get; set; }
        public string Location { get; set; }
        public string Age { get; set; }
        public int MacroCount { get; set; }
        public List<MacroDetails> Macros { get; set; }
        public bool IsOwner { get; set; }
        public string Email { get; set; }

        public class MacroDetails
        {
            public int VoteCount { get; set; }
            public int CommentCount { get; set; }
            public string Title { get; set; }
            public EntityCollection<Tag> Tags { get; set; }
            public DateTime CreatedDate { get; set; }
            public int ViewCount { get; set; }
            public int MacroID { get; set; }
        }


        public IndexViewModel(int userIntID, Guid? currentUserID)
        {
            WowMacroEditorEntities db = new WowMacroEditorEntities();
            var completeUser = db.GetCompleteUser(userIntID);

            HeaderName = completeUser.DisplayName;
            Description = completeUser.Description;
            MemberFor = TimeToText(completeUser.CreateDate);
            LastLogin = TimeToText(completeUser.LastLoginDate);
            GravatarID = completeUser.GravatarID;
            GuildWebsite = completeUser.GuildWebsite;
            Location = completeUser.Location;
            if (completeUser.BirthDate.HasValue)
            {
                Age = TimeToText(completeUser.BirthDate.Value);
            }

            Email = completeUser.Email;

            MacroCount = db.Macroes.Count(p => p.UserID == completeUser.UserID);
            Macros = db.Macroes
                .Where(m => m.UserID == completeUser.UserID)
                .OrderByDescending(m=>m.Views)
                .OrderByDescending(m=>m.Created)
                .Select(m=> new MacroDetails() {
                    CommentCount = m.Comments.Count,
                    CreatedDate = m.Created,
                    Tags = m.Tags,
                    Title = m.Title,
                    ViewCount = m.Views,
                    VoteCount = m.RankMacroes.Count,
                    MacroID = m.MacroID,
                })
                .ToList();

            IsOwner = completeUser.UserID == currentUserID;

        }

        public static string TimeToText(DateTime? nullableOriginalDate)
        {
            DateTime originalDate;
            if (nullableOriginalDate.HasValue)
                originalDate = nullableOriginalDate.Value;
            else
                originalDate = DateTime.Now.AddMinutes(-1);


            TimeSpan difference = DateTime.Now.Subtract(originalDate);
            if (difference < new TimeSpan(0, 1, 0))
            {
                return difference.Seconds + " second" + AddPluralization(difference.Seconds);
            }
            else if (difference < new TimeSpan(1, 0, 0))
            {
                return difference.Minutes + " minute" + AddPluralization(difference.Minutes);
            }
            else if (difference < new TimeSpan(1, 0, 0, 0))
            {
                return difference.Hours + " hour" + AddPluralization(difference.Hours);
            }

            int differenceInMonths = ((DateTime.Now.Year - originalDate.Year) * 12) + DateTime.Now.Month - originalDate.Month;
            if ( differenceInMonths < 1)
                return difference.Days + " day" + AddPluralization(difference.Days);

            string differenceAddition = "";
            if (differenceInMonths > 12)
                differenceAddition += differenceInMonths / 12 + " year" + AddPluralization(differenceInMonths / 12);
            if (differenceInMonths % 12 > 0)
                differenceAddition += (String.IsNullOrEmpty(differenceAddition) ? "" : ", ") + (differenceInMonths % 12) + " month" + AddPluralization(differenceInMonths % 12);
            return differenceAddition;

        }   

        private static string AddPluralization(int number)
        {
            return number > 1 ? "s" : "";
        }

    }
}