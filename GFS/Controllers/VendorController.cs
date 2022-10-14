using GFS.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GFS.Controllers
{
    [Authorize(Roles = "Vendor")]
    public class VendorController : Controller
    {
        GFSEDB db = new GFSEDB();

        // GET: Vendor
        public ActionResult Index()
        {
            var UId = db.Accounts.Where(x => x.Email == User.Identity.Name).FirstOrDefault().UserId;
            TempData["TotalProduct"] = db.Products.Where(x=>x.UserId== UId).Count();
            TempData["TotalOrders"] = db.Carts.Where(x => x.Status == "deliver" && x.UserId == UId).Count();
            TempData["TotalSale"] = db.Carts.Where(x => x.Status == "deliver" && x.UserId == UId).Sum(x => (double?)(x.Amount)) ?? 0;
            return View();
        }
    }
}