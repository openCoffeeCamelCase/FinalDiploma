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
    public class CustomMenusController : Controller
    {
        private RestaurantEntities db = new RestaurantEntities();

        // GET: CustomMenus
        public ActionResult Index()
        {
            var customMenu = db.CustomMenu.Include(c => c.RegUser);
            IEnumerable<CustomMenuMeals> customMenuMeals = db.CustomMenuMeals.AsEnumerable();
            ViewBag.Alldishes = customMenuMeals;
            return View(customMenu.ToList());
        }

        // GET: CustomMenus/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomMenu customMenu = db.CustomMenu.Find(id);
            IEnumerable<CustomMenuMeals> customMenuMeals = db.CustomMenuMeals.Where(u => u.CustomMenuId == id).AsEnumerable();
            if (customMenu == null)
            {
                return HttpNotFound();
            }
            ViewBag.TotalCost = 0;
            foreach (CustomMenuMeals wh in customMenuMeals)
            {
                ViewBag.TotalCost += wh.Dish.Price;
               
            }
            ViewBag.dishes = customMenuMeals;
            return View(customMenu);          
        }

        // GET: CustomMenus/Create
        public ActionResult Create()
        {
            ViewBag.RegUserId = new SelectList(db.RegUser, "Id", "Login");
            return View();
        }

        // POST: CustomMenus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,RegUserId")] CustomMenu customMenu)
        {
            if (ModelState.IsValid)
            {
                db.CustomMenu.Add(customMenu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.RegUserId = new SelectList(db.RegUser, "Id", "Login", customMenu.RegUserId);
            return View(customMenu);
        }

        // GET: CustomMenus/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomMenu customMenu = db.CustomMenu.Find(id);
            if (customMenu == null)
            {
                return HttpNotFound();
            }
            ViewBag.RegUserId = new SelectList(db.RegUser, "Id", "Login", customMenu.RegUserId);
            return View(customMenu);
        }

        // POST: CustomMenus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,RegUserId")] CustomMenu customMenu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customMenu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RegUserId = new SelectList(db.RegUser, "Id", "Login", customMenu.RegUserId);
            return View(customMenu);
        }

        // GET: CustomMenus/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomMenu customMenu = db.CustomMenu.Find(id);
            if (customMenu == null)
            {
                return HttpNotFound();
            }
            return View(customMenu);
        }

        // POST: CustomMenus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CustomMenu customMenu = db.CustomMenu.Find(id);
            db.CustomMenu.Remove(customMenu);
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
