using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using UserAuthentication.Models;

namespace UserAuthentication.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Session["Email"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public ActionResult About()
        {
            if (Session["Email"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public ActionResult Contact()
        {
            if (Session["Email"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        //reference...https://www.youtube.com/watch?v=gSJFjuWFTdA
        // Registration GET
        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }

        // Registration POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration(User user)
        {
            bool Status = false;
            string message = "";
            if (ModelState.IsValid)
            {
                #region Email already exist
                var exist = EmailExist(user.Email);
                if (exist)
                {
                    ModelState.AddModelError("EmailExists", "Email already registered");
                    return View(user);
                }
                #endregion

                #region Save to Database
                using (DatabaseEntities de = new DatabaseEntities())
                {
                    de.Users.Add(user);
                    de.SaveChanges();

                    //send email to user
                    SendEmail(user.Email);
                    message = "Registration finished " + "Please check your email " + user.Email;
                    Status = true;
                }
                #endregion
            }
            else
            {
                message = "Invalid Request";
            }

            ViewBag.Messsage = message;
            ViewBag.Status = Status;
            return View(user);
        }

        //if email already registered
        [NonAction]
        public bool EmailExist(string email)
        {
            using (DatabaseEntities de = new DatabaseEntities())
            {
                var v = de.Users.Where(a => a.Email == email).FirstOrDefault();
                return v != null;
            }
        }

        //send email to user
        [NonAction]
        public void SendEmail(string email)
        {
            var fromEmail = new MailAddress("mugurel.budara@gmail.com", "Company");
            var toEmail = new MailAddress(email);
            var fromEmailPassword = "andrei16gle";
            string subject = "New account created";

            string body = "<br/>Hi, " + email + "<br/>" + "We are happy to let you know your account has been created";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(message); //if compile error > allow less secure apps...https://myaccount.google.com/lesssecureapps
        }


        //reference...https://www.youtube.com/watch?v=qGbpfgVm-M4&t=75s&list=PLBDqPL56AB_41khT8knDVa6rwn7Jz8Ocg&index=16
        //Login GET
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        //Login POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserLogin login)
        {
            string message = "";
            using (DatabaseEntities de = new DatabaseEntities())
            {
                var v = de.Users.Where(a => a.Email == login.Email).FirstOrDefault();
                if (v != null)
                {
                    if (string.Compare(login.Password, v.Password) == 0)
                    {
                        Session["Email"] = login.Email.ToString();
                        return RedirectToAction("About", "Home");
                    }
                    else
                    {
                        message = "Invalid credentials";
                    }
                }

                else
                {
                    message = "Invalid credentials";
                }
            }
            ViewBag.Message = message;
            return View();
        }

        //Log out
        [HttpPost]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            TempData.Clear();
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Login", "Home");
        }
    }
}