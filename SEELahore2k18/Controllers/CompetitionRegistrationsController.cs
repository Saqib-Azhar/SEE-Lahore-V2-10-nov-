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
using PagedList;

namespace SEELahore2k18.Controllers
{
    [Authorize]
    public class CompetitionRegistrationsController : Controller
    {
        private SEELahoreEntities db = new SEELahoreEntities();

        // GET: CompetitionRegistrations
        public ActionResult Index(int? type = 0, int? page = 1)
        {
            int pageSize = 10;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            IPagedList<CompetitionRegistration> competitionRegistrations = null;
            if (type != 0)
            {
            ViewBag.InstituteId = new SelectList(db.Institutes, "Id", "Institute1");
                competitionRegistrations = db.CompetitionRegistrations.OrderByDescending(s => s.Id).Where(s => s.RequestStatusId == type).Include(c => c.Competition).Include(c => c.RequestStatu).ToPagedList(pageIndex, pageSize);

            }
            else
            {
            ViewBag.InstituteId = new SelectList(db.Institutes, "Id", "Institute1");
                competitionRegistrations = db.CompetitionRegistrations.OrderByDescending(s => s.Id).Include(c => c.Competition).Include(c => c.RequestStatu).ToPagedList(pageIndex, pageSize);

            }
            if (page == 1)
            {
                ViewBag.startingCounter = competitionRegistrations.Count;
            }
            else
            {
                ViewBag.startingCounter = (pageSize * pageIndex) + competitionRegistrations.Count;
            }
            return View(competitionRegistrations);
        }

        // GET: CompetitionRegistrations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompetitionRegistration competitionRegistration = db.CompetitionRegistrations.Find(id);
            if (competitionRegistration == null)
            {
                return HttpNotFound();
            }
            return View(competitionRegistration);
        }
        [AllowAnonymous]
        // GET: CompetitionRegistrations/Create
        public ActionResult Create(int? val = 0)
        {
            try
            {
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                var dateRange = db.RegistrationDeadLines.FirstOrDefault(s => s.RegistrationType == controllerName);
                var comparisonto = (DateTime.Compare(Convert.ToDateTime(DateTime.Now), Convert.ToDateTime(dateRange.To)));
                var comparisonfrom = (DateTime.Compare(Convert.ToDateTime(DateTime.Now), Convert.ToDateTime(dateRange.From)));

                ViewBag.CompetitionList = db.Competitions.ToList();
                if (comparisonto != -1)
                {
                    return RedirectToAction("RegistrationDeadline", "Home", new { status = "Registrations Ended" });
                }
                else if (comparisonfrom != 1)
                {
                    return RedirectToAction("RegistrationDeadline", "Home", new { status = "Registrations will be open soon!" });
                }
                if (val == 1)
                {
                    ViewBag.isTalentGala = 1;
                }
                else
                {
                    ViewBag.isTalentGala = 0;

                }

            }
            catch (Exception ex)
            {

                HomeController.infoMessage(ex.Message);
                HomeController.writeErrorLog(ex);

            }
           
            ViewBag.CompetitionId = new SelectList(db.Competitions, "Id", "CompetitionName");
            ViewBag.InstituteId = new SelectList(db.Institutes, "Id", "Institute1");
            ViewBag.RequestStatusId = new SelectList(db.RequestStatus, "Id", "Status");
            return View();
        }
        [AllowAnonymous]
        // POST: CompetitionRegistrations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,ContactNo,Designation,EmailId,CNIC,InstituteId,RequestStatusId,CreatedAt,Address,City,CompetitionId")] CompetitionRegistration competitionRegistration)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        if(competitionRegistration.CompetitionId == 2)
                        {
                            var teamLead = Request.Form["DramaTeamLeader"];
                            var totalMembers = Request.Form["DramaTotalTeamMembers"];
                            var dramaDescription = Request.Form["DramaDescription"];
                            var toSaveWithAddressString = " || "+ teamLead + " || " + totalMembers + " || " + dramaDescription;
                            competitionRegistration.Address = competitionRegistration.Address + " !Drama! " + toSaveWithAddressString;
                        }

                        competitionRegistration.RequestStatusId = 1;
                        competitionRegistration.CreatedAt = DateTime.Now;
                        db.CompetitionRegistrations.Add(competitionRegistration);
                        db.SaveChanges();
                        string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                        string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                        return RedirectToAction("SubmissionResponce", "Home", new { status = "You are successfully registerd for Competition with your crdentials,Team SEE Lahore will soon respond you through Email.Stay Connected for Bigest Event of Lahore, See Lahore 2018", url = controllerName + "/" + actionName });
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

                        HomeController.EntityinfoMessage(competitionRegistration.Name + ": " + message);
                        HomeController.EntitywriteErrorLog(ex);
                        string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                        string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                        return RedirectToAction("SubmissionResponce", "Home", new { status = "Due to Server overload something went wrong! please try again. Sorry for Inconvenience", url = controllerName + "/" + actionName });

                    }
                }


                ViewBag.CompetitionId = new SelectList(db.Competitions, "Id", "CompetitionName", competitionRegistration.CompetitionId);
                ViewBag.InstituteId = new SelectList(db.Institutes, "Id", "Institute1");
                ViewBag.RequestStatusId = new SelectList(db.RequestStatus, "Id", "Status", competitionRegistration.RequestStatusId);
                return View(competitionRegistration);

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

        // GET: CompetitionRegistrations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompetitionRegistration competitionRegistration = db.CompetitionRegistrations.Find(id);
            if (competitionRegistration == null)
            {
                return HttpNotFound();
            }
            ViewBag.InstituteId = new SelectList(db.Institutes, "Id", "Institute1");
            ViewBag.CompetitionId = new SelectList(db.Competitions, "Id", "CompetitionName", competitionRegistration.CompetitionId);
            ViewBag.RequestStatusId = new SelectList(db.RequestStatus, "Id", "Status", competitionRegistration.RequestStatusId);
            return View(competitionRegistration);
        }

        // POST: CompetitionRegistrations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,ContactNo,Designation,EmailId,CNIC,InstituteId,RequestStatusId,CreatedAt,Address,City,CompetitionId")] CompetitionRegistration competitionRegistration)
        {
            if (ModelState.IsValid)
            {
                competitionRegistration.CreatedAt = DateTime.Now;
                db.Entry(competitionRegistration).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.InstituteId = new SelectList(db.Institutes, "Id", "Institute1");
            ViewBag.CompetitionId = new SelectList(db.Competitions, "Id", "CompetitionName", competitionRegistration.CompetitionId);
            ViewBag.RequestStatusId = new SelectList(db.RequestStatus, "Id", "Status", competitionRegistration.RequestStatusId);
            return View(competitionRegistration);
        }

        // GET: CompetitionRegistrations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompetitionRegistration competitionRegistration = db.CompetitionRegistrations.Find(id);
            if (competitionRegistration == null)
            {
                return HttpNotFound();
            }
            return View(competitionRegistration);
        }

        // POST: CompetitionRegistrations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CompetitionRegistration competitionRegistration = db.CompetitionRegistrations.Find(id);
            db.CompetitionRegistrations.Remove(competitionRegistration);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult UpdateStatus(int? id, int? Status)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompetitionRegistration competitionRegistration = db.CompetitionRegistrations.Find(id);
            if (competitionRegistration == null)
            {
                return HttpNotFound();
            }
            competitionRegistration.RequestStatusId = Status;
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
