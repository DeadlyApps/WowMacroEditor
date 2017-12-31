using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WowMacroEditorMVC.Models;
using System.Web.Security;
using WowMacroEditorMVC.Models.Macros;
using WowMacroEditorMVC.Infrastructure;

namespace WowMacroEditorMVC.Controllers
{

    public class MacrosController : Controller
    {
        private WowMacroEditorEntities db;
        private const int PAGE_LENGTH = 15;

        public MacrosController()
        {
            db = new WowMacroEditorEntities();
        }

        [RequireHttp]
        [Authorize]
        public ViewResult Index()
        {
            Guid UserID = AuthUtil.GetUserID().Value;
            List<Macro> macros = db.Macroes.Where(p => p.UserID == UserID).OrderByDescending(p => p.Created).ToList();
            foreach (var macro in macros)
                macro.MacroText = FormatMacroTextForDisplay(macro.MacroText);
            return View(macros);
        }
        [RequireHttp]
        public ViewResult Search()
        {
            List<Macro> macros = new List<Macro>();
            return View(macros);
        }
        [RequireHttp]
        public PartialViewResult SearchQuery(string q)
        {
            var macros = db.Macroes.Where(r => r.Title.Contains(q) ||
                                                String.IsNullOrEmpty(q)).Take(10);
            return PartialView("_MacroSearchResults", macros);
        }
        [RequireHttp]
        public ActionResult TitleQuickSearch(string term)
        {
            var titles = db.Macroes
                .Where(r => r.Title.Contains(term))
                .Take(10)
                .Select(r => new { label = r.Title.Trim() });
            return Json(titles, JsonRequestBehavior.AllowGet);
        }
        [RequireHttp]
        public ActionResult TagQuickSearch(string term)
        {
            var tags = db.Tags
                .Where(r => r.Tag1.StartsWith(term) || String.IsNullOrEmpty(term))
                .Take(10)
                .Select(r => new { label = r.Tag1.Trim() });
            return Json(tags, JsonRequestBehavior.AllowGet);
        }

        [RequireHttp]
        public ViewResult Browse(String Sort, String Tag, int Page = 0)
        {
            var query = (from x in db.Macroes
                         where x.Tags.Count(p => String.Compare(p.Tag1, Tag, true) == 0) > 0 || Tag == null
                         orderby x.Created descending
                         select x).OrderByDescending(p => p.Rank);


            if (!String.IsNullOrEmpty(Sort))
            {

                if (String.Compare(Sort, "Newest", true) == 0)
                    query = query.OrderByDescending(p => p.Created);
                if (String.Compare(Sort, "Rank", true) == 0)
                    query = query.OrderByDescending(p => p.Rank);
                if (String.Compare(Sort, "Views", true) == 0)
                    query = query.OrderByDescending(p => p.Views);
            }
            List<Macro> macros = query.Skip(Page * PAGE_LENGTH).Take(PAGE_LENGTH).ToList();
            ViewBag.Sort = Sort;
            ViewBag.Tag = Tag;
            ViewBag.MacroCount = query.Count();
            ViewBag.PageSize = PAGE_LENGTH;
            ViewBag.SelectedPage = Page;
            ViewBag.PriorityTags = db.Tags.Where(p => p.PriorityTag).OrderBy(p => p.Tag1).Select(p => p.Tag1).ToList();
            ViewBag.Title = Tag + " Marcos";
            foreach (var macro in macros)
                macro.MacroText = FormatMacroTextForDisplay(macro.MacroText);
            return View(macros);
        }

        [ChildActionOnly]
        public ActionResult MacroCount()
        {
            WowMacroEditorEntities db = new WowMacroEditorEntities();
            ViewBag.MacroCount = db.Macroes.Count();
            return PartialView("_MacroCount");
        }

