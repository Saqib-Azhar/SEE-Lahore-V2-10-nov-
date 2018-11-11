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
//    public class VolunteersAndAmbassadorsController : Controller
//    {
//        private DefaultConnection db = new DefaultConnection();

//        // GET: VolunteersAndAmbassadors
//        public ActionResult Index()
//        {
//            var volunteersAndAmbassadors = db.VolunteersAndAmbassadors.Include(v => v.RequestStatu);
//            return View(volunteersAndAmbassadors.ToList());
//        }

//        // GET: VolunteersAndAmbassadors/Details/5
//        public ActionResult Details(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            VolunteersAndAmbassador volunteersAndAmbassador = db.VolunteersAndAmbassadors.Find(id);
//            if (volunteersAndAmbassador == null)
//            {
//                return HttpNotFound();
//            }
//            return View(volunteersAndAmbassador);
//        }

//        // GET: VolunteersAndAmbassadors/Create
//        public ActionResult Create()
//        {
//            ViewBag.StatusId = new SelectList(db.RequestStatus, "Id", "Status");
//            return View();
//        }

//        // POST: VolunteersAndAmbassadors/Create
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create([Bind(Include = "Id,Name,ContactNo,Designation,EmailId,CNIC,Institute,StatusId,CreatedAt,Address,City")] VolunteersAndAmbassador volunteersAndAmbassador)
//        {
//            if (ModelState.IsValid)
//            {
//                db.VolunteersAndAmbassadors.Add(volunteersAndAmbassador);
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }

//            ViewBag.StatusId = new SelectList(db.RequestStatus, "Id", "Status", volunteersAndAmbassador.StatusId);
//            return View(volunteersAndAmbassador);
//        }

//        // GET: VolunteersAndAmbassadors/Edit/5
//        public ActionResult Edit(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            VolunteersAndAmbassador volunteersAndAmbassador = db.VolunteersAndAmbassadors.Find(id);
//            if (volunteersAndAmbassador == null)
//            {
//                return HttpNotFound();
//            }
//            ViewBag.StatusId = new SelectList(db.RequestStatus, "Id", "Status", volunteersAndAmbassador.StatusId);
//            return View(volunteersAndAmbassador);
//        }

//        // POST: VolunteersAndAmbassadors/Edit/5
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Edit([Bind(Include = "Id,Name,ContactNo,Designation,EmailId,CNIC,Institute,StatusId,CreatedAt,Address,City")] VolunteersAndAmbassador volunteersAndAmbassador)
//        {
//            if (ModelState.IsValid)
//            {
//                db.Entry(volunteersAndAmbassador).State = EntityState.Modified;
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }
//            ViewBag.StatusId = new SelectList(db.RequestStatus, "Id", "Status", volunteersAndAmbassador.StatusId);
//            return View(volunteersAndAmbassador);
//        }

//        // GET: VolunteersAndAmbassadors/Delete/5
//        public ActionResult Delete(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            VolunteersAndAmbassador volunteersAndAmbassador = db.VolunteersAndAmbassadors.Find(id);
//            if (volunteersAndAmbassador == null)
//            {
//                return HttpNotFound();
//            }
//            return View(volunteersAndAmbassador);
//        }

//        // POST: VolunteersAndAmbassadors/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public ActionResult DeleteConfirmed(int id)
//        {
//            VolunteersAndAmbassador volunteersAndAmbassador = db.VolunteersAndAmbassadors.Find(id);
//            db.VolunteersAndAmbassadors.Remove(volunteersAndAmbassador);
//            db.SaveChanges();
//            return RedirectToAction("Index");
//        }

//        public ActionResult UpdateStatus(int? id, int? Status)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            VolunteersAndAmbassador volunteersAndAmbassador = db.VolunteersAndAmbassadors.Find(id);
//            if (volunteersAndAmbassador == null)
//            {
//                return HttpNotFound();
//            }
//            volunteersAndAmbassador.StatusId = Status;
//            db.SaveChanges();

//            return RedirectToAction("Index");
//        }

//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                db.Dispose();
//            }
//            base.Dispose(disposing);
//        }
//    }
}
