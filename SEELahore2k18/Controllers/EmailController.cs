using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SEELahore2k18.Models;
using System.Web.Configuration;
using System.Net.Mail;
using System.Net;

namespace SEELahore2k18.Controllers
{
    public class EmailController : Controller
    {

        private static string SenderEmailId = WebConfigurationManager.AppSettings["DefaultEmailId"];
        private static string SenderEmailPassword = WebConfigurationManager.AppSettings["DefaultEmailPassword"];
        private static int SenderEmailPort = Convert.ToInt32(WebConfigurationManager.AppSettings["DefaultEmailPort"]);
        private static string SenderEmailHost = WebConfigurationManager.AppSettings["DefaultEmailHost"];
        //private static string SenderEmailSSL = WebConfigurationManager.AppSettings["DefaultEmailSSL"];

        private SEELahoreEntities db = new SEELahoreEntities();


        public void SubmitEmailForm(FormCollection fc)
        {
            var EmailSubject = fc["EmailSubject"];
            var EmailBody = fc["EmailBody"];
            var EmailTo = fc["EmailTo"];
            var EmailName = fc["EmailName"];
            SendEmail(EmailSubject, EmailBody, EmailTo, EmailName);
        }


        [HttpPost]
        public JsonResult SendEmail(string EmailSubject, string EmailBody, string EmailTo, string EmailName)
        {

            try
            {                
                System.Net.Mail.MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(SenderEmailHost);
                mail.From = new MailAddress(SenderEmailId,"SEE Lahore 2k18");
                mail.To.Add(EmailTo);
                mail.Subject = EmailSubject;
                mail.Body = EmailBody;
                mail.IsBodyHtml = true;
                SmtpServer.Port = SenderEmailPort;
                SmtpServer.Credentials = new System.Net.NetworkCredential(SenderEmailId, SenderEmailPassword);
                SmtpServer.EnableSsl = false;

                SmtpServer.Send(mail);



                return Json("Email sent successfuly!", JsonRequestBehavior.AllowGet);


            }
            catch (Exception ex)
            {

                HomeController.infoMessage(ex.Message);
                HomeController.writeErrorLog(ex);
                return Json("Something went wrong! Please try again", JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public JsonResult SendBulkEmail(string EmailSubject, string EmailBody, string EmailTo)
        {

            try
            {
                var emails = EmailTo.Split(',');
                List<string> EmailResults = new List<string>();
                foreach (var item in emails)
                {
                    try
                    {

                        System.Net.Mail.MailMessage mail = new MailMessage();
                        SmtpClient SmtpServer = new SmtpClient(SenderEmailHost);
                        mail.From = new MailAddress(SenderEmailId, "SEE Lahore 2k18");
                        mail.To.Add(item);
                        mail.Subject = EmailSubject;
                        mail.Body = EmailBody;
                        mail.IsBodyHtml = true;
                        SmtpServer.Port = SenderEmailPort;
                        SmtpServer.Credentials = new System.Net.NetworkCredential(SenderEmailId, SenderEmailPassword);
                        SmtpServer.EnableSsl = false;

                        SmtpServer.Send(mail);

                        var emailResult = "\n\nEmail Sent Successfuly to:" + item;
                        EmailResults.Add(emailResult);
                    }
                    catch (Exception ex)
                    {
                        var emailResult = "\n\nSomething went wrong while sending email to:" + item;
                        EmailResults.Add(emailResult);
                    }

                }



                return Json(EmailResults, JsonRequestBehavior.AllowGet);


            }
            catch (Exception ex)
            {

                HomeController.infoMessage(ex.Message);
                HomeController.writeErrorLog(ex);
                return Json("Something went wrong! Please try again", JsonRequestBehavior.AllowGet);
            }
        }


        //[HttpPost]
        public ActionResult CheckEmail(int? a=0)
        {

            try
            {
                var body = string.Format("test");
                var _email = "alert@seelahore.com";
                System.Net.Mail.MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("mail.seelahore.com");
                mail.From = new MailAddress(_email);
                mail.To.Add(_email);
                mail.Subject = "testing email see";
                mail.Body = body;
                mail.IsBodyHtml = true;
                SmtpServer.Port = 25;
                SmtpServer.Credentials = new System.Net.NetworkCredential(_email, "Seelahore@123");
                SmtpServer.EnableSsl = false;

                SmtpServer.Send(mail);
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                HomeController.infoMessage(ex.Message);
                HomeController.writeErrorLog(ex);

            }
            return Json(false, JsonRequestBehavior.AllowGet);

        }

    }
}