using GFS.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace GFS.Controllers
{
    [Authorize(Roles = "User")]
    public class UsersController : Controller
    {
        GFSEDB db = new GFSEDB();
        // GET: Users
        public ActionResult Index()
        {
            var uid = db.Accounts.Where(x => x.Email == User.Identity.Name).FirstOrDefault().UserId;
            return View(db.Carts.Where(x=>x.UserId==uid).ToList());
        }
        public ActionResult Cart(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var product = db.CartDetails.Where(x=>x.CartId==id).ToList();
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        public ActionResult Orders()
        {

            return View();
        }
    }
}