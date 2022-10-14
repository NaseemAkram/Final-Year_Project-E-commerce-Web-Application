using GFS.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GFS.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        GFSEDB db = new GFSEDB();
       
        // GET: Admin
        public ActionResult Index()
        {
            var a   = db.Products.Count();
            var b = db.Catagories.Count();
            var c   = db.Carts.Where(x => x.Status == "deliver").Count();
            var d      = db.Carts.Where(x=>x.Status== "deliver").Sum(x => (double?)(x.Amount)) ?? 0;
            TempData["TotalProduct"] = a;
            TempData["TotalCatagory"] = b;
            TempData["TotalOrders"] = c;
            TempData["TotalSale"] = d;

            return View(db.Accounts.ToList());
        }

        public ActionResult Vendor()
        {
            return View(db.Accounts.Where(x=>x.Role=="Vendor").ToList());
        }
        public ActionResult Users()
        {
            return View(db.Accounts.Where(x=>x.Role=="User").ToList());
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Account data)
        {
            if(ModelState.IsValid)
            {
                data.Status = true;
                data.Date = DateTime.Now.Date;
                db.Accounts.Add(data);
                db.SaveChanges();
                TempData["msg"]= "User Account Created Successed";
            }
           
            return RedirectToAction("Add");
        }
        public ActionResult Table()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Register()
        {
            return View();
        }
    }
}