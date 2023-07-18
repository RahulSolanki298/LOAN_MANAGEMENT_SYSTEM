using System;
using System.Collections.Specialized;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.SessionState;
using website.Dto;
using website.Interface;

namespace website.Controllers
{
    [SessionState(SessionStateBehavior.Default)]
    public class AccountController : Controller
    {


        public readonly IAccountRepository _accountRepository;

        public AccountController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(LoginDTO login)
        {
            if (ModelState.IsValid)
            {
                var response = _accountRepository.LoginProcess(login);
                if (response.Id > 0)
                {
                    FormsAuthentication.SetAuthCookie(response.FirstName + " " + response.LastName, false);

                    Session["UserId"] = response.Id;
                    Session["NameOfAdmin"] = response.FirstName + " " + response.LastName;
                    Session["UserName"] = response.UserName;
                    Session["RoleName"] = response.RoleName;
                    Session["BranchId"] = response.BranchId;
                    Session["BranchName"] = response.BranchName;

                    if (response.RoleName != "Admin" && response.RoleName != "BranchAdmin")
                    {
                        return RedirectToAction("Index", "Home");
                    }

                    Random random = new Random();
                    int Otp = random.Next(1000, 9999);
                    Session["OTP"] = Otp;

                    //var resp=sendMail(response.EmailId, Otp);

                    //string destinationAddr = "91" + response.MobileNo;
                    //string message = "Your OTP Number Is :" + Otp;

                    //string verify = sendSMS(response.MobileNo, Otp);
                    //var verifyEmail=sendMail("rahulsolanki259@gmail.com", "rahulsolanki11100079@gmail.com", Otp);
                    return RedirectToAction("Verify", "Account");
                }
                return View();
            }

            ModelState.AddModelError("FAIL", "Invalid username and password");
            ViewBag.Error = "Please enter username and password.";
            return View();
        }

        public ActionResult Verify()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Verify(string otp)
        {
            try
            {
                if (Session["OTP"] != null && Session["NameOfAdmin"] != null)
                {
                    var otpNumber = Convert.ToInt32(Session["OTP"]);
                    var NameOfAdmin = Session["NameOfAdmin"];

                    if (otpNumber == Convert.ToInt32(otp))
                    {
                        FormsAuthentication.SetAuthCookie(NameOfAdmin.ToString(), false);
                        return RedirectToAction("Index", "Home");

                    }
                    ViewBag.Message = "Wrong otp. Please try agaign.";
                    return View();
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {

                return RedirectToAction("Index");
            }
        }

        private string sendSMS(string number, int otp)
        {
            var textMessage = "Your OTP Number Is :" + otp;
            string message = HttpUtility.UrlEncode(textMessage);
            using (var wb = new WebClient())
            {
                byte[] response = wb.UploadValues("https://api.textlocal.in/send/", new NameValueCollection()
                {
                {"apikey" , "MzM0NTU3NzI2MzQ2NmI2MzRlNzE2OTdhNjM1MzczNjQ="},
                {"numbers" , "91"+number},
                {"message" , message},
                {"sender" , "TXTLCL"}
                });
                string result = System.Text.Encoding.UTF8.GetString(response);
                return result;
            }
        }

        private string sendMail(string to, int otp)
        {
            string from = "singhmutualnidhilimited@gmail.com";
            using (MailMessage mail = new MailMessage())
            {
                string Body = $"<h1>OTP:{otp}</h1>";
                mail.From = new MailAddress(from);
                mail.To.Add(to);
                mail.Subject = "OTP Verification";
                mail.Body = Body;
                mail.IsBodyHtml = true;
                //mail.Attachments.Add(new Attachment("C:\\file.zip"));

                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.Credentials = new NetworkCredential(from, "@Ruhi5151");
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }

                return "OTP Send on your email Id.";
            }
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return View();
        }

    }
}