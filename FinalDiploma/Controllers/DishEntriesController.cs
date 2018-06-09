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
    public class DishEntriesController : Controller
    {
        private RestaurantEntities db = new RestaurantEntities();

        // GET: DishEntries
        public ActionResult Index()
        {
            var dishEntry = db.DishEntry.Include(d => d.Dish).Include(d => d.Product);
            return View(dishEntry.ToList());
        }

        // GET: DishEntries/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DishEntry dishEntry = db.DishEntry.Find(id);
            if (dishEntry == null)
            {
                return HttpNotFound();
            }
            return View(dishEntry);
        }

        // GET: DishEntries/Create
        public ActionResult Create(int? dishId)
        {
            if (dishId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //ViewBag.DishId = new SelectList(db.Dish, "Id", "Name");
            IEnumerable<DishEntry> DishEntrys = db.DishEntry.Where(u => u.DishId == dishId).AsEnumerable();
            ViewBag.products = DishEntrys;
            ViewBag.ProductId = new SelectList(db.Product, "Id", "Name");
            DishEntry currentDishEntry = new DishEntry();
            currentDishEntry.Dish = DishEntrys.FirstOrDefault().Dish;
            currentDishEntry.DishId = dishId ?? currentDishEntry.DishId;
            return View(currentDishEntry);
        }


        // POST: DishEntries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,DishId,ProductId,Weight")] DishEntry dishEntry)
        {
            if (ModelState.IsValid)
            {
                db.DishEntry.Add(dishEntry);
                db.SaveChanges();
                //return RedirectToAction("Index");
            }
            IEnumerable<DishEntry> DishEntrys = db.DishEntry.Where(u => u.DishId == dishEntry.DishId).AsEnumerable();
            ViewBag.products = DishEntrys;
            ViewBag.ProductId = new SelectList(db.Product, "Id", "Name", dishEntry.ProductId);
            dishEntry.Dish = DishEntrys.FirstOrDefault().Dish;
            return View(dishEntry);
        }

        // GET: DishEntries/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DishEntry dishEntry = db.DishEntry.Find(id);
            if (dishEntry == null)
            {
                return HttpNotFound();
            }
            ViewBag.DishId = new SelectList(db.Dish, "Id", "Name", dishEntry.DishId);
            ViewBag.ProductId = new SelectList(db.Product, "Id", "Name", dishEntry.ProductId);
            return View(dishEntry);
        }

        // POST: DishEntries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DishId,ProductId,Weight")] DishEntry dishEntry)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dishEntry).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DishId = new SelectList(db.Dish, "Id", "Name", dishEntry.DishId);
            ViewBag.ProductId = new SelectList(db.Product, "Id", "Name", dishEntry.ProductId);
            return View(dishEntry);
        }

        // GET: DishEntries/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DishEntry dishEntry = db.DishEntry.Find(id);
            if (dishEntry == null)
            {
                return HttpNotFound();
            }
            return View(dishEntry);
        }

        // POST: DishEntries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DishEntry dishEntry = db.DishEntry.Find(id);
            db.DishEntry.Remove(dishEntry);
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
