using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WowMacroEditorMVC.Models;
using System.Web.Security;

namespace WowMacroEditorMVC.Controllers
{
    [RequireHttp]
    public class CommentController : Controller
    {
        private WowMacroEditorEntities db = new WowMacroEditorEntities();


        [ChildActionOnly]
        public ActionResult MacroComments(int id)
        {
            var comments = db.Comments.Where(p => p.MacroID == id).OrderByDescending(p => p.CreatedDate);
            return PartialView("_MacroComments", comments.ToList());
        }

        [ChildActionOnly]
        public ActionResult CreateComment(int id, int? CommentID)
        {
            ViewBag.MacroID = new SelectList(db.Macroes, "MacroID", "Title");
            Comment comment = new Comment();
            comment.MacroID = id;
            return PartialView("_CreateComment", comment);
        }

        //
        // GET: /Comment/Details/5
        [Authorize]
        public ViewResult Details(int id)
        {
            Comment comment = db.Comments.Single(c => c.CommentID == id);
            return View(comment);
        }

        //
        // GET: /Comment/Create
        [Authorize]
        public ActionResult Create(int id)
        {
            ViewBag.MacroID = new SelectList(db.Macroes, "MacroID", "Title");
            Comment comment = new Comment();
            comment.MacroID = id;
            return View(comment);
        }

        //
        // POST: /Comment/Create
        [Authorize]
        [HttpPost]
        public ActionResult Create(Comment comment)
        {
            comment.CreatedDate = DateTime.Now;
            comment.UserID = AuthUtil.GetUserID().Value;
            comment.Rank = 0;
            comment.CreatedDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.Comments.AddObject(comment);
                db.SaveChanges();
                return RedirectToAction("Details", "Macros", new { id = comment.MacroID });
            }
            return RedirectToAction("Details", "Macros", new { id = comment.MacroID });
        }

        //
        // GET: /Comment/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            Comment comment = db.Comments.Single(c => c.CommentID == id);
            ViewBag.MacroID = new SelectList(db.Macroes, "MacroID", "Title", comment.MacroID);
            return View(comment);
        }


        // POST: /Comment/Edit/5
        [Authorize]
        [HttpPost]
        public ActionResult Edit(Comment comment)
        {
            if (ModelState.IsValid)
            {
                db.Comments.Attach(comment);
                db.ObjectStateManager.ChangeObjectState(comment, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MacroID = new SelectList(db.Macroes, "MacroID", "Title", comment.MacroID);
            return View(comment);
        }

        //
        // GET: /Comment/Delete/5
        [Authorize]
        public ActionResult Delete(int id)
        {
            Comment comment = db.Comments.Single(c => c.CommentID == id);
            return View(comment);
        }

        //
        // POST: /Comment/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Comment comment = db.Comments.Single(c => c.CommentID == id);
            int MacroID = comment.Macro.MacroID;
            db.Comments.DeleteObject(comment);
            db.SaveChanges();
            return RedirectToAction("Details", "Macros", new { id = MacroID });
        }

        [Authorize]
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}