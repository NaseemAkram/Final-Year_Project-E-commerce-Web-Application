using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GFS.Models.DB;

namespace GFS.Controllers
{
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        private GFSEDB db = new GFSEDB();

        // GET: Order
        public ActionResult Index()
        {
            var carts = db.Carts.Include(c => c.Account).Where(x=>x.Status== "Submited");
            return View(carts.ToList());
        }
        public ActionResult CartDetails(int? id)
        {
            TempData["OId"] = id;
            var carts = db.CartDetails;
            return View(carts.Where(x=>x.CartId==id).ToList());
        }

        [HttpPost]
       [OverrideAuthentication]
        public ActionResult ChangeStatus(int OId, string OStatus)
        {
            Cart data = db.Carts.Find(OId);
            data.Status = OStatus;
            db.Entry(data).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index","Admin");
        }
        // GET: Order/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cart cart = db.Carts.Find(id);
            if (cart == null)
            {
                return HttpNotFound();
            }
            return View(cart);
        }

        // GET: Order/Create
        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.Accounts, "UserId", "FullName");
            return View();
        }

        // POST: Order/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CartId,UserId,ShippingAddress,Phone,Status,Date")] Cart cart)
        {
            if (ModelState.IsValid)
            {
                db.Carts.Add(cart);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(db.Accounts, "UserId", "FullName", cart.UserId);
            return View(cart);
        }

        // GET: Order/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cart cart = db.Carts.Find(id);
            if (cart == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.Accounts, "UserId", "FullName", cart.UserId);
            return View(cart);
        }

        // POST: Order/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CartId,UserId,ShippingAddress,Phone,Status,Date")] Cart cart)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cart).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.Accounts, "UserId", "FullName", cart.UserId);
            return View(cart);
        }

        // GET: Order/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cart cart = db.Carts.Find(id);
            if (cart == null)
            {
                return HttpNotFound();
            }
            return View(cart);
        }

        // POST: Order/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Cart cart = db.Carts.Find(id);
            db.Carts.Remove(cart);
            db.SaveChanges();
            return RedirectToAction("Index");
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
