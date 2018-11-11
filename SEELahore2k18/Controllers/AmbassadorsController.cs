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
    public class AmbassadorsController : Controller
    {
        private SEELahoreEntities db = new SEELahoreEntities();

        // GET: Ambassadors
        public ActionResult Index(int? type = 0)
        {
            if (type != 0)
            {
                ViewBag.InstituteId = new SelectList(db.Institutes, "Id", "Institute1");
                var ambassadors = db.Ambassadors.Where(s => s.StatusId == type).Include(a => a.AmbassadorCategory).OrderByDescending(s => s.Id).Include(a => a.RequestStatu);
                return View(ambassadors.ToList());
            }
            else
            {
                ViewBag.InstituteId = new SelectList(db.Institutes, "Id", "Institute1");
                var ambassadors = db.Ambassadors.Include(a => a.AmbassadorCategory).OrderByDescending(s => s.Id).Include(a => a.RequestStatu);
                return View(ambassadors.ToList());
            }
        }


        public ActionResult UpdateStatus(int? id, int? Status)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                try
                {
                    Ambassador volunteer = db.Ambassadors.Find(id);
                    if (volunteer == null)
                    {
                        return HttpNotFound();
                    }
                    volunteer.StatusId = Status;
                    db.SaveChanges();
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

                    HomeController.EntityinfoMessage("Ambassodor status: " + message);
                    HomeController.EntitywriteErrorLog(ex);

                }
            }
            catch (Exception ex)
            {
                HomeController.infoMessage(ex.Message);
                HomeController.writeErrorLog(ex);
            }

            return RedirectToAction("Index");
        }
        // GET: Ambassadors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ambassador ambassador = db.Ambassadors.Find(id);
            if (ambassador == null)
            {
                return HttpNotFound();
            }
            return View(ambassador);
        }
        [AllowAnonymous]
        // GET: Ambassadors/Create
        public ActionResult Create()
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
            ViewBag.AmbassadorCategoryId = new SelectList(db.AmbassadorCategories, "Id", "Category");
            ViewBag.StatusId = new SelectList(db.RequestStatus, "Id", "Status");
            var InstituteId = db.Institutes.ToList();
            ViewBag.InstituteId = new SelectList(db.Institutes, "Id", "Institute1");
            return View();
        }

        // POST: Ambassadors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Create([Bind(Include = "Id,Name,ContactNo,FacebookId,EmailId,CNIC,InstituteId,StatusId,CreatedAt,Address,CityOfResidence,Degree,PreviousExperiance,AmbassadorCategoryId,Hostelite,Why,ExpectationsFromSEE")] Ambassador ambassador)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var obj = db.Ambassadors.FirstOrDefault(s => s.EmailId == ambassador.EmailId || s.ContactNo == ambassador.ContactNo);
                    if (obj != null)
                    {
                        ViewBag.ErrorMessage = "Email Or Phone No. Already Exists!";
                        ViewBag.InstituteId = new SelectList(db.Institutes, "Id", "Institute1");

                        ViewBag.AmbassadorCategoryId = new SelectList(db.AmbassadorCategories, "Id", "Category", ambassador.AmbassadorCategoryId);
                        ViewBag.StatusId = new SelectList(db.RequestStatus, "Id", "Status", ambassador.StatusId);
                        return View(ambassador);
                    }
                    try
                    {
                        ambassador.ExpectationsFromSEE = Request.Form["ExpectationsFromSEE"];
                        ambassador.CreatedAt = DateTime.Now;
                        ambassador.StatusId = 1;
                        db.Ambassadors.Add(ambassador);
                        db.SaveChanges();
                        string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                        string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                        return RedirectToAction("SubmissionResponce", "Home", new { status = "You are successfully registerd for Ambassador with your crdentials,Team SEE Lahore will soon respond you through Email.Stay Connected for Bigest Event of Lahore, See Lahore 2018", url = controllerName + "/" + actionName });

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

                        HomeController.EntityinfoMessage(ambassador.Name + ": " + message);
                        HomeController.EntitywriteErrorLog(ex);
                        string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                        string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                        return RedirectToAction("SubmissionResponce", "Home", new { status = "Due to Server overload something went wrong! please try again. Sorry for Inconvenience", url = controllerName + "/" + actionName });

                    }
                }


                ViewBag.InstituteId = new SelectList(db.Institutes, "Id", "Institute1");

                ViewBag.AmbassadorCategoryId = new SelectList(db.AmbassadorCategories, "Id", "Category", ambassador.AmbassadorCategoryId);
                ViewBag.StatusId = new SelectList(db.RequestStatus, "Id", "Status", ambassador.StatusId);
                return View(ambassador);
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

        
        // GET: Ambassadors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ambassador ambassador = db.Ambassadors.Find(id);
            if (ambassador == null)
            {
                return HttpNotFound();
            }
            ViewBag.AmbassadorCategoryId = new SelectList(db.AmbassadorCategories, "Id", "Category", ambassador.AmbassadorCategoryId);
            ViewBag.InstituteId = new SelectList(db.Institutes, "Id", "Institute1");
            ViewBag.StatusId = new SelectList(db.RequestStatus, "Id", "Status", ambassador.StatusId);
            return View(ambassador);
        }

        // POST: Ambassadors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,ContactNo,FacebookId,EmailId,CNIC,InstituteId,StatusId,CreatedAt,Address,CityOfResidence,Degree,PreviousExperiance,AmbassadorCategoryId,Hostelite")] Ambassador ambassador)
        {
            if (ModelState.IsValid)
            {
                ambassador.CreatedAt = DateTime.Now;
                db.Entry(ambassador).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AmbassadorCategoryId = new SelectList(db.AmbassadorCategories, "Id", "Category", ambassador.AmbassadorCategoryId);
            ViewBag.InstituteId = new SelectList(db.Institutes, "Id", "Institute1");
            ViewBag.StatusId = new SelectList(db.RequestStatus, "Id", "Status", ambassador.StatusId);
            return View(ambassador);
        }

        // GET: Ambassadors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ambassador ambassador = db.Ambassadors.Find(id);
            if (ambassador == null)
            {
                return HttpNotFound();
            }
            return View(ambassador);
        }

        // POST: Ambassadors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ambassador ambassador = db.Ambassadors.Find(id);
            db.Ambassadors.Remove(ambassador);
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
