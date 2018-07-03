using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Net;
using System.Text;

namespace BAL
{
    public class Email
    {
        public bool sendMail(string reciver, string senderName, string subject, string OTP,string StationCode)
        {
            bool result = false;
            string mailBody = "";
            string link = ConfigurationManager.AppSettings["WebUrl"].ToString();
            try
            {
                mailBody = System.IO.File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath("/Views/OTPMail.html"));
                mailBody = mailBody.Replace("#firstname#", senderName);
                mailBody = mailBody.Replace("#OTPCode#", OTP);
                mailBody = mailBody.Replace("#StationCode#", StationCode);
                mailBody = mailBody.Replace("#URL#", link);
                if (mailsend(reciver, mailBody, subject))
                    result = true;
            }
            catch (Exception)
            {


            }

            return result;

        }


        public bool mailsend(string reciver, string mailbody, string subject)
        {
            bool result = false;
            try
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(reciver);
                mail.From = new MailAddress(ConfigurationManager.AppSettings["mailid"].ToString());
                mail.Subject = subject;
                mail.Body = mailbody;
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = ConfigurationManager.AppSettings["mailserver"].ToString();
                smtp.Port = Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPort"].ToString());
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["mailid"].ToString(), ConfigurationManager.AppSettings["mailpass"].ToString());
                smtp.EnableSsl = true;
                smtp.Send(mail);
                result = true;
            }
            catch (Exception ex)
            {

            }
            return result;

        }

        public bool SendSMS(String Recipient, String Message)
        {
            String AccountID = ConfigurationManager.AppSettings["AccountID"].ToString();
            String Email = ConfigurationManager.AppSettings["Email"].ToString();
            String Password = ConfigurationManager.AppSettings["Password"].ToString();
            WebClient Client = new WebClient();
            String RequestURL, RequestData;

            RequestURL = "https://redoxygen.net/sms.dll?Action=SendSMS";

            RequestData = "AccountId=" + AccountID
                + "&Email=" + System.Web.HttpUtility.UrlEncode(Email)
                + "&Password=" + System.Web.HttpUtility.UrlEncode(Password)
                + "&Recipient=" + System.Web.HttpUtility.UrlEncode(Recipient)
                + "&Message=" + System.Web.HttpUtility.UrlEncode(Message);

            byte[] PostData = Encoding.ASCII.GetBytes(RequestData);
            byte[] Response = Client.UploadData(RequestURL, PostData);

            String Result = Encoding.ASCII.GetString(Response);
            int ResultCode = System.Convert.ToInt32(Result.Substring(0, 4));

            return true;
        }
    }
}