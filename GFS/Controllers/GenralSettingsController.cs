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
    public class GenralSettingsController : Controller
    {
        private GFSEDB db = new GFSEDB();

        // GET: GenralSettings
        public ActionResult Index()
        {
            return View(db.GenralSettings.ToList());
        }

        // GET: GenralSettings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GenralSetting genralSetting = db.GenralSettings.Find(id);
            if (genralSetting == null)
            {
                return HttpNotFound();
            }
            return View(genralSetting);
        }

        // GET: GenralSettings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GenralSetting genralSetting = db.GenralSettings.Find(id);
            if (genralSetting == null)
            {
                return HttpNotFound();
            }
            return View(genralSetting);
        }

        // POST: GenralSettings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "GSId,Address,ContactNo,Email,Fee,Facebook,Indeed,Linkedin,Youtube,Pintrest")] GenralSetting genralSetting)
        {
            if (ModelState.IsValid)
            {
                db.Entry(genralSetting).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(genralSetting);
        }

        // GET: GenralSettings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GenralSetting genralSetting = db.GenralSettings.Find(id);
            if (genralSetting == null)
            {
                return HttpNotFound();
            }
            return View(genralSetting);
        }

        // POST: GenralSettings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GenralSetting genralSetting = db.GenralSettings.Find(id);
            db.GenralSettings.Remove(genralSetting);
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
