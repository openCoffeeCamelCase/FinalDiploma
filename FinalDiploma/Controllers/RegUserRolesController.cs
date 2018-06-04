using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FinalDiploma.Models;

namespace FinalDiploma.Controllers
{
    public class RegUserRolesController : Controller
    {
        private RestaurantEntities db = new RestaurantEntities();

        // GET: RegUserRoles
        public ActionResult Index()
        {
            return View(db.RegUserRole.ToList());
        }

        // GET: RegUserRoles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RegUserRole regUserRole = db.RegUserRole.Find(id);
            if (regUserRole == null)
            {
                return HttpNotFound();
            }
            return View(regUserRole);
        }

        // GET: RegUserRoles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RegUserRoles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] RegUserRole regUserRole)
        {
            if (ModelState.IsValid)
            {
                db.RegUserRole.Add(regUserRole);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(regUserRole);
        }

        // GET: RegUserRoles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RegUserRole regUserRole = db.RegUserRole.Find(id);
            if (regUserRole == null)
            {
                return HttpNotFound();
            }
            return View(regUserRole);
        }

        // POST: RegUserRoles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] RegUserRole regUserRole)
        {
            if (ModelState.IsValid)
            {
                db.Entry(regUserRole).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(regUserRole);
        }

        // GET: RegUserRoles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RegUserRole regUserRole = db.RegUserRole.Find(id);
            if (regUserRole == null)
            {
                return HttpNotFound();
            }
            return View(regUserRole);
        }

        // POST: RegUserRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RegUserRole regUserRole = db.RegUserRole.Find(id);
            db.RegUserRole.Remove(regUserRole);
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
