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
using System.Data.Entity.Validation;

namespace SEELahore2k18.Controllers
{
    [Authorize]
    public class StallRequestsController : Controller
    {
        private SEELahoreEntities db = new SEELahoreEntities();

        // GET: StallRequests
        public ActionResult Index(int? type = 0)
        {
            if (type != 0)
            {
            ViewBag.InstituteId = new SelectList(db.Institutes, "Id", "Institute1");
                var stallRequests = db.StallRequests.Where(s => s.RequestStatusId == type).OrderByDescending(s => s.Id).Include(s => s.AspNetUser).Include(s => s.RequestStatu).Include(s => s.StallCategory);
                return View(stallRequests.ToList());
            }
            else
            {
            ViewBag.InstituteId = new SelectList(db.Institutes, "Id", "Institute1");
                var stallRequests = db.StallRequests.Include(s => s.AspNetUser).OrderByDescending(s => s.Id).Include(s => s.RequestStatu).Include(s => s.StallCategory);
                return View(stallRequests.ToList());
            }
        }

        // GET: StallRequests/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StallRequest stallRequest = db.StallRequests.Find(id);
            if (stallRequest == null)
            {
                return HttpNotFound();
            }
            return View(stallRequest);
        }

        [AllowAnonymous]
        // GET: StallRequests/Create
        public ActionResult Create()
        {


            try
            {
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                var dateRange = db.RegistrationDeadLines.FirstOrDefault(s => s.RegistrationType == controllerName);
                var comparisonto = (DateTime.Compare(Convert.ToDateTime(DateTime.Now), Convert.ToDateTime(dateRange.To)));
                var comparisonfrom = (DateTime.Compare(Convert.ToDateTime(DateTime.Now), Convert.ToDateTime(dateRange.From)));

                if (comparisonto != -1)
                {
                    return RedirectToAction("RegistrationDeadline", "Home", new { status = "Registrations Ended" });
                }
                else if (comparisonfrom != 1)
                {
                    return RedirectToAction("RegistrationDeadline", "Home", new { status = "Registrations will be open soon!" });
                }
            }
            catch (Exception ex)
            {

                HomeController.infoMessage(ex.Message);
                HomeController.writeErrorLog(ex);

            }

            ViewBag.CreatedBy = new SelectList(db.AspNetUsers, "Id", "Email");
            ViewBag.RequestStatusId = new SelectList(db.RequestStatus, "Id", "Status");
            ViewBag.InstituteId = new SelectList(db.Institutes, "Id", "Institute1");
            ViewBag.CategoryId = new SelectList(db.StallCategories, "Id", "StallType");
            return View();
        }