        //
        // GET: /Default1/Details/5
        [RequireHttp]
        public ActionResult Details(int id, String MacroString)
        {
            Macro macro = db.Macroes.Single(m => m.MacroID == id);
            if (String.IsNullOrEmpty(MacroString) || String.Compare(MacroString, macro.Title.ToFriendlyUrl(), false) != 0)
            {
                MacroString = macro.Title.ToFriendlyUrl();
                return RedirectPermanent(Url.Action("Details", "Macros", new { id = id, MacroString = MacroString }));
            }
            macro.MacroText = FormatMacroTextForDisplay(macro.MacroText);
            macro.Views++;


            Guid? currentUserID = AuthUtil.GetUserID();
            if (currentUserID.HasValue)
            {
                ViewBag.UpVoteClickable = currentUserID.HasValue ? db.RankMacroes.Where(p => p.UserID == currentUserID.Value && p.MacroID == macro.MacroID).Any(v => !v.RankUp) ? "clickable" : "" : "";
                ViewBag.DownVoteClickable = currentUserID.HasValue ? db.RankMacroes.Where(p => p.UserID == currentUserID.Value && p.MacroID == macro.MacroID).Any(v => v.RankUp) ? "clickable" : "" : "";
            }
            else
            {
                ViewBag.UpVoteClickable = "clickable";
                ViewBag.DownVoteClickable = "clickable";
            }


            db.SaveChanges();
            return View(macro);
        }

        [RequireHttp]
        public ActionResult Embed(int id, String MacroString)
        {
            Macro macro = db.Macroes.Single(m => m.MacroID == id);
            if (String.IsNullOrEmpty(MacroString))
            {
                MacroString = macro.Title.ToFriendlyUrl();
                return RedirectToAction("Details", "Macros", new { id = id, MacroString = MacroString });
            }
            macro.MacroText = FormatMacroTextForDisplay(macro.MacroText);
            macro.Views++;
            db.SaveChanges();
            return View(macro);
        }


        [Authorize]
        public void RankMacro(int id, Boolean RankUp)
        {
            if (db.Macroes.Count(p => p.MacroID == id) > 0)
            {
                Macro macro = db.Macroes.First(p => p.MacroID == id);
                Guid UserID = AuthUtil.GetUserID().Value;
                if (db.RankMacroes.Count(p => p.MacroID == id && p.UserID == UserID) > 0)
                {
                    RankMacro rank = db.RankMacroes.First(p => p.MacroID == id && p.UserID == UserID);
                    if (rank.RankUp && RankUp)
                    {

                    }
                    else if (rank.RankUp && !RankUp)
                    {
                        db.RankMacroes.DeleteObject(rank);
                    }
                    else if (!rank.RankUp && RankUp)
                    {
                        db.RankMacroes.DeleteObject(rank);
                    }
                    else
                    {

                    }

                    if (rank.RankUp != RankUp)
                        macro.Rank = macro.Rank + (RankUp ? 1 : -1);

                    db.SaveChanges();
                }
                else
                {
                    RankMacro rank = new RankMacro();
                    rank.MacroID = id;
                    rank.UserID = UserID;
                    rank.RankUp = RankUp;
                    macro.Rank += (RankUp ? 1 : -1);
                    db.RankMacroes.AddObject(rank);
                    db.SaveChanges();
                }
            }
        }

        [RequireHttp]
        public ActionResult Edit(int? id)
        {
            Macro macro = id.HasValue ? db.Macroes.Single(m => m.MacroID == id.Value) : new Macro();
            macro.MacroText = FormatMacroTextForDisplay(macro.MacroText);
            ViewBag.Tags = db.Tags.OrderBy(x => x.Tag1).ToList();
            return View(macro);
        }

