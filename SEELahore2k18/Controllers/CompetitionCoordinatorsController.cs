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
    public class CompetitionCoordinatorsController : Controller
    {
        private SEELahoreEntities db = new SEELahoreEntities();

        // GET: CompetitionCoordinators
        public ActionResult Index()
        {
            var competitionCoordinators = db.CompetitionCoordinators.Include(c => c.AspNetUser).Include(c => c.Competition);
            return View(competitionCoordinators.ToList());
        }

        // GET: CompetitionCoordinators/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompetitionCoordinator competitionCoordinator = db.CompetitionCoordinators.Find(id);
            if (competitionCoordinator == null)
            {
                return HttpNotFound();
            }
            return View(competitionCoordinator);
        }

        // GET: CompetitionCoordinators/Create
        public ActionResult Create()
        {
            ViewBag.CreatedBy = new SelectList(db.AspNetUsers, "Id", "Email");
            ViewBag.CompetitionId = new SelectList(db.Competitions, "Id", "CompetitionName");
            return View();
        }

        // POST: CompetitionCoordinators/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CoordinatorName,Photo,Contact,Email,CompetitionId,CreatedAt,CreatedBy")] CompetitionCoordinator competitionCoordinator, HttpPostedFileBase Photo)
        {
            if (ModelState.IsValid)
            {
                if (Photo != null && Photo.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(Photo.FileName);
                    var path = Path.Combine(Server.MapPath("~/UploadedImages/"), fileName);
                    Photo.SaveAs(path);
                    competitionCoordinator.Photo = fileName;
                }
                competitionCoordinator.CreatedAt = DateTime.Now;
                competitionCoordinator.CreatedBy = User.Identity.GetUserId();
                db.CompetitionCoordinators.Add(competitionCoordinator);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CreatedBy = new SelectList(db.AspNetUsers, "Id", "Email", competitionCoordinator.CreatedBy);
            ViewBag.CompetitionId = new SelectList(db.Competitions, "Id", "CompetitionName", competitionCoordinator.CompetitionId);
            return View(competitionCoordinator);
        }

        // GET: CompetitionCoordinators/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompetitionCoordinator competitionCoordinator = db.CompetitionCoordinators.Find(id);
            if (competitionCoordinator == null)
            {
                return HttpNotFound();
            }
            ViewBag.CreatedBy = new SelectList(db.AspNetUsers, "Id", "Email", competitionCoordinator.CreatedBy);
            ViewBag.CompetitionId = new SelectList(db.Competitions, "Id", "CompetitionName", competitionCoordinator.CompetitionId);
            return View(competitionCoordinator);
        }

        // POST: CompetitionCoordinators/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CoordinatorName,Photo,Contact,Email,CompetitionId,CreatedAt,CreatedBy")] CompetitionCoordinator competitionCoordinator, HttpPostedFileBase Photo)
        {
            if (ModelState.IsValid)
            {
                if (Photo != null && Photo.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(Photo.FileName);
                    var path = Path.Combine(Server.MapPath("~/UploadedImages/"), fileName);
                    Photo.SaveAs(path);
                    competitionCoordinator.Photo = fileName;
                }
                competitionCoordinator.CreatedAt = DateTime.Now;
                competitionCoordinator.CreatedBy = User.Identity.GetUserId();
                db.Entry(competitionCoordinator).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CreatedBy = new SelectList(db.AspNetUsers, "Id", "Email", competitionCoordinator.CreatedBy);
            ViewBag.CompetitionId = new SelectList(db.Competitions, "Id", "CompetitionName", competitionCoordinator.CompetitionId);
            return View(competitionCoordinator);
        }

        // GET: CompetitionCoordinators/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompetitionCoordinator competitionCoordinator = db.CompetitionCoordinators.Find(id);
            if (competitionCoordinator == null)
            {
                return HttpNotFound();
            }
            return View(competitionCoordinator);
        }

        // POST: CompetitionCoordinators/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CompetitionCoordinator competitionCoordinator = db.CompetitionCoordinators.Find(id);
            db.CompetitionCoordinators.Remove(competitionCoordinator);
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
