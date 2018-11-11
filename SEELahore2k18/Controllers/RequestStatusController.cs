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
    public class RequestStatusController : Controller
    {
        private SEELahoreEntities db = new SEELahoreEntities();

        // GET: RequestStatus
        public ActionResult Index()
        {
            return View(db.RequestStatus.ToList());
        }

        // GET: RequestStatus/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RequestStatu requestStatu = db.RequestStatus.Find(id);
            if (requestStatu == null)
            {
                return HttpNotFound();
            }
            return View(requestStatu);
        }

        // GET: RequestStatus/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RequestStatus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Status")] RequestStatu requestStatu)
        {
            if (ModelState.IsValid)
            {
                db.RequestStatus.Add(requestStatu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(requestStatu);
        }

        // GET: RequestStatus/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RequestStatu requestStatu = db.RequestStatus.Find(id);
            if (requestStatu == null)
            {
                return HttpNotFound();
            }
            return View(requestStatu);
        }

        // POST: RequestStatus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Status")] RequestStatu requestStatu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(requestStatu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(requestStatu);
        }

        // GET: RequestStatus/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RequestStatu requestStatu = db.RequestStatus.Find(id);
            if (requestStatu == null)
            {
                return HttpNotFound();
            }
            return View(requestStatu);
        }

        // POST: RequestStatus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RequestStatu requestStatu = db.RequestStatus.Find(id);
            db.RequestStatus.Remove(requestStatu);
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
