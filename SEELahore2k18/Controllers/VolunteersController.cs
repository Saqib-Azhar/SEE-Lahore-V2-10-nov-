using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SEELahore2k18.Models;
using System.Data.Entity.Validation;

namespace SEELahore2k18.Controllers
{
    [Authorize]
    public class VolunteersController : Controller
    {
        private SEELahoreEntities db = new SEELahoreEntities();

        // GET: Volunteers
        public ActionResult Index(int? type = 0)
        {
            if (type != 0)
            {
            ViewBag.InstituteId = new SelectList(db.Institutes, "Id", "Institute1");
                var volunteers = db.Volunteers.Where(s=>s.StatusId == type).Include(v => v.RequestStatu).OrderByDescending(s => s.Id).Include(v => v.VolunteerCategory);
                return View(volunteers.ToList());
            }
            else
            {
            ViewBag.InstituteId = new SelectList(db.Institutes, "Id", "Institute1");
                var volunteers = db.Volunteers.Include(v => v.RequestStatu).OrderByDescending(s => s.Id).Include(v => v.VolunteerCategory);
                return View(volunteers.ToList());
            }
        }

        // GET: Volunteers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Volunteer volunteer = db.Volunteers.Find(id);
            if (volunteer == null)
            {
                return HttpNotFound();
            }
            return View(volunteer);
        }
        [AllowAnonymous]
        // GET: Volunteers/Create
        public ActionResult Create()
        {

            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            var dateRange = db.RegistrationDeadLines.FirstOrDefault(s => s.RegistrationType == controllerName);
            var comparisonto =  (DateTime.Compare(Convert.ToDateTime(DateTime.Now), Convert.ToDateTime(dateRange.To))); 
            var comparisonfrom =  (DateTime.Compare(Convert.ToDateTime(DateTime.Now), Convert.ToDateTime(dateRange.From)));

            if (comparisonto != -1)
            {
                return RedirectToAction("RegistrationDeadline", "Home", new { status = "Registrations Ended!" });
            }
            else if (comparisonfrom != 1)
            {
                return RedirectToAction("RegistrationDeadline", "Home", new { status = "Registrations will be open soon!" });
            }
            ViewBag.StatusId = new SelectList(db.RequestStatus, "Id", "Status");
            ViewBag.InstituteId = new SelectList(db.Institutes, "Id", "Institute1");
            ViewBag.VolunteerCategoryId = new SelectList(db.VolunteerCategories, "Id", "Category");

            return View();
            
        }
        [AllowAnonymous]
        // POST: Volunteers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,ContactNo,FacebookId,EmailId,CNIC,InstituteId,StatusId,CreatedAt,Address,CityOfResidence,Degree,PreviousExperiance,VolunteerCategoryId,Hostelite,Why,ExpectationsFromSEE")] Volunteer volunteer)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var obj = db.Volunteers.FirstOrDefault(s => s.EmailId == volunteer.EmailId || s.ContactNo == volunteer.ContactNo);
                    if (obj != null)
                    {
                        ViewBag.ErrorMessage = "Email Or Phone No. Already Exists!";
                        ViewBag.StatusId = new SelectList(db.RequestStatus, "Id", "Status", volunteer.StatusId);
                        ViewBag.InstituteId = new SelectList(db.Institutes, "Id", "Institute1");
                        ViewBag.VolunteerCategoryId = new SelectList(db.VolunteerCategories, "Id", "Category", volunteer.VolunteerCategoryId);
                        return View(volunteer);
                    }
                    try
                    {
                        volunteer.ExpectationsFromSEE = Request.Form["ExpectationsFromSEE"];
                        volunteer.CreatedAt = DateTime.Now;
                        volunteer.StatusId = 1;
                        db.Volunteers.Add(volunteer);
                        db.SaveChanges();
                        string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                        string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                        return RedirectToAction("SubmissionResponce", "Home", new { status = "You are successfully registerd for volunteer with your crdentials,Team SEE Lahore will soon respond you through Email.Stay Connected for Bigest Event of Lahore,See Lahore 2018", url = controllerName + "/" + actionName });


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

                        HomeController.EntityinfoMessage(volunteer.Name + ": " + message);
                        HomeController.EntitywriteErrorLog(ex);
                        string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                        string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                        return RedirectToAction("SubmissionResponce", "Home", new { status = "Due to Server overload something went wrong! please try again. Sorry for Inconvenience", url = controllerName + "/" + actionName });

                    }
                }
                ViewBag.StatusId = new SelectList(db.RequestStatus, "Id", "Status", volunteer.StatusId);
                ViewBag.InstituteId = new SelectList(db.Institutes, "Id", "Institute1");
                ViewBag.VolunteerCategoryId = new SelectList(db.VolunteerCategories, "Id", "Category", volunteer.VolunteerCategoryId);
                return View(volunteer);
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

        // GET: Volunteers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Volunteer volunteer = db.Volunteers.Find(id);
            if (volunteer == null)
            {
                return HttpNotFound();
            }
            ViewBag.StatusId = new SelectList(db.RequestStatus, "Id", "Status", volunteer.StatusId);
            ViewBag.InstituteId = new SelectList(db.Institutes, "Id", "Institute1");
            ViewBag.VolunteerCategoryId = new SelectList(db.VolunteerCategories, "Id", "Category", volunteer.VolunteerCategoryId);
            return View(volunteer);
        }

        // POST: Volunteers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,ContactNo,FacebookId,EmailId,CNIC,InstituteId,StatusId,CreatedAt,Address,CityOfResidence,Degree,PreviousExperiance,VolunteerCategoryId,Hostelite")] Volunteer volunteer)
        {
            if (ModelState.IsValid)
            {
                volunteer.CreatedAt = DateTime.Now;
                db.Entry(volunteer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.StatusId = new SelectList(db.RequestStatus, "Id", "Status", volunteer.StatusId);
            ViewBag.InstituteId = new SelectList(db.Institutes, "Id", "Institute1");
            ViewBag.VolunteerCategoryId = new SelectList(db.VolunteerCategories, "Id", "Category", volunteer.VolunteerCategoryId);
            return View(volunteer);
        }

        // GET: Volunteers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Volunteer volunteer = db.Volunteers.Find(id);
            if (volunteer == null)
            {
                return HttpNotFound();
            }
            return View(volunteer);
        }

        // POST: Volunteers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Volunteer volunteer = db.Volunteers.Find(id);
            db.Volunteers.Remove(volunteer);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult UpdateStatus(int? id, int? Status)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Volunteer volunteer = db.Volunteers.Find(id);
            if (volunteer == null)
            {
                return HttpNotFound();
            }
            volunteer.StatusId = Status;
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
