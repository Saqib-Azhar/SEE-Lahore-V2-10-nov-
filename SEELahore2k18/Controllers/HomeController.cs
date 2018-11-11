using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SEELahore2k18.Models;
using System.IO;
using System.Data.Entity.Validation;

namespace SEELahore2k18.Controllers
{
    public class HomeController : Controller
    {
        private SEELahoreEntities db = new SEELahoreEntities();
        private static List<SEELahoreTeam> SEELahoreTeamsList = new List<SEELahoreTeam>();
        private static List<Guest> GuestsList = new List<Guest>();
        private static List<EventSegment> EventSegmentsList = new List<EventSegment>();
        private static List<ProudPartner> ProudPartnersList = new List<ProudPartner>();
        private static List<Season> SeasonsList = new List<Season>();
        private static int? AmbassadorsCountList;
        private static int? VolunteersCountList;

        public ActionResult Index()
        {
            if (GuestsList == null || GuestsList.Count == 0)
                GuestsList = db.Guests.ToList();
            if (SEELahoreTeamsList == null || SEELahoreTeamsList.Count == 0)
                SEELahoreTeamsList = db.SEELahoreTeams.ToList();
            if (EventSegmentsList == null || EventSegmentsList.Count == 0)
                EventSegmentsList = db.EventSegments.ToList();
            if (ProudPartnersList == null || ProudPartnersList.Count == 0)
                ProudPartnersList = db.ProudPartners.ToList();
            if (SeasonsList == null || SeasonsList.Count == 0)
                SeasonsList = db.Seasons.ToList();
            if (AmbassadorsCountList == null || AmbassadorsCountList == 0)
                AmbassadorsCountList = db.Ambassadors.Where(s => s.StatusId == 2).Count();
            if (VolunteersCountList == null || VolunteersCountList == 0)
                VolunteersCountList = db.Volunteers.Where(s => s.StatusId == 2).Count();

            ViewBag.Guests = GuestsList;
            ViewBag.SEELahoreTeams = SEELahoreTeamsList;
            ViewBag.EventSegments = EventSegmentsList;
            ViewBag.ProudPartners = ProudPartnersList;
            ViewBag.AmbassadorsCount = AmbassadorsCountList;
            ViewBag.VolunteersCount = VolunteersCountList;
            ViewBag.Seasons = SeasonsList;
            ViewBag.Announcements = db.Announcements.ToList();
            return View();
        }



        [Authorize(Roles = "Admin")]
        public ActionResult AdminPanel()
        {
            return View();
        }

        public ActionResult SubmissionResponce(string status, string url)
        {
            ViewBag.Message = status;
            ViewBag.Url = url;

            return View();
        }
        
        public ActionResult NewSubmission(string url)
        {
            var a = url.Split('/');
            return RedirectToAction(a[1], a[0]);
        }

        public ActionResult RegistrationDeadline(string status)
        {
            ViewBag.Message = status;
            return View();
        }



        public static void infoMessage(string _message)
        {
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\ErrorLogs.txt", true);
                sw.WriteLine(DateTime.Now.ToString() + ": " + _message + "\n\n\n\n");
                sw.Flush();
                sw.Close();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public static void writeErrorLog(Exception ex)
        {
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\LogFile.txt", true);
                sw.WriteLine(DateTime.Now.ToString() + " | " + ex.Source.ToString().Trim() + " | " + ex.StackTrace.ToString().Trim() + " | " + ex.Message.ToString().Trim()+ "\n\n\n\n");
                sw.Flush();
                sw.Close();
            }
            catch (Exception exp)
            {

                throw exp;
            }
        }


        public static void EntityinfoMessage(string _message)
        {
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\EntityErrorLogs.txt", true);
                sw.WriteLine(DateTime.Now.ToString() + ": " + _message + "\n\n\n\n");
                sw.Flush();
                sw.Close();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public static void EntitywriteErrorLog(DbEntityValidationException ex)
        {
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\EntityLogFile.txt", true);
                sw.WriteLine(DateTime.Now.ToString() + " | " + ex.Source.ToString().Trim() + " | " + ex.StackTrace.ToString().Trim() + " | " + ex.Message.ToString().Trim() + "\n\n\n\n");
                sw.Flush();
                sw.Close();
            }
            catch (Exception exp)
            {

                throw exp;
            }
        }

    }
}