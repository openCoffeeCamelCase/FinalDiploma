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
    public class RegUsersController : Controller
    {
        private RestaurantEntities db = new RestaurantEntities();

        // GET: RegUsers
        public ActionResult Index()
        {
            var regUser = db.RegUser.Include(r => r.RegUserRole);
            return View(regUser.ToList());
        }

        // GET: RegUsers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RegUser regUser = db.RegUser.Find(id);
            if (regUser == null)
            {
                return HttpNotFound();
            }
            return View(regUser);
        }

        // GET: RegUsers/Create
        public ActionResult Create()
        {
            ViewBag.RegUserRoleId = new SelectList(db.RegUserRole, "Id", "Name");
            return View();
        }

        // POST: RegUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Login,Password,Email,Phone,RegUserRoleId")] RegUser regUser)
        {
            if (ModelState.IsValid)
            {
                db.RegUser.Add(regUser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.RegUserRoleId = new SelectList(db.RegUserRole, "Id", "Name", regUser.RegUserRoleId);
            return View(regUser);
        }

        // GET: RegUsers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RegUser regUser = db.RegUser.Find(id);
            if (regUser == null)
            {
                return HttpNotFound();
            }
            ViewBag.RegUserRoleId = new SelectList(db.RegUserRole, "Id", "Name", regUser.RegUserRoleId);
            return View(regUser);
        }

        // POST: RegUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Login,Password,Email,Phone,RegUserRoleId")] RegUser regUser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(regUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RegUserRoleId = new SelectList(db.RegUserRole, "Id", "Name", regUser.RegUserRoleId);
            return View(regUser);
        }

        // GET: RegUsers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RegUser regUser = db.RegUser.Find(id);
            if (regUser == null)
            {
                return HttpNotFound();
            }
            return View(regUser);
        }

        // POST: RegUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RegUser regUser = db.RegUser.Find(id);
            db.RegUser.Remove(regUser);
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
