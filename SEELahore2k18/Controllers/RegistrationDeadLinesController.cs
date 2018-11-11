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
    public class RegistrationDeadLinesController : Controller
    {
        private SEELahoreEntities db = new SEELahoreEntities();

        // GET: RegistrationDeadLines
        public ActionResult Index()
        {
            return View(db.RegistrationDeadLines.ToList());
        }

        // GET: RegistrationDeadLines/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RegistrationDeadLine registrationDeadLine = db.RegistrationDeadLines.Find(id);
            if (registrationDeadLine == null)
            {
                return HttpNotFound();
            }
            return View(registrationDeadLine);
        }

        // GET: RegistrationDeadLines/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: RegistrationDeadLines/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Id,RegistrationType,From,To")] RegistrationDeadLine registrationDeadLine)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.RegistrationDeadLines.Add(registrationDeadLine);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(registrationDeadLine);
        //}

        // GET: RegistrationDeadLines/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RegistrationDeadLine registrationDeadLine = db.RegistrationDeadLines.Find(id);
            if (registrationDeadLine == null)
            {
                return HttpNotFound();
            }
            ViewBag.RegistrationType = registrationDeadLine.RegistrationType;
            return View(registrationDeadLine);
        }

        // POST: RegistrationDeadLines/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,From,To")] RegistrationDeadLine registrationDeadLine)
        {
            if (ModelState.IsValid)
            {
                var type = Request.Form["RegistrationType"];
                var obj = db.RegistrationDeadLines.Find(registrationDeadLine.Id);
                obj.To = registrationDeadLine.To;
                obj.From = registrationDeadLine.From;
                //db.Entry(registrationDeadLine).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(registrationDeadLine);
        }

        // GET: RegistrationDeadLines/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RegistrationDeadLine registrationDeadLine = db.RegistrationDeadLines.Find(id);
            if (registrationDeadLine == null)
            {
                return HttpNotFound();
            }
            return View(registrationDeadLine);
        }

        // POST: RegistrationDeadLines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RegistrationDeadLine registrationDeadLine = db.RegistrationDeadLines.Find(id);
            db.RegistrationDeadLines.Remove(registrationDeadLine);
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
