using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SEELahore2k18.Models;

namespace SEELahore2k18.Controllers
{
    [Authorize]
    public class EventDatesController : Controller
    {
        private SEELahoreEntities db = new SEELahoreEntities();

        // GET: EventDates
        public ActionResult Index()
        {
            return View(db.EventDates.ToList());
        }

        // GET: EventDates/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventDate eventDate = db.EventDates.Find(id);
            if (eventDate == null)
            {
                return HttpNotFound();
            }
            return View(eventDate);
        }

        // GET: EventDates/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EventDates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Date")] EventDate eventDate)
        {
            if (ModelState.IsValid)
            {
                db.EventDates.Add(eventDate);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(eventDate);
        }

        // GET: EventDates/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventDate eventDate = db.EventDates.Find(id);
            if (eventDate == null)
            {
                return HttpNotFound();
            }
            return View(eventDate);
        }

        // POST: EventDates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Date")] EventDate eventDate)
        {
            if (ModelState.IsValid)
            {
                db.Entry(eventDate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(eventDate);
        }

        // GET: EventDates/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventDate eventDate = db.EventDates.Find(id);
            if (eventDate == null)
            {
                return HttpNotFound();
            }
            return View(eventDate);
        }

        // POST: EventDates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EventDate eventDate = db.EventDates.Find(id);
            db.EventDates.Remove(eventDate);
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
