﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FinalDiploma.Models;
using FinalDiploma.Utils;

namespace FinalDiploma.Controllers
{
    public class DishesController : Controller
    {
        private RestaurantEntities db = new RestaurantEntities();

        // GET: Dishes
        public ActionResult Index()
        {
            var dish = db.Dish.Include(d => d.Category);
            return View(dish.ToList());
        }

        // GET: Dishes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dish dish = db.Dish.Find(id);
            IEnumerable<DishEntry> dishentrys = db.DishEntry.Where(u => u.DishId == id).AsEnumerable();
            if (dish == null)
            {
                return HttpNotFound();
            }
            ViewBag.TotalWeight = 0;
            foreach (DishEntry wh in dishentrys)
            {
                ViewBag.TotalWeight += wh.Weight;
            }
            ViewBag.products = dishentrys;
            return View(dish);
        }

        // GET: Dishes/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Category, "Id", "Name");
            return View();
        }

        // POST: Dishes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Price,CategoryId,Preview")] Dish dish)
        {
            if (ModelState.IsValid)
            {
                db.Dish.Add(dish);
                db.SaveChanges();
                List<string> ListOfMail = new List<string>();
                string NameFrom = "Адміністрація ресторану";
                string Message = String.Format("Вітємо! Раді повідомити що у нашому меню з'явився новий пункт - {0} всього за {1}грн. ", dish.Name, dish.Price);
                string Header = "Новий пункт меню";
                ListOfMail.AddRange(db.RegUser.Where(u => u.RegUserRole.Name == "Client").Select(c => c.Email).ToList());
                Mail.MailSender(NameFrom, Message, Header, ListOfMail);
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.Category, "Id", "Name", dish.CategoryId);
            return View(dish);
        }

        // GET: Dishes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dish dish = db.Dish.Find(id);
            if (dish == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Category, "Id", "Name", dish.CategoryId);
            return View(dish);
        }

        // POST: Dishes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Price,CategoryId,Preview")] Dish dish)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dish).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Category, "Id", "Name", dish.CategoryId);
            return View(dish);
        }

        // GET: Dishes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dish dish = db.Dish.Find(id);
            if (dish == null)
            {
                return HttpNotFound();
            }
            return View(dish);
        }

        // POST: Dishes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Dish dish = db.Dish.Find(id);
            db.Dish.Remove(dish);
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
