using GFS.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GFS.Controllers
{
    public class HomeController : Controller
    {
        GFSEDB db = new GFSEDB();
        public ActionResult Index()
        {
            var a = db.Accounts.FirstOrDefault();
            var g = db.GenralSettings.FirstOrDefault();
            if (a==null)
            {
                Account data = new Account();
                data.FullName = "Admin";
                data.Email = "admin@gmail.com";
                data.Password = "admin";
                data.Address = "Quetta Balochistan Pakistan";
                data.Phone = "123456789";
                data.Role = "Admin";
                data.Status = true;
                data.Date = DateTime.Now.Date;
                db.Accounts.Add(data);
                db.SaveChanges();
            }
            if(g==null)
            {
                GenralSetting gdata = new GenralSetting();
                gdata.Address = "abc address";
                gdata.ContactNo = "123456789";
                gdata.Email = "info@gfs.com";
                gdata.Fee = 10;
                gdata.Facebook = "Facebook.com";
                gdata.Indeed = "Indeed.com";
                gdata.Linkedin = "Linkedin.com";
                gdata.Pintrest = "Pintrest.com";
                gdata.Youtube = "Youtube.com";
                db.GenralSettings.Add(gdata);
                db.SaveChanges();

            }
            return View(db.Catagories.ToList());
        }
        public ActionResult list(int id)
        {

            var a = db.Products.Where(x => x.PId == id).FirstOrDefault();
            return PartialView("_GetProduct", a);
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View(db.GenralSettings.FirstOrDefault());
        }
        public ActionResult Shop()
        {
            return View();
        }
        public ActionResult FAQ()
        {
            return View(db.FAQs.ToList());
        }
        public ActionResult Blog()
        {
            return View();
        }
        public class tst
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}