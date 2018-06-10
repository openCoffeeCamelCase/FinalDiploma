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
    public class OrdsController : Controller
    {
        private RestaurantEntities db = new RestaurantEntities();

        // GET: Ords
        public ActionResult Index()
        {
            
            var ord = db.Ord.Include(o => o.RegUser).Include(o => o.Tabl).Include(o => o.RegUser1);
            return View(ord.ToList());
        }

        // GET: Ords/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ord ord = db.Ord.Find(id);
            IEnumerable<Meals> MenuMeals = db.Meals.Where(u => u.OrdId == id).AsEnumerable();
            ViewBag.dishes = MenuMeals;
            if (ord == null)
            {
                return HttpNotFound();
            }
            return View(ord);
        }

        // GET: Ords/Create
        public ActionResult Create()
        {
            ViewBag.ClientId = new SelectList(db.RegUser, "Id", "Login");
            ViewBag.TableId = new SelectList(db.Tabl, "Id", "Number");
            ViewBag.WaiterId = new SelectList(db.RegUser, "Id", "Login");
            return View();
        }

        // POST: Ords/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ClientId,WaiterId,TimeOpen,TimeEnd,TableId,TotalCost")] Ord ord)
        {
            if (ModelState.IsValid)
            {
                ord.TimeOpen = DateTime.Now;
                db.Ord.Add(ord);
                db.SaveChanges();
                return RedirectToAction("Details", "Ords", new { id = ord.Id
                    });
            }

            ViewBag.ClientId = new SelectList(db.RegUser, "Id", "Login", ord.ClientId);
            ViewBag.TableId = new SelectList(db.Tabl, "Id", "Number", ord.TableId);
            ViewBag.WaiterId = new SelectList(db.RegUser, "Id", "Login", ord.WaiterId);
            return View(ord);
        }

        public ActionResult CloseOrd(int id)
        {
            Ord ord = db.Ord.Find(id);
            ord.TimeEnd = DateTime.Now;
            db.Entry(ord).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Ords/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ord ord = db.Ord.Find(id);
            if (ord == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClientId = new SelectList(db.RegUser, "Id", "Login", ord.ClientId);
            ViewBag.TableId = new SelectList(db.Tabl, "Id", "Number", ord.TableId);
            ViewBag.WaiterId = new SelectList(db.RegUser, "Id", "Login", ord.WaiterId);
            return View(ord);
        }

        // POST: Ords/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ClientId,WaiterId,TimeOpen,TimeEnd,TableId,TotalCost")] Ord ord)
        {
            if (ModelState.IsValid)
            {
                if (ord.TimeEnd != null)
                {
                    DateTime te = ord.TimeEnd ?? DateTime.Now;
                    ord.TimeEnd = te.ToUniversalTime();
                }
                DateTime to = ord.TimeOpen;
                ord.TimeOpen = to.ToUniversalTime();
                
                db.Entry(ord).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClientId = new SelectList(db.RegUser, "Id", "Login", ord.ClientId);
            ViewBag.TableId = new SelectList(db.Tabl, "Id", "Number", ord.TableId);
            ViewBag.WaiterId = new SelectList(db.RegUser, "Id", "Login", ord.WaiterId);
            return View(ord);
        }

        // GET: Ords/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ord ord = db.Ord.Find(id);
            if (ord == null)
            {
                return HttpNotFound();
            }
            return View(ord);
        }

        // POST: Ords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ord ord = db.Ord.Find(id);
            db.Ord.Remove(ord);
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
