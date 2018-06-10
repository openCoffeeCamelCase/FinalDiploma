using FinalDiploma.Models;
using FinalDiploma.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FinalDiploma.Controllers
{
    public class MailController : Controller
    {
        private RestaurantEntities db = new RestaurantEntities();

        public ActionResult EmailSender()
        {
            ViewBag.RoleList = db.RegUserRole.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EmailSender(string NameFrom, string EmailTo, string Message, string Header, List<string> SelectedRole)
        {
            List<string> ListOfMail = new List<string>();
            foreach (string CurrentEmail in EmailTo.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
            {
                ListOfMail.Add(CurrentEmail);
            }
            foreach (string CurrentRole in SelectedRole)
            {
                ListOfMail.AddRange(db.RegUser.Where(u => u.RegUserRole.Name == CurrentRole).Select(c => c.Email).ToList());
            }
            Mail.MailSender(NameFrom, Message, Header, ListOfMail);
            ViewBag.RoleList = db.RegUserRole.ToList();
            return View();
        }
    }
}