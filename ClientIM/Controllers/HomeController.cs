using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using TotpAuth;

namespace ClientIM.Controllers
{
    public class HomeController : Controller
    {
        Models.ClientEntities db = new Models.ClientEntities();
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(FormCollection collection)
        {
            Session["friend_request"] = 0;
            Session["new_message"] = 0;
            Session["Address_person_id"] = 0;
            string username = collection["username"];
            Models.User theUser = db.Users.SingleOrDefault(u => u.username.Equals(username));
            if (theUser != null && 
                Crypto.VerifyHashedPassword(theUser.password_hash, collection["password_hash"]))
            {
                if (theUser.secret != null)
                {
                    Totp totp = new Totp(theUser.secret);
                    string theCode = totp.AuthenticationCode;
                    if (theCode.Equals(collection["validation"]))
                    {
                        Session["user_id"] = theUser.user_id;
                        if (theUser.person_id != null)
                        {
                            Session["person_id"] = theUser.person_id;
                            return RedirectToAction("Index", "Profile");
                        }
                        else
                        {
                            return RedirectToAction("Create", "Profile");
                        }
                    }
                }
                else
                {
                    Session["user_id"] = theUser.user_id;
                    if (theUser.person_id != null)
                    {
                        Session["person_id"] = theUser.person_id;
                        return RedirectToAction("Index", "Profile");
                    }
                    else
                        return RedirectToAction("Create", "Profile");
                }
                ViewBag.error = "Wrong Username/Password/Validation combination!";
                return View();
            }
            else
            {
                ViewBag.error = "Wrong Username/Password combination!";
                return View();
            }
        }

        // GET: Home/Details/5
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index");
        }

        private static Random random = new Random();
        public static string RandomBase32String(int lenght)
        {
            const string chars = "ACBCDEFGHIJKLMNOPQRSTUVWXYZ234567";
            return new string(Enumerable.Repeat(chars, lenght)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        // GET: Home/Create
        public ActionResult Register()
        {
            //generate a string

            string secret = RandomBase32String(16);
            //otpauth://totp/Example:alice@google.com?secret=JBSWY3DPEHPK3PXP&issuer=Example
            string otpauth = "otpauth://totp/Example:alice@google.com?secret=" + secret + "&issuer=Example";

            //scret that has 16 chars A-Z,not 0, not 1, but 2-7?


            //Generate code
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(otpauth, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);
            ImageConverter converter = new ImageConverter();
            ViewBag.QRCode = (byte[])converter.ConvertTo(qrCodeImage, typeof(byte[]));

            Session["secret"] = secret;

            return View();
        }

        // POST: Home/Create
        [HttpPost]
        public ActionResult Register(FormCollection collection)
        {
            try
            {
                String secret = Session["secret"].ToString();
                Totp totp = new Totp(secret);

                string theSecret = null;

                if (totp.AuthenticationCode == collection["validation"]) {
                    theSecret = secret;
                }

                // TODO: Add insert logic here
                string username = collection["username"];
                Models.User theUser = db.Users.SingleOrDefault(u => u.username.Equals(username));

                if (theUser != null)
                    return RedirectToAction("Register");

                Models.User newUser = new Models.User()
                {
                    username = collection["username"],
                    password_hash = Crypto.HashPassword(collection["password_hash"]),
                    secret = theSecret
                };
                db.Users.Add(newUser);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
