using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SEELahore2k18.Models;
using System.IO;
using Microsoft.AspNet.Identity;

namespace SEELahore2k18.Controllers
{
    [Authorize]
    public class CompetitionsController : Controller
    {
        private SEELahoreEntities db = new SEELahoreEntities();

        // GET: Competitions
        public ActionResult Index()
        {
            var competitions = db.Competitions.Include(c => c.AspNetUser);
            return View(competitions.ToList());
        }

        // GET: Competitions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Competition competition = db.Competitions.Find(id);
            if (competition == null)
            {
                return HttpNotFound();
            }
            return View(competition);
        }

        // GET: Competitions/Create
        public ActionResult Create()
        {
            ViewBag.CreatedBy = new SelectList(db.AspNetUsers, "Id", "Email");
            return View();
        }

        // POST: Competitions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CompetitionName,CompetitionDescription,Image1,Image2,Image3,Image4,CreatedAt,CreatedBy")] Competition competition, HttpPostedFileBase Image1, HttpPostedFileBase Image2, HttpPostedFileBase Image3, HttpPostedFileBase Image4)
        {
            if (ModelState.IsValid)
            {
                if (Request.Files.Count > 0)
                {
                    if (Image1 != null && Image1.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(Image1.FileName);
                        var path = Path.Combine(Server.MapPath("~/UploadedImages/"), fileName);
                        Image1.SaveAs(path);
                        competition.Image1 = fileName;
                    }
                    if (Image2 != null && Image2.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(Image2.FileName);
                        var path = Path.Combine(Server.MapPath("~/UploadedImages/"), fileName);
                        Image2.SaveAs(path);
                        competition.Image2 = fileName;
                    }
                    if (Image3 != null && Image3.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(Image3.FileName);
                        var path = Path.Combine(Server.MapPath("~/UploadedImages/"), fileName);
                        Image3.SaveAs(path);
                        competition.Image3 = fileName;
                    }
                    if (Image4 != null && Image4.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(Image4.FileName);
                        var path = Path.Combine(Server.MapPath("~/UploadedImages/"), fileName);
                        Image4.SaveAs(path);
                        competition.Image4 = fileName;
                    }
                }
                competition.CreatedAt = DateTime.Now;
                competition.CreatedBy = User.Identity.GetUserId();
                db.Competitions.Add(competition);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CreatedBy = new SelectList(db.AspNetUsers, "Id", "Email", competition.CreatedBy);
            return View(competition);
        }

        // GET: Competitions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Competition competition = db.Competitions.Find(id);
            if (competition == null)
            {
                return HttpNotFound();
            }
            ViewBag.CreatedBy = new SelectList(db.AspNetUsers, "Id", "Email", competition.CreatedBy);
            return View(competition);
        }

        // POST: Competitions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CompetitionName,CompetitionDescription,Image1,Image2,Image3,Image4,CreatedAt,CreatedBy")] Competition competition, HttpPostedFileBase Image1, HttpPostedFileBase Image2, HttpPostedFileBase Image3, HttpPostedFileBase Image4)
        {
            if (ModelState.IsValid)
            {
                if (Request.Files.Count > 0)
                {
                    if (Image1 != null && Image1.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(Image1.FileName);
                        var path = Path.Combine(Server.MapPath("~/UploadedImages/"), fileName);
                        Image1.SaveAs(path);
                        competition.Image1 = fileName;
                    }
                    if (Image2 != null && Image2.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(Image2.FileName);
                        var path = Path.Combine(Server.MapPath("~/UploadedImages/"), fileName);
                        Image2.SaveAs(path);
                        competition.Image2 = fileName;
                    }
                    if (Image3 != null && Image3.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(Image3.FileName);
                        var path = Path.Combine(Server.MapPath("~/UploadedImages/"), fileName);
                        Image3.SaveAs(path);
                        competition.Image3 = fileName;
                    }
                    if (Image4 != null && Image4.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(Image4.FileName);
                        var path = Path.Combine(Server.MapPath("~/UploadedImages/"), fileName);
                        Image4.SaveAs(path);
                        competition.Image4 = fileName;
                    }
                }
                competition.CreatedAt = DateTime.Now;
                competition.CreatedBy = User.Identity.GetUserId();
                db.Entry(competition).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CreatedBy = new SelectList(db.AspNetUsers, "Id", "Email", competition.CreatedBy);
            return View(competition);
        }

        // GET: Competitions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Competition competition = db.Competitions.Find(id);
            if (competition == null)
            {
                return HttpNotFound();
            }
            return View(competition);
        }

        // POST: Competitions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Competition competition = db.Competitions.Find(id);
            db.Competitions.Remove(competition);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
