using GFS.Models.DB;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace GFS.Controllers
{
    
    public class ShoppingController : Controller
    {
        GFSEDB db = new GFSEDB();
        // GET: Shopping
        public ActionResult Order(int OPID)
        {
            var CUser = db.Accounts.Where(x => x.Email == User.Identity.Name).FirstOrDefault();
            if(CUser!=null && (CUser.Role=="User" ||CUser.Role== "Gest"))
            {
                var cart = db.Carts.Where(x => x.UserId == CUser.UserId && x.Status == "Pending").FirstOrDefault();
                double productamount = db.Products.Where(x => x.PId == OPID).FirstOrDefault().Price;
                CartDetail cd = new CartDetail();
                if(cart!=null)
                {
                    cd.CartId = cart.CartId;
                    cd.PId = OPID;
                    db.CartDetails.Add(cd);
                    db.SaveChanges();

                    Cart p = db.Carts.Where(x=>x.UserId== cart.UserId).FirstOrDefault();
                    p.Amount += productamount;
                    db.Entry(p).State = EntityState.Modified;
                    db.SaveChanges();


                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    Cart c = new Cart();
                    c.UserId = CUser.UserId;
                    c.ShippingAddress = "0";
                    c.Phone = "0";
                    c.Status = "Pending";
                    c.Amount = productamount;
                    c.PaymentStatus = "0";
                    c.Date = DateTime.Now.Date;
                    db.Carts.Add(c);
                    db.SaveChanges();


                    cd.CartId = c.CartId;
                    cd.PId = OPID;
                    db.CartDetails.Add(cd);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Home");

                }
            }
            else
            {
                Account ac = new Account();
                var guid = Guid.NewGuid();
                ac.FullName = "Gest";
                ac.Email = guid + "@gmail.com";
                ac.Password = "Gest";
                ac.Address = "Gest";
                ac.Phone = "Gest";
                ac.Role = "Gest";
                ac.Date = DateTime.Now.Date;
                db.Accounts.Add(ac);
                db.SaveChanges();

                double productamount = db.Products.Where(x => x.PId == OPID).FirstOrDefault().Price;
                CartDetail cd = new CartDetail();
                Cart c = new Cart();
                c.UserId = ac.UserId;
                c.ShippingAddress = "0";
                c.Phone = "0";
                c.Status = "Pending";
                c.Amount = productamount;
                c.PaymentStatus = "0";
                c.Date = DateTime.Now.Date;
                db.Carts.Add(c);
                db.SaveChanges();


                cd.CartId = c.CartId;
                cd.PId = OPID;
                db.CartDetails.Add(cd);
                db.SaveChanges();

                FormsAuthentication.SetAuthCookie(ac.Email, false);
                
                return RedirectToAction("Index", "Home");
            }
        }

       public ActionResult Checkout()
        {
            var usr = db.Accounts.Where(x => x.Email == User.Identity.Name).FirstOrDefault();
            var chicklist = db.Carts.Where(x => x.UserId == usr.UserId && x.Status=="Pending").FirstOrDefault();
            return View(chicklist);
        }

        [HttpPost]
        public ActionResult Checkout(Cart cart)
        {
            
            cart.Status = "Submited";
            cart.PaymentStatus = "Cash on delivery";

            db.Entry(cart).State = EntityState.Modified;
            db.SaveChanges();

            FormsAuthentication.SignOut();

            return RedirectToAction("thanks");
        }
        public ActionResult Thanks()
        {
            return View();
        }
    }
}