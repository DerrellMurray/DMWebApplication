using DMWebApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace DMWebApplication.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult IssueTracker()
        {
            return View();
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View(new MailModels());
        }


        [HttpPost]
        public ActionResult Contact(MailModels e)
        {
            if (ModelState.IsValid)
            {

                //prepare email
                var toAddress = "dmurray561az@yahoo.com";
                var fromAddress = e.Email.ToString();
                var subject = "Test enquiry from " + e.Name;
                var message = new StringBuilder();
                message.Append("Message from Derrell.com Contact Us page  \n");
                message.Append("Name: " + e.Name + "\n");
                message.Append("Email: " + e.Email + "\n");
                message.Append("Telephone: " + e.Telephone + "\n\n");
                message.Append(e.Message);

                //start email Thread
                var tEmail = new Thread(() =>
               SendEmail(toAddress, fromAddress, subject, message));
                tEmail.Start();
               // ViewBag.Message = "Thank you for contacting me.  I will get back to you as soon as possible.";
                return View("ThankYou");
            }
            return View(e);
        }

        public ViewResult ThankYou()
        {
            ViewBag.Thanks = "Thank you for contacting me.  I will get back to you as soon as possible.";

            return View();
        }


        ///
        public void SendEmail(string toAddress, string fromAddress, string subject, StringBuilder message)
        {
            try
            {
                using (var mail = new MailMessage())
                {
                    const string email = "dmurray561az@yahoo.com";
                    const string password = "Takiyah561az";

                    var loginInfo = new NetworkCredential(email, password);


                    mail.From = new MailAddress(fromAddress);
                    mail.To.Add(new MailAddress(toAddress));
                    mail.Subject = subject;
                    mail.Body = message.ToString();
                    mail.IsBodyHtml = true;

                    try
                    {
                        using (var smtpClient = new SmtpClient("smtp.mail.yahoo.com", 465))
                        {
                            smtpClient.EnableSsl = true;
                            smtpClient.UseDefaultCredentials = false;
                            smtpClient.Credentials = loginInfo;
                            smtpClient.Send(mail);
                        }

                    }

                    finally
                    {
                        //dispose the client
                        mail.Dispose();
                    }

                }
            }
            catch (SmtpFailedRecipientsException ex)
            {
                foreach (SmtpFailedRecipientException t in ex.InnerExceptions)
                {
                    var status = t.StatusCode;
                    if (status == SmtpStatusCode.MailboxBusy ||
                        status == SmtpStatusCode.MailboxUnavailable)
                    {
                        Response.Write("Delivery failed - retrying in 5 seconds.");
                        System.Threading.Thread.Sleep(5000);
                        //resend
                        //smtpClient.Send(message);
                    }
                    else
                    {
                        Response.Write("Failed to deliver message to " + t.FailedRecipient);
                    }
                }
            }
            catch (SmtpException Se)
            {
                // handle exception here
                Response.Write(Se.ToString());
            }

            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }

        }

        ////


    }
}