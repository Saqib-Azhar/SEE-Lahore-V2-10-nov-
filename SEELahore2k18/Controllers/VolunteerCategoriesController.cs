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
    public class VolunteerCategoriesController : Controller
    {
        private SEELahoreEntities db = new SEELahoreEntities();

        // GET: VolunteerCategories
        public ActionResult Index()
        {
            var volunteerCategories = db.VolunteerCategories.Include(v => v.AspNetUser);
            return View(volunteerCategories.ToList());
        }

        // GET: VolunteerCategories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VolunteerCategory volunteerCategory = db.VolunteerCategories.Find(id);
            if (volunteerCategory == null)
            {
                return HttpNotFound();
            }
            return View(volunteerCategory);
        }

        // GET: VolunteerCategories/Create
        public ActionResult Create()
        {
            ViewBag.CreatedBy = new SelectList(db.AspNetUsers, "Id", "Email");
            return View();
        }

        // POST: VolunteerCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Category,CreatedBy,CreatedAt")] VolunteerCategory volunteerCategory)
        {
            if (ModelState.IsValid)
            {
                //volunteerCategory.CreatedAt = Convert.ToString(DateTime.Now);
                volunteerCategory.CreatedBy = User.Identity.GetUserId();
                db.VolunteerCategories.Add(volunteerCategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CreatedBy = new SelectList(db.AspNetUsers, "Id", "Email", volunteerCategory.CreatedBy);
            return View(volunteerCategory);
        }

        // GET: VolunteerCategories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VolunteerCategory volunteerCategory = db.VolunteerCategories.Find(id);
            if (volunteerCategory == null)
            {
                return HttpNotFound();
            }
            ViewBag.CreatedBy = new SelectList(db.AspNetUsers, "Id", "Email", volunteerCategory.CreatedBy);
            return View(volunteerCategory);
        }

        // POST: VolunteerCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Category,CreatedBy,CreatedAt")] VolunteerCategory volunteerCategory)
        {
            if (ModelState.IsValid)
            {
                volunteerCategory.CreatedAt = Convert.ToString(DateTime.Now);
                volunteerCategory.CreatedBy = User.Identity.GetUserId();
                db.Entry(volunteerCategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CreatedBy = new SelectList(db.AspNetUsers, "Id", "Email", volunteerCategory.CreatedBy);
            return View(volunteerCategory);
        }

        // GET: VolunteerCategories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VolunteerCategory volunteerCategory = db.VolunteerCategories.Find(id);
            if (volunteerCategory == null)
            {
                return HttpNotFound();
            }
            return View(volunteerCategory);
        }

        // POST: VolunteerCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            VolunteerCategory volunteerCategory = db.VolunteerCategories.Find(id);
            db.VolunteerCategories.Remove(volunteerCategory);
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
