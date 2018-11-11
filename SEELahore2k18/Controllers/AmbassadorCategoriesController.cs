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
    public class AmbassadorCategoriesController : Controller
    {
        private SEELahoreEntities db = new SEELahoreEntities();

        // GET: AmbassadorCategories
        public ActionResult Index()
        {
            var ambassadorCategories = db.AmbassadorCategories.Include(a => a.AspNetUser);
            return View(ambassadorCategories.ToList());
        }

        // GET: AmbassadorCategories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AmbassadorCategory ambassadorCategory = db.AmbassadorCategories.Find(id);
            if (ambassadorCategory == null)
            {
                return HttpNotFound();
            }
            return View(ambassadorCategory);
        }

        // GET: AmbassadorCategories/Create
        public ActionResult Create()
        {
            ViewBag.CreatedBy = new SelectList(db.AspNetUsers, "Id", "Email");
            return View();
        }

        // POST: AmbassadorCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Category,CreatedBy,CreatedAt")] AmbassadorCategory ambassadorCategory)
        {
            if (ModelState.IsValid)
            {
                //ambassadorCategory.CreatedAt = Convert.ToString(DateTime.Now);
                ambassadorCategory.CreatedBy = User.Identity.GetUserId();
                db.AmbassadorCategories.Add(ambassadorCategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CreatedBy = new SelectList(db.AspNetUsers, "Id", "Email", ambassadorCategory.CreatedBy);
            return View(ambassadorCategory);
        }

        // GET: AmbassadorCategories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AmbassadorCategory ambassadorCategory = db.AmbassadorCategories.Find(id);
            if (ambassadorCategory == null)
            {
                return HttpNotFound();
            }
            ViewBag.CreatedBy = new SelectList(db.AspNetUsers, "Id", "Email", ambassadorCategory.CreatedBy);
            return View(ambassadorCategory);
        }

        // POST: AmbassadorCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Category,CreatedBy,CreatedAt")] AmbassadorCategory ambassadorCategory)
        {
            if (ModelState.IsValid)
            {
                ambassadorCategory.CreatedAt = Convert.ToString(DateTime.Now);
                ambassadorCategory.CreatedBy = User.Identity.GetUserId();
                db.Entry(ambassadorCategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CreatedBy = new SelectList(db.AspNetUsers, "Id", "Email", ambassadorCategory.CreatedBy);
            return View(ambassadorCategory);
        }

        // GET: AmbassadorCategories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AmbassadorCategory ambassadorCategory = db.AmbassadorCategories.Find(id);
            if (ambassadorCategory == null)
            {
                return HttpNotFound();
            }
            return View(ambassadorCategory);
        }

        // POST: AmbassadorCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AmbassadorCategory ambassadorCategory = db.AmbassadorCategories.Find(id);
            db.AmbassadorCategories.Remove(ambassadorCategory);
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