        // POST: StallRequests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [AllowAnonymous]

        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,StallName,StallDetails,Logo,RequestStatusId,CategoryId,CreatedBy,CreatedAt,OwnerName,ContactNo,Email,Address,City,Profession,Institute")] StallRequest stallRequest, HttpPostedFileBase Logo)
        {

            try
            {

                if (ModelState.IsValid)
                {
                    var obj = db.StallRequests.FirstOrDefault(s => s.Email == stallRequest.Email);
                    if (obj != null)
                    {
                        ViewBag.ErrorMessage = "Email Already Exists!";
                        ViewBag.CreatedBy = new SelectList(db.AspNetUsers, "Id", "Email", stallRequest.CreatedBy);
                        ViewBag.InstituteId = new SelectList(db.Institutes, "Id", "Institute1");
                        ViewBag.RequestStatusId = new SelectList(db.RequestStatus, "Id", "Status", stallRequest.RequestStatusId);
                        ViewBag.CategoryId = new SelectList(db.StallCategories, "Id", "StallType", stallRequest.CategoryId);
                        return View(stallRequest);
                    }
                    if (Logo != null && Logo.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(Logo.FileName);
                        var path = Path.Combine(Server.MapPath("~/UploadedImages/"), fileName);
                        Logo.SaveAs(path);
                        stallRequest.Logo = fileName;
                    }
                    try
                    {
                        stallRequest.RequestStatusId = 1;
                        stallRequest.CreatedAt = DateTime.Now;
                        stallRequest.CreatedBy = User.Identity.GetUserId();
                        db.StallRequests.Add(stallRequest);
                        db.SaveChanges();
                        string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                        string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                        return RedirectToAction("SubmissionResponce", "Home", new { status = "Your stall proposal is successfully registerd with your crdentials,Team SEE Lahore will soon respond you through Email.Stay Connected for Bigest Event of Lahore,See Lahore 2018", url = controllerName + "/" + actionName });
                    }
                    catch (DbEntityValidationException ex)
                    {
                        string message = "";
                        foreach (var validationErrors in ex.EntityValidationErrors)
                        {
                            foreach (var validationError in validationErrors.ValidationErrors)
                            {
                                message = message + validationError.PropertyName + "  " + validationError.ErrorMessage + "\n\n";
                            }
                        }

                        HomeController.EntityinfoMessage(stallRequest.OwnerName + ": " + message);
                        HomeController.EntitywriteErrorLog(ex);
                        string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                        string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                        return RedirectToAction("SubmissionResponce", "Home", new { status = "Due to Server overload something went wrong! please try again. Sorry for Inconvenience", url = controllerName + "/" + actionName });

                    }
                }
                ViewBag.CreatedBy = new SelectList(db.AspNetUsers, "Id", "Email", stallRequest.CreatedBy);
                ViewBag.InstituteId = new SelectList(db.Institutes, "Id", "Institute1");
                ViewBag.RequestStatusId = new SelectList(db.RequestStatus, "Id", "Status", stallRequest.RequestStatusId);
                ViewBag.CategoryId = new SelectList(db.StallCategories, "Id", "StallType", stallRequest.CategoryId);
                return View(stallRequest);

            }
            catch (Exception ex)
            {

                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                HomeController.infoMessage(ex.Message);
                HomeController.writeErrorLog(ex);
                return RedirectToAction("SubmissionResponce", "Home", new { status = "Due to Server overload something went wrong! please try again. Sorry for Inconvenience", url = controllerName + "/" + actionName });

            }
            
        }

        // GET: StallRequests/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StallRequest stallRequest = db.StallRequests.Find(id);
            if (stallRequest == null)
            {
                return HttpNotFound();
            }
            ViewBag.CreatedBy = new SelectList(db.AspNetUsers, "Id", "Email", stallRequest.CreatedBy);
            ViewBag.InstituteId = new SelectList(db.Institutes, "Id", "Institute1");
            ViewBag.RequestStatusId = new SelectList(db.RequestStatus, "Id", "Status", stallRequest.RequestStatusId);
            ViewBag.CategoryId = new SelectList(db.StallCategories, "Id", "StallType", stallRequest.CategoryId);
            return View(stallRequest);
        }

        // POST: StallRequests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,StallName,StallDetails,Logo,RequestStatusId,CategoryId,CreatedBy,CreatedAt,OwnerName,ContactNo,Email,Address,City,Profession,Institute")] StallRequest stallRequest, HttpPostedFileBase Logo)
        {
            if (ModelState.IsValid)
            {
                if (Logo != null && Logo.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(Logo.FileName);
                    var path = Path.Combine(Server.MapPath("~/UploadedImages/"), fileName);
                    Logo.SaveAs(path);
                    stallRequest.Logo = fileName;
                }
                stallRequest.CreatedAt = DateTime.Now;
                stallRequest.CreatedBy = User.Identity.GetUserId();
                db.Entry(stallRequest).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CreatedBy = new SelectList(db.AspNetUsers, "Id", "Email", stallRequest.CreatedBy);
            ViewBag.InstituteId = new SelectList(db.Institutes, "Id", "Institute1");
            ViewBag.RequestStatusId = new SelectList(db.RequestStatus, "Id", "Status", stallRequest.RequestStatusId);
            ViewBag.CategoryId = new SelectList(db.StallCategories, "Id", "StallType", stallRequest.CategoryId);
            return View(stallRequest);
        }

        // GET: StallRequests/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StallRequest stallRequest = db.StallRequests.Find(id);
            if (stallRequest == null)
            {
                return HttpNotFound();
            }
            return View(stallRequest);
        }

        // POST: StallRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StallRequest stallRequest = db.StallRequests.Find(id);
            db.StallRequests.Remove(stallRequest);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult UpdateStatus(int? id, int? Status)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StallRequest stallRequest = db.StallRequests.Find(id);
            if (stallRequest == null)
            {
                return HttpNotFound();
            }
            stallRequest.RequestStatusId = Status;
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
