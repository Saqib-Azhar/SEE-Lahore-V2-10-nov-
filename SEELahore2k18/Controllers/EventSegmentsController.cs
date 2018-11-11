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
    public class EventSegmentsController : Controller
    {
        private SEELahoreEntities db = new SEELahoreEntities();

        // GET: EventSegments
        public ActionResult Index()
        {
            var eventSegments = db.EventSegments.Include(e => e.AspNetUser).Include(e => e.EventDate);
            return View(eventSegments.ToList());
        }

        // GET: EventSegments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventSegment eventSegment = db.EventSegments.Find(id);
            if (eventSegment == null)
            {
                return HttpNotFound();
            }
            return View(eventSegment);
        }

        // GET: EventSegments/Create
        public ActionResult Create()
        {
            ViewBag.CreatedBy = new SelectList(db.AspNetUsers, "Id", "Email");
            ViewBag.EventDayId = new SelectList(db.EventDates, "Id", "Id");
            return View();
        }

        // POST: EventSegments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,From,To,SegmentName,SegmentDescription,Image,EventDayId,CreatedBy,CreatedAt")] EventSegment eventSegment, HttpPostedFileBase Image)
        {
            if (ModelState.IsValid)
            {
                if (Image != null && Image.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(Image.FileName);
                    var path = Path.Combine(Server.MapPath("~/UploadedImages/"), fileName);
                    Image.SaveAs(path);
                    eventSegment.Image = fileName;
                }
                eventSegment.CreatedAt = DateTime.Now;
                eventSegment.CreatedBy = User.Identity.GetUserId();
                db.EventSegments.Add(eventSegment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CreatedBy = new SelectList(db.AspNetUsers, "Id", "Email", eventSegment.CreatedBy);
            ViewBag.EventDayId = new SelectList(db.EventDates, "Id", "Id", eventSegment.EventDayId);
            return View(eventSegment);
        }

        // GET: EventSegments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventSegment eventSegment = db.EventSegments.Find(id);
            if (eventSegment == null)
            {
                return HttpNotFound();
            }
            ViewBag.CreatedBy = new SelectList(db.AspNetUsers, "Id", "Email", eventSegment.CreatedBy);
            ViewBag.EventDayId = new SelectList(db.EventDates, "Id", "Id", eventSegment.EventDayId);
            return View(eventSegment);
        }

        // POST: EventSegments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,From,To,SegmentName,SegmentDescription,Image,EventDayId,CreatedBy,CreatedAt")] EventSegment eventSegment, HttpPostedFileBase Image)
        {
            if (ModelState.IsValid)
            {
                if (Image != null && Image.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(Image.FileName);
                    var path = Path.Combine(Server.MapPath("~/UploadedImages/"), fileName);
                    Image.SaveAs(path);
                    eventSegment.Image = fileName;
                }
                eventSegment.CreatedAt = DateTime.Now;
                eventSegment.CreatedBy = User.Identity.GetUserId();
                db.Entry(eventSegment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CreatedBy = new SelectList(db.AspNetUsers, "Id", "Email", eventSegment.CreatedBy);
            ViewBag.EventDayId = new SelectList(db.EventDates, "Id", "Id", eventSegment.EventDayId);
            return View(eventSegment);
        }

        // GET: EventSegments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventSegment eventSegment = db.EventSegments.Find(id);
            if (eventSegment == null)
            {
                return HttpNotFound();
            }
            return View(eventSegment);
        }

        // POST: EventSegments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EventSegment eventSegment = db.EventSegments.Find(id);
            db.EventSegments.Remove(eventSegment);
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
