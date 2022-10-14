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
    public class CatagoriesController : Controller
    {
        private GFSEDB db = new GFSEDB();

        // GET: Catagories
        public ActionResult Index()
        {
            return View(db.Catagories.ToList());
        }
       
        // GET: Catagories/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Catagory catagory, HttpPostedFileBase Picture)
        {
            if (ModelState.IsValid)
            {
                if(Picture.ContentLength>0)
                {
                    string F1 = Path.GetFileName(Picture.FileName);
                    string P1 = Path.Combine(Server.MapPath("~/assets/images"), F1);
                    Picture.SaveAs(P1);
                    catagory.Picture = F1;
                    

                }
                db.Catagories.Add(catagory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(catagory);
        }

        // GET: Catagories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Catagory catagory = db.Catagories.Find(id);
            if (catagory == null)
            {
                return HttpNotFound();
            }
            return View(catagory);
        }

        // POST: Catagories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CId,Title,Picture")] Catagory catagory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(catagory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(catagory);
        }

        // GET: Catagories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Catagory catagory = db.Catagories.Find(id);
            if (catagory == null)
            {
                return HttpNotFound();
            }
            return View(catagory);
        }

        // POST: Catagories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Catagory catagory = db.Catagories.Find(id);

            var prd = db.Products.Where(x => x.CId == id).FirstOrDefault();
            if(prd!=null)
            {
                TempData["ordMsg"] = "You are Not Allow to Delete this Catagory.!";
                return RedirectToAction("Index");
            }
            else
            {
                db.Catagories.Remove(catagory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
           
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
