using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SEELahore2k18.Models;
using Microsoft.AspNet.Identity;

namespace SEELahore2k18.Controllers
{
    [Authorize]
    public class StallCategoriesController : Controller
    {
        private SEELahoreEntities db = new SEELahoreEntities();

        // GET: StallCategories
        public ActionResult Index()
        {
            var stallCategories = db.StallCategories.Include(s => s.AspNetUser);
            return View(stallCategories.ToList());
        }

        // GET: StallCategories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StallCategory stallCategory = db.StallCategories.Find(id);
            if (stallCategory == null)
            {
                return HttpNotFound();
            }
            return View(stallCategory);
        }

        // GET: StallCategories/Create
        public ActionResult Create()
        {
            ViewBag.CreatedBy = new SelectList(db.AspNetUsers, "Id", "Email");
            return View();
        }

        // POST: StallCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,StallType,CreatedBy,CreatedAt")] StallCategory stallCategory)
        {
            if (ModelState.IsValid)
            {
                stallCategory.CreatedAt = DateTime.Now;
                stallCategory.CreatedBy = User.Identity.GetUserId();
                db.StallCategories.Add(stallCategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CreatedBy = new SelectList(db.AspNetUsers, "Id", "Email", stallCategory.CreatedBy);
            return View(stallCategory);
        }

        // GET: StallCategories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StallCategory stallCategory = db.StallCategories.Find(id);
            if (stallCategory == null)
            {
                return HttpNotFound();
            }
            ViewBag.CreatedBy = new SelectList(db.AspNetUsers, "Id", "Email", stallCategory.CreatedBy);
            return View(stallCategory);
        }

        // POST: StallCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,StallType,CreatedBy,CreatedAt")] StallCategory stallCategory)
        {
            if (ModelState.IsValid)
            {
                stallCategory.CreatedAt = DateTime.Now;
                stallCategory.CreatedBy = User.Identity.GetUserId();
                db.Entry(stallCategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CreatedBy = new SelectList(db.AspNetUsers, "Id", "Email", stallCategory.CreatedBy);
            return View(stallCategory);
        }

        // GET: StallCategories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StallCategory stallCategory = db.StallCategories.Find(id);
            if (stallCategory == null)
            {
                return HttpNotFound();
            }
            return View(stallCategory);
        }

        // POST: StallCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StallCategory stallCategory = db.StallCategories.Find(id);
            db.StallCategories.Remove(stallCategory);
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
