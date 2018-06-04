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
    public class CustomMenuMealsController : Controller
    {
        private RestaurantEntities db = new RestaurantEntities();

        // GET: CustomMenuMeals
        public ActionResult Index()
        {
            var customMenuMeals = db.CustomMenuMeals.Include(c => c.CustomMenu).Include(c => c.Dish);
            return View(customMenuMeals.ToList());
        }

        // GET: CustomMenuMeals/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomMenuMeals customMenuMeals = db.CustomMenuMeals.Find(id);
            if (customMenuMeals == null)
            {
                return HttpNotFound();
            }
            return View(customMenuMeals);
        }

        // GET: CustomMenuMeals/Create
        public ActionResult Create(int? customMenuId)
        {
            if (customMenuId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IEnumerable<CustomMenuMeals> MenuMeals = db.CustomMenuMeals.Where(u => u.CustomMenuId == customMenuId).AsEnumerable();
            ViewBag.dishes = MenuMeals;
            CustomMenuMeals currentCustomMenuMeals = new CustomMenuMeals();
            currentCustomMenuMeals.CustomMenuId = customMenuId ?? currentCustomMenuMeals.CustomMenuId;
            ViewBag.DishId = new SelectList(db.Dish, "Id", "Name");
            ViewBag.MenuName = db.CustomMenu.Find(customMenuId).Name;
            return View(currentCustomMenuMeals);
        }

        // POST: CustomMenuMeals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CustomMenuId,DishId")] CustomMenuMeals customMenuMeals)
        {
            if (ModelState.IsValid)
            {
                db.CustomMenuMeals.Add(customMenuMeals);
                db.SaveChanges();

            }
            IEnumerable<CustomMenuMeals> MenuMeals = db.CustomMenuMeals.Where(u => u.CustomMenuId == customMenuMeals.CustomMenuId).AsEnumerable();
            ViewBag.dishes = MenuMeals;
            ViewBag.MenuName = db.CustomMenu.Find(customMenuMeals.CustomMenuId).Name;
            ViewBag.DishId = new SelectList(db.Dish, "Id", "Name", customMenuMeals.DishId);
            return View(customMenuMeals);
        }

        // GET: CustomMenuMeals/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomMenuMeals customMenuMeals = db.CustomMenuMeals.Find(id);
            if (customMenuMeals == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomMenuId = new SelectList(db.CustomMenu, "Id", "Name", customMenuMeals.CustomMenuId);
            ViewBag.DishId = new SelectList(db.Dish, "Id", "Name", customMenuMeals.DishId);
            return View(customMenuMeals);
        }

        // POST: CustomMenuMeals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CustomMenuId,DishId")] CustomMenuMeals customMenuMeals)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customMenuMeals).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomMenuId = new SelectList(db.CustomMenu, "Id", "Name", customMenuMeals.CustomMenuId);
            ViewBag.DishId = new SelectList(db.Dish, "Id", "Name", customMenuMeals.DishId);
            return View(customMenuMeals);
        }

        // GET: CustomMenuMeals/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomMenuMeals customMenuMeals = db.CustomMenuMeals.Find(id);
            if (customMenuMeals == null)
            {
                return HttpNotFound();
            }
            return View(customMenuMeals);
        }

        // POST: CustomMenuMeals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CustomMenuMeals customMenuMeals = db.CustomMenuMeals.Find(id);
            db.CustomMenuMeals.Remove(customMenuMeals);
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
