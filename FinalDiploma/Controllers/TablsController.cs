using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FinalDiploma.Models;
using System.Collections;

namespace FinalDiploma.Controllers
{
    public class TablsController : Controller
    {
        private RestaurantEntities db = new RestaurantEntities();

        // GET: Tabls
        public ActionResult Index()
        {
            return View(db.Tabl.ToList());
        }

        [HttpGet]
        public ActionResult StateOfTables(String status)
        {
            String StatusAll = "Всі";
            String StatusBusy = "Занятий";
            String StatusFree = "Вільний";
            //String status = StatusAll;
            var StatusDropDownList = new List<String>();
            StatusDropDownList.Add(StatusAll);
            StatusDropDownList.Add(StatusBusy);
            StatusDropDownList.Add(StatusFree);
            List<TablStatus> StatusTableList = new List<TablStatus>();
            var tabls = db.Tabl.ToList();
            foreach (var curTabl in tabls)
            {
                TablStatus NewCurrentStatusTable = new TablStatus();
                NewCurrentStatusTable.Tabl = curTabl;
                NewCurrentStatusTable.Status = StatusFree;
                StatusTableList.Add(NewCurrentStatusTable);
            }
            var ActiveOrds = db.Ord.Where(u => u.TimeEnd == null).ToList();
            foreach (var curOrd in ActiveOrds)
            {
                StatusTableList.Where(u => u.Tabl == curOrd.Tabl).FirstOrDefault().Status = StatusBusy;
            }
            ViewBag.CountOfAllTables = tabls.Count;
            ViewBag.CountOfFreeTables = StatusTableList.Where(u => u.Status == StatusFree).Count();
            ViewBag.CountOfBusyTables = ViewBag.CountOfAllTables - ViewBag.CountOfFreeTables;
            SelectList StatusSelectList = new SelectList(StatusDropDownList);
            ViewBag.StatusList = StatusSelectList;
            if (status != null && status != StatusAll)
            {
                return View(StatusTableList.Where(u => u.Status == status).ToList());
            }
            return View(StatusTableList);
        }


        // GET: Tabls/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tabl tabl = db.Tabl.Find(id);
            if (tabl == null)
            {
                return HttpNotFound();
            }
            return View(tabl);
        }

        // GET: Tabls/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tabls/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Number,PlaceNumber")] Tabl tabl)
        {
            if (ModelState.IsValid)
            {
                db.Tabl.Add(tabl);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tabl);
        }

        // GET: Tabls/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tabl tabl = db.Tabl.Find(id);
            if (tabl == null)
            {
                return HttpNotFound();
            }
            return View(tabl);
        }

        // POST: Tabls/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Number,PlaceNumber")] Tabl tabl)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tabl).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tabl);
        }

        // GET: Tabls/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tabl tabl = db.Tabl.Find(id);
            if (tabl == null)
            {
                return HttpNotFound();
            }
            return View(tabl);
        }

        // POST: Tabls/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tabl tabl = db.Tabl.Find(id);
            db.Tabl.Remove(tabl);
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
