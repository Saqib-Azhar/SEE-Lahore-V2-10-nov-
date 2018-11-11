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
    public class SeasonGalleriesController : Controller
    {
        private SEELahoreEntities db = new SEELahoreEntities();

        // GET: SeasonGalleries
        public ActionResult Index()
        {
            var seasonGalleries = db.SeasonGalleries.Include(s => s.AspNetUser).Include(s => s.Season);
            return View(seasonGalleries.ToList());
        }

        // GET: SeasonGalleries/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SeasonGallery seasonGallery = db.SeasonGalleries.Find(id);
            if (seasonGallery == null)
            {
                return HttpNotFound();
            }
            return View(seasonGallery);
        }

        // GET: SeasonGalleries/Create
        public ActionResult Create()
        {
            ViewBag.CreatedBy = new SelectList(db.AspNetUsers, "Id", "Email");
            ViewBag.SeasonId = new SelectList(db.Seasons, "Id", "Name");
            return View();
        }

        // POST: SeasonGalleries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CreatedBy,CreatedAt,Image,SeasonId")] SeasonGallery seasonGallery, HttpPostedFileBase Image)
        {
            if (ModelState.IsValid)
            {
                if (Image != null && Image.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(Image.FileName);
                    var path = Path.Combine(Server.MapPath("~/UploadedImages/"), fileName);
                    Image.SaveAs(path);
                    seasonGallery.Image = fileName;
                }
                seasonGallery.CreatedAt = DateTime.Now;
                seasonGallery.CreatedBy = User.Identity.GetUserId();
                db.SeasonGalleries.Add(seasonGallery);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CreatedBy = new SelectList(db.AspNetUsers, "Id", "Email", seasonGallery.CreatedBy);
            ViewBag.SeasonId = new SelectList(db.Seasons, "Id", "Name", seasonGallery.SeasonId);
            return View(seasonGallery);
        }

        // GET: SeasonGalleries/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SeasonGallery seasonGallery = db.SeasonGalleries.Find(id);
            if (seasonGallery == null)
            {
                return HttpNotFound();
            }
            ViewBag.CreatedBy = new SelectList(db.AspNetUsers, "Id", "Email", seasonGallery.CreatedBy);
            ViewBag.SeasonId = new SelectList(db.Seasons, "Id", "Name", seasonGallery.SeasonId);
            return View(seasonGallery);
        }

        // POST: SeasonGalleries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CreatedBy,CreatedAt,Image,SeasonId")] SeasonGallery seasonGallery, HttpPostedFileBase Image)
        {
            if (ModelState.IsValid)
            {
                if (Image != null && Image.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(Image.FileName);
                    var path = Path.Combine(Server.MapPath("~/UploadedImages/"), fileName);
                    Image.SaveAs(path);
                    seasonGallery.Image = fileName;
                }
                seasonGallery.CreatedAt = DateTime.Now;
                seasonGallery.CreatedBy = User.Identity.GetUserId();
                db.Entry(seasonGallery).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CreatedBy = new SelectList(db.AspNetUsers, "Id", "Email", seasonGallery.CreatedBy);
            ViewBag.SeasonId = new SelectList(db.Seasons, "Id", "Name", seasonGallery.SeasonId);
            return View(seasonGallery);
        }

        // GET: SeasonGalleries/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SeasonGallery seasonGallery = db.SeasonGalleries.Find(id);
            if (seasonGallery == null)
            {
                return HttpNotFound();
            }
            return View(seasonGallery);
        }

        // POST: SeasonGalleries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SeasonGallery seasonGallery = db.SeasonGalleries.Find(id);
            db.SeasonGalleries.Remove(seasonGallery);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        public ActionResult Seasons()
        {
            var seasonsList = db.Seasons.ToList();
            return View(seasonsList);
        }

        [AllowAnonymous]
        public ActionResult Gallery(int? seasonId)
        {
            var seasonGalleries = db.SeasonGalleries.Where(s=>s.SeasonId == seasonId).Include(s => s.AspNetUser).Include(s => s.Season);
            return View(seasonGalleries.ToList());
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
