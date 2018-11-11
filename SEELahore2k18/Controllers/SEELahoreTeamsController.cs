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
    public class SEELahoreTeamsController : Controller
    {
        private SEELahoreEntities db = new SEELahoreEntities();

        // GET: SEELahoreTeams
        public ActionResult Index()
        {
            var sEELahoreTeams = db.SEELahoreTeams.Include(s => s.AspNetUser);
            return View(sEELahoreTeams.ToList());
        }

        // GET: SEELahoreTeams/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SEELahoreTeam sEELahoreTeam = db.SEELahoreTeams.Find(id);
            if (sEELahoreTeam == null)
            {
                return HttpNotFound();
            }
            return View(sEELahoreTeam);
        }

        // GET: SEELahoreTeams/Create
        public ActionResult Create()
        {
            ViewBag.CreatedBy = new SelectList(db.AspNetUsers, "Id", "Email");
            return View();
        }

        // POST: SEELahoreTeams/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CreatedBy,CreatedAt,ContactNo,Email,Name,Designation,Photo")] SEELahoreTeam sEELahoreTeam, HttpPostedFileBase Photo)
        {
            if (ModelState.IsValid)
            {
                if (Photo != null && Photo.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(Photo.FileName);
                    var path = Path.Combine(Server.MapPath("~/UploadedImages/"), fileName);
                    Photo.SaveAs(path);
                    sEELahoreTeam.Photo = fileName;
                }
                sEELahoreTeam.CreatedAt = DateTime.Now;
                sEELahoreTeam.CreatedBy = User.Identity.GetUserId();
                db.SEELahoreTeams.Add(sEELahoreTeam);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CreatedBy = new SelectList(db.AspNetUsers, "Id", "Email", sEELahoreTeam.CreatedBy);
            return View(sEELahoreTeam);
        }

        // GET: SEELahoreTeams/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SEELahoreTeam sEELahoreTeam = db.SEELahoreTeams.Find(id);
            if (sEELahoreTeam == null)
            {
                return HttpNotFound();
            }
            ViewBag.CreatedBy = new SelectList(db.AspNetUsers, "Id", "Email", sEELahoreTeam.CreatedBy);
            return View(sEELahoreTeam);
        }

        // POST: SEELahoreTeams/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CreatedBy,CreatedAt,ContactNo,Email,Name,Designation,Photo")] SEELahoreTeam sEELahoreTeam, HttpPostedFileBase Photo)
        {
            if (ModelState.IsValid)
            {
                if (Photo != null && Photo.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(Photo.FileName);
                    var path = Path.Combine(Server.MapPath("~/UploadedImages/"), fileName);
                    Photo.SaveAs(path);
                    sEELahoreTeam.Photo = fileName;
                }
                sEELahoreTeam.CreatedAt = DateTime.Now;
                sEELahoreTeam.CreatedBy = User.Identity.GetUserId();
                db.Entry(sEELahoreTeam).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CreatedBy = new SelectList(db.AspNetUsers, "Id", "Email", sEELahoreTeam.CreatedBy);
            return View(sEELahoreTeam);
        }

        // GET: SEELahoreTeams/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SEELahoreTeam sEELahoreTeam = db.SEELahoreTeams.Find(id);
            if (sEELahoreTeam == null)
            {
                return HttpNotFound();
            }
            return View(sEELahoreTeam);
        }

        // POST: SEELahoreTeams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SEELahoreTeam sEELahoreTeam = db.SEELahoreTeams.Find(id);
            db.SEELahoreTeams.Remove(sEELahoreTeam);
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