        [Authorize]
        [HttpPost]
        [RequireHttp]
        public ActionResult Edit(Macro macro, String[] selectedTags)
        {
            Guid? UserID = AuthUtil.GetUserID();
            if (!UserID.HasValue)
                return View(macro);

            if (ModelState.IsValid)
            {
                if (macro.UserID == Guid.Empty)
                {
                    macro.UserID = UserID.Value;
                    macro.Title.Trim();
                    macro.Created = DateTime.Now;
                    macro.Rank = 0;
                    macro.Views = 0;

                    AddSelectedTagsToMacro(macro, selectedTags);

                    db.Macroes.AddObject(macro);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                Macro editMacro = db.Macroes.First(x => x.MacroID == macro.MacroID);
                if (editMacro.UserID == UserID)
                {
                    editMacro.Description = macro.Description;
                    editMacro.MacroText = macro.MacroText;
                    editMacro.Title = macro.Title;
                    AddSelectedTagsToMacro(editMacro, selectedTags);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(macro);
        }



        //
        // GET: /Default1/Delete/5
        [RequireHttp]
        public ActionResult Delete(int id)
        {
            Macro macro = db.Macroes.Single(m => m.MacroID == id);
            macro.MacroText = FormatMacroTextForDisplay(macro.MacroText);
            return View(macro);
        }

        //
        // POST: /Default1/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [RequireHttp]
        public ActionResult DeleteConfirmed(int id)
        {
            Macro macro = db.Macroes.Include(m => m.Tags).Include(m => m.RankMacroes).Include(m => m.Comments).Single(m => m.MacroID == id);
            macro.Tags.Clear();
            db.SaveChanges();
            macro.RankMacroes.Clear();
            db.SaveChanges();

            db.SaveChanges();
            db.Macroes.DeleteObject(macro);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize]
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        public ActionResult Display(int macroID)
        {
            Guid? currentUserID = AuthUtil.GetUserID();
            DisplayViewModel model = new DisplayViewModel(macroID, currentUserID);
            return View(model);
        }


        private static string FormatMacroTextForDisplay(String macroText)
        {
            if (String.IsNullOrEmpty(macroText))
                return String.Empty;
            return macroText.Replace("\n", "").Replace("/", "\n/").Replace("#", "\n#");
        }

        private void AddSelectedTagsToMacro(Macro macro, String[] selectedTags)
        {
            if (selectedTags != null)
            {
                macro.Tags.Clear();
                foreach (var tagID in selectedTags)
                {
                    int id = Convert.ToInt32(tagID);
                    macro.Tags.Add(db.Tags.First(x => x.TagID == id));
                }
            }
        }

        public JsonResult JsonGetMacroNames(string term = "")
        {
            return Json(db.Macroes.Where(x => x.Title.StartsWith(term)).Select(x => x.Title), JsonRequestBehavior.AllowGet);
        }

        public ActionResult BrowseTemplate()
        {
            return View();
        }

        public JsonResult JsonGetMacroPageCount(int macrosPerPage = PAGE_LENGTH, int page = 0, string term = "", string tag = "")
        {
            var count = Math.Ceiling((double)db.Macroes
                .Where(x => x.Title.Contains(term))
                .Where(x => x.Tags.Count(y => y.Tag1.StartsWith(tag)) > 0).Count() / macrosPerPage);
            return Json(count, JsonRequestBehavior.AllowGet);
        }

        public JsonResult JsonGetMacroData(int? id, int macrosPerPage = PAGE_LENGTH, int page = 0, string term = "", string tag = "")
        {
            Guid? currentUserID = AuthUtil.GetUserID();
            var macroList = db.Macroes
                .Where(x => x.Title.Contains(term))
                .Where(x => x.Tags.Count(y => y.Tag1.StartsWith(tag)) > 0)
                .OrderByDescending(x => x.Rank)
                .Skip(macrosPerPage * page).Take(macrosPerPage)
                .Join(db.CompleteUsers, a => a.UserID, b => b.UserID.Value, (x, userProfile) =>
                new
                {
                    x.MacroID,
                    x.Rank,
                    x.Title,
                    x.Description,
                    Created = x.Created,
                    userProfile.GravatarID,
                    userProfile.DisplayName,
                    userProfile.UserIntID,
                    Tags = x.Tags.Select(t => t.Tag1),
                    CanVoteUp = currentUserID.HasValue ? db.RankMacroes.Where(p => p.UserID == currentUserID.Value && p.MacroID == x.MacroID).Any(v => !v.RankUp) : false,
                    CanVoteDown = currentUserID.HasValue ? db.RankMacroes.Where(p => p.UserID == currentUserID.Value && p.MacroID == x.MacroID).Any(v => v.RankUp) : false
                });

            if (id.HasValue)
                macroList = macroList.Where(x => x.MacroID == id.Value);

            var returnList = macroList.ToList().OrderByDescending(x => x.Rank).Select(x =>
                new
                {
                    x.MacroID,
                    x.Rank,
                    x.Title,
                    Description = x.Description.Substring(0, x.Description.Length > 150 ? 145 : x.Description.Length) + "...",
                    Created = x.Created.ToString(),
                    GravatarUrl = GravatarUtility.GetGravatarUrl(x.GravatarID, 80),
                    x.DisplayName,
                    UserID = x.UserIntID,
                    x.Tags,
                    CanUpVote = x.CanVoteUp || !currentUserID.HasValue || (!x.CanVoteUp && !x.CanVoteDown && currentUserID.HasValue) ? "clickable" : "",
                    CanDownVote = x.CanVoteDown || !currentUserID.HasValue || (!x.CanVoteUp && !x.CanVoteDown && currentUserID.HasValue) ? "clickable" : ""
                });

            return Json(returnList, JsonRequestBehavior.AllowGet);
        }

    }
}