using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WowMacroEditorMVC.Models.Macros
{
    public class DisplayViewModel
    {
        public int Rank { get; set; }
        public int MacroID { get; set; }
        public string Title { get; set; }
        public List<Tag> Tags { get; set; }
        public Guid UserID { get; set; }
        public DateTime Created { get; set; }
        public Guid? CurrentUserID { get; set; }

        public bool CanVoteUp { get; set; }
        public bool CanVoteDown { get; set; }


        public DisplayViewModel(int macroID, Guid? currentUserID)
        {
            WowMacroEditorEntities db = new WowMacroEditorEntities();
            var macro = db.Macroes.FirstOrDefault(p => p.MacroID == macroID);
            Rank = macro.Rank;
            MacroID = macro.MacroID;
            Title = macro.Title;
            Tags = macro.Tags.ToList();
            UserID = macro.UserID;
            Created = macro.Created;

            CurrentUserID = currentUserID;

            CanVoteUp = false;
            CanVoteDown = false;

            if (CurrentUserID != null)
            {
                var votesFromUser = db.RankMacroes.Where(p => p.UserID == currentUserID.Value && p.MacroID == macroID);
                if (votesFromUser.Count() == 0)
                {
                    CanVoteUp = true;
                    CanVoteDown = true;
                }
                else
                {
                    CanVoteUp = votesFromUser.Any(v => !v.RankUp);
                    CanVoteDown = votesFromUser.Any(v => v.RankUp);
                }
            }
        }

    }
}