using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace SEELahore2k18.Controllers
{
    public class SMSController : Controller
    {
        private static string SMSApiUrl = WebConfigurationManager.AppSettings["SMSApiUrl"];
        private static string SMSSenderNumber = WebConfigurationManager.AppSettings["SMSSenderNumber"];
        private static string SMSSenderPassowrd = WebConfigurationManager.AppSettings["SMSSenderPassowrd"];
        private static string SMSLanguage = WebConfigurationManager.AppSettings["SMSLanguage"];
        private static string SMSType = WebConfigurationManager.AppSettings["SMSType"];
        private static string SMSMask = WebConfigurationManager.AppSettings["SMSMask"];


        [HttpPost]
        public JsonResult SendBulkSMS(string SMSReceiverNumbers, string Subject, string Message)
        {
            List<string> Results = new List<string>();
            try
            {
                var SMSReceivers = SMSReceiverNumbers.Split(',');
                foreach (var item in SMSReceivers)
                {
                    if (item == "" || item == " " || item == null)
                        continue;
                    var result = SendSMS(item, Subject, Message);
                    Results.Add(result.Data.ToString() + "\n\n");
                }
            }
            catch (Exception ex)
            {

                HomeController.infoMessage(ex.Message);
                HomeController.writeErrorLog(ex);

            }
            return Json(Results, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SendSMS(string SMSReceiverNumber, string Subject, string Message)
        {

            try
            {
                SMSReceiverNumber = System.Text.RegularExpressions.Regex.Replace(SMSReceiverNumber, "[^\\w\\._]", "");
                if(SMSReceiverNumber.StartsWith("0"))
                {
                    SMSReceiverNumber = "92" + SMSReceiverNumber.Substring(1, SMSReceiverNumber.Length - 1);
                }
                else if (SMSReceiverNumber.StartsWith("+"))
                {
                    SMSReceiverNumber = SMSReceiverNumber.Substring(1, SMSReceiverNumber.Length - 1);
                }
                string url = SMSApiUrl;
                String result = "";
                String message = HttpUtility.UrlEncode(Subject + ":\n" + Message);
                String strPost = "id=" + SMSSenderNumber + "&pass=" + SMSSenderPassowrd + "&msg=" + message +
                "&to=" + SMSReceiverNumber + "&mask=" + SMSMask + "&type=" + SMSType + "&lang=" + SMSLanguage;
                StreamWriter myWriter = null;
                HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(url);
                objRequest.Method = "POST";
                objRequest.ContentLength = Encoding.UTF8.GetByteCount(strPost);
                objRequest.ContentType = "application/x-www-form-urlencoded";
                try
                {
                    myWriter = new StreamWriter(objRequest.GetRequestStream());
                    myWriter.Write(strPost);
                }
                catch (Exception e)
                {
                    return Json(e.Message, JsonRequestBehavior.AllowGet);
                }
                finally
                {
                    myWriter.Close();
                }
                HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();
                using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    // Close and clean up the StreamReader
                    sr.Close();
                    dynamic json = System.Web.Helpers.Json.Decode(result);
                    var a = json.corpsms;
                    var b = a[0];
                    var c = b.response;
                    var d = Convert.ToString(c);
                    return Json(SMSReceiverNumber + ": " + d, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {

                HomeController.infoMessage(ex.Message);
                HomeController.writeErrorLog(ex);
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
    }
}