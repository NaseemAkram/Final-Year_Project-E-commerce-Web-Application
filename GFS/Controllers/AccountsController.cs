using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GFS.Models.DB;
using System.Web.Security;

namespace GFS.Controllers
{
    public class AccountsController : Controller
    {
        private GFSEDB db = new GFSEDB();


        //Login Action 



        public ActionResult Login()
        {
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Login(Account data)
        {
            var usr = db.Accounts.Where(x => x.Email == data.Email && x.Password == data.Password).FirstOrDefault();
            if(usr!=null)
            {
                FormsAuthentication.SetAuthCookie(usr.Email, false);

                if(usr.Role=="Admin")
                {
                    return RedirectToAction("Index","Admin");
                }
                else if (usr.Role == "Vendor")
                {
                    return RedirectToAction("Index", "Vendor");
                }
                else if(usr.Role=="User")
                {
                    return RedirectToAction("Shop", "Home");
                }
                else
                {
                    TempData["msg"] = "You are not Authorized member";
                }

            }
            else
            {
                TempData["msg"] = "Invalid Username or Password please try again ... !";
            }


            return View();
        }



        //Register 
        public ActionResult Vendor()
        {
            return View();
        }
        public ActionResult Register()
        {
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Register(Account data)
        {
            data.Date = DateTime.Now.Date;
            data.Status = false;
            db.Accounts.Add(data);
            db.SaveChanges();

            TempData["msg"] = "Account Register Successed..!";

            return RedirectToAction("Login");
        }
        public ActionResult Forgot()
        {
            return View();
        }
        public ActionResult Conform(string Email)
        {
            Account data = db.Accounts.Where(x => x.Email == Email).FirstOrDefault();
            
            if (data != null)
            {
                return View(data);
            }
            else
            {
                TempData["msg"] = "Invalid Email Address";
                return RedirectToAction("Login");
            }
            
        }
        [HttpPost]
        public ActionResult Conform(Account account)
        {
            Account data = db.Accounts.Find(account.UserId);
            if (data != null)
            {
                data.Password = account.Password;
                db.Entry(data).State = EntityState.Modified;
                db.SaveChanges();
                TempData["msg"] = "Account Password Changed  Successed..!";
            }
            else
            {
                TempData["msg"] = "Invalid Email Address";
            }
            return RedirectToAction("Login");

        }
        

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
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
