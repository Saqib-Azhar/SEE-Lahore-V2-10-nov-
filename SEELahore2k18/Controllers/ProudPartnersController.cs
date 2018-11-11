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
    public class ProudPartnersController : Controller
    {
        private SEELahoreEntities db = new SEELahoreEntities();

        // GET: ProudPartners
        public ActionResult Index()
        {
            var proudPartners = db.ProudPartners.Include(p => p.AspNetUser);
            return View(proudPartners.ToList());
        }

        // GET: ProudPartners/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProudPartner proudPartner = db.ProudPartners.Find(id);
            if (proudPartner == null)
            {
                return HttpNotFound();
            }
            return View(proudPartner);
        }

        // GET: ProudPartners/Create
        public ActionResult Create()
        {
            ViewBag.CreatedBy = new SelectList(db.AspNetUsers, "Id", "Email");
            return View();
        }

        // POST: ProudPartners/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CreatedBy,CreatedAt,PartnerName,Description,Logo")] ProudPartner proudPartner, HttpPostedFileBase Logo)
        {
            if (ModelState.IsValid)
            {
                if (Logo != null && Logo.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(Logo.FileName);
                    var path = Path.Combine(Server.MapPath("~/UploadedImages/"), fileName);
                    Logo.SaveAs(path);
                    proudPartner.Logo = fileName;
                }
                proudPartner.CreatedAt = DateTime.Now;
                proudPartner.CreatedBy = User.Identity.GetUserId();
                db.ProudPartners.Add(proudPartner);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CreatedBy = new SelectList(db.AspNetUsers, "Id", "Email", proudPartner.CreatedBy);
            return View(proudPartner);
        }

        // GET: ProudPartners/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProudPartner proudPartner = db.ProudPartners.Find(id);
            if (proudPartner == null)
            {
                return HttpNotFound();
            }
            ViewBag.CreatedBy = new SelectList(db.AspNetUsers, "Id", "Email", proudPartner.CreatedBy);
            return View(proudPartner);
        }

        // POST: ProudPartners/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CreatedBy,CreatedAt,PartnerName,Description,Logo")] ProudPartner proudPartner, HttpPostedFileBase Logo)
        {
            if (ModelState.IsValid)
            {
                if (Logo != null && Logo.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(Logo.FileName);
                    var path = Path.Combine(Server.MapPath("~/UploadedImages/"), fileName);
                    Logo.SaveAs(path);
                    proudPartner.Logo = fileName;
                }
                proudPartner.CreatedAt = DateTime.Now;
                proudPartner.CreatedBy = User.Identity.GetUserId();
                db.Entry(proudPartner).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CreatedBy = new SelectList(db.AspNetUsers, "Id", "Email", proudPartner.CreatedBy);
            return View(proudPartner);
        }

        // GET: ProudPartners/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProudPartner proudPartner = db.ProudPartners.Find(id);
            if (proudPartner == null)
            {
                return HttpNotFound();
            }
            return View(proudPartner);
        }

        // POST: ProudPartners/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProudPartner proudPartner = db.ProudPartners.Find(id);
            db.ProudPartners.Remove(proudPartner);
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
