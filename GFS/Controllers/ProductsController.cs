using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GFS.Models.DB;

namespace GFS.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private GFSEDB db = new GFSEDB();

        // GET: Products
        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.Account).Include(p => p.Catagory);
            return View(products.ToList());
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.Accounts, "UserId", "FullName");
            ViewBag.CId = new SelectList(db.Catagories, "CId", "Title");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product, HttpPostedFileBase[] File1)
        {
            //find who is uploading this file

            var a = User.Identity.Name;
            var currentuser = db.Accounts.Where(x => x.Email == a).FirstOrDefault().UserId;

            if (ModelState.IsValid)
            {
               
                if (File1.Length>0)
                {
                    string F1 = Path.GetFileName(File1[0].FileName);
                    string P1 = Path.Combine(Server.MapPath("~/assets/images"), F1);
                    File1[0].SaveAs(P1);
                    product.File1 = F1;
                }
               
                if (File1.Length>1)
                {
                    string F2 = Path.GetFileName(File1[1].FileName);
                    string P2 = Path.Combine(Server.MapPath("~/assets/images"), F2);
                    File1[1].SaveAs(P2);
                    product.File2 = F2;
                }
                else
                {
                    product.File2= Path.GetFileName(File1[0].FileName);
                }
                if (File1.Length> 2)
                {
                    string F3 = Path.GetFileName(File1[2].FileName);
                    string P3 = Path.Combine(Server.MapPath("~/assets/images"), F3);
                    File1[2].SaveAs(P3);
                    product.File3 = F3;


                }
                else
                {
                    product.File3= Path.GetFileName(File1[0].FileName);
                }
                if (File1.Length> 3)
                {
                    string F4 = Path.GetFileName(File1[3].FileName);
                    string P4 = Path.Combine(Server.MapPath("~/assets/images"), F4);
                    File1[3].SaveAs(P4);
                    product.File4= F4;


                }
                else
                {
                    product.File4 = Path.GetFileName(File1[0].FileName);
                }

                product.UserId = currentuser;
                product.Date = DateTime.Now;
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(db.Accounts, "UserId", "FullName", product.UserId);
            ViewBag.CId = new SelectList(db.Catagories, "CId", "Title", product.CId);
            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.Accounts, "UserId", "FullName", product.UserId);
            ViewBag.CId = new SelectList(db.Catagories, "CId", "Title", product.CId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product product, HttpPostedFileBase File1)
        {
            if (File1!=null)
            {
                string F1 = Path.GetFileName(File1.FileName);
                string P1 = Path.Combine(Server.MapPath("~/assets/images"), F1);
                File1.SaveAs(P1);


                product.File1 = F1;
            }
            
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.Accounts, "UserId", "FullName", product.UserId);
            ViewBag.CId = new SelectList(db.Catagories, "CId", "Title", product.CId);
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult OrdDlt(int id)
        {
            CartDetail product = db.CartDetails.Find(id);
            db.CartDetails.Remove(product);

            var price = db.Products.Where(x => x.PId == product.PId).FirstOrDefault();

            var crt = db.Carts.Where(x => x.CartId == product.CartId).FirstOrDefault();
            crt.Amount = crt.Amount - price.Price;
            db.Entry(crt).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index","Home");
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
