﻿using System;
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
    public class MealsController : Controller
    {
        private RestaurantEntities db = new RestaurantEntities();

        // GET: Meals
        public ActionResult Index()
        {
            var meals = db.Meals.Include(m => m.Dish).Include(m => m.Ord);
            return View(meals.ToList());
        }

        // GET: Meals/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Meals meals = db.Meals.Find(id);
            if (meals == null)
            {
                return HttpNotFound();
            }
            return View(meals);
        }

        // GET: Meals/Create
        public ActionResult Create(int? ordId)
        {
            if (ordId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IEnumerable<Meals> MenuMeals = db.Meals.Where(u => u.OrdId == ordId).AsEnumerable();
            Meals CurrentMeal = new Meals();
            CurrentMeal.OrdId = ordId ?? CurrentMeal.OrdId;      
            Ord CurrentOrder = db.Ord.Find(ordId);
            ViewBag.DishId = new SelectList(db.Dish, "Id", "Name");
            ViewBag.dishes = MenuMeals;
            ViewBag.IsOrdClosed = false;
            if (CurrentOrder.TimeEnd != null)
            {
                ViewBag.IsOrdClosed = true;
            }

            return View(CurrentMeal);
        }

        // POST: Meals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,OrdId,DishId,Status")] Meals meals)
        {
            if (ModelState.IsValid)
            {
               
                Ord CurrentOrder = db.Ord.Find(meals.OrdId);
                if (CurrentOrder != null) { 
               
                ViewBag.IsOrdClosed = false;
                if ( CurrentOrder.TimeEnd != null)
                    {
                        db.Meals.Add(meals);
                        CurrentOrder.TotalCost += db.Dish.Find(meals.DishId).Price;
                        db.Entry(CurrentOrder).State = EntityState.Modified;
                        db.SaveChanges();
                        ViewBag.IsOrdClosed = true;
                    }
            }         
                //return RedirectToAction("Create", "Meals", new { ordId = meals.OrdId });
            }
            IEnumerable<Meals> MenuMeals = db.Meals.Where(u => u.OrdId == meals.OrdId).AsEnumerable();
            ViewBag.dishes = MenuMeals;
            ViewBag.DishId = new SelectList(db.Dish, "Id", "Name", meals.DishId);
            return View(meals);
        }


        // GET: Meals/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Meals meals = db.Meals.Find(id);
            if (meals == null)
            {
                return HttpNotFound();
            }
            ViewBag.DishId = new SelectList(db.Dish, "Id", "Name", meals.DishId);
            ViewBag.OrdId = new SelectList(db.Ord, "Id", "Id", meals.OrdId);
            return View(meals);
        }

        // POST: Meals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,OrdId,DishId,Status")] Meals meals)
        {
            if (ModelState.IsValid)
            {
                db.Entry(meals).State = EntityState.Modified;
                Ord curOrd = db.Ord.Find(meals.OrdId);
                db.Entry(curOrd).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "Ords", new { id = meals.OrdId });
            }

            ViewBag.DishId = new SelectList(db.Dish, "Id", "Name", meals.DishId);
            ViewBag.OrdId = new SelectList(db.Ord, "Id", "Id", meals.OrdId);
            return View(meals);
        }

        // GET: Meals/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Meals meals = db.Meals.Find(id);
            if (meals == null)
            {
                return HttpNotFound();
            }
            return View(meals);
        }

        // POST: Meals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            Meals meals = db.Meals.Find(id);
            Ord curOrd = db.Ord.Find(meals.OrdId);

            if (curOrd != null && curOrd.TimeEnd != null)
            {
                ViewBag.IsOrdClosed = false;
                curOrd.TotalCost -= db.Dish.Find(meals.DishId).Price;
                db.Meals.Remove(meals);
                db.Entry(curOrd).State = EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                ViewBag.IsOrdClosed = true;
            }
            return RedirectToAction("Details", "Ords", new { id = meals.OrdId });
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
