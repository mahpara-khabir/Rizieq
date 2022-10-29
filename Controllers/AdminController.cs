using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RiziqFinal.Models;
using System.Net.Mail;

namespace RiziqFinal.Controllers
{
    public class AdminController : Controller
    {
        private RiziqFinalEntities db = new RiziqFinalEntities();


        public ActionResult AdminPanel()
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Login");
            }
            return View();
        }
        public ActionResult ApproveSign()
        {
            var tBL_USER = db.TBL_USER.Include(t => t.TBL_MEMBERSHIP).Include(t => t.TBL_OCCUPATION);
            return View(tBL_USER.ToList());
        }

        public ActionResult ApproveNeed()
        {
            return View(db.NeedHelps.ToList().OrderByDescending(x => x.N_id));
        }

        public ActionResult MoneyStore()
        {
            return View(db.TBL_DONATE.ToList().OrderByDescending(x => x.d_id));
        }

        public ActionResult OtherItemStore()
        {
            return View(db.TBL_DONATEOthers.ToList().OrderByDescending(x => x.do_id));
        }

        public ActionResult Feedback()
        {
            return View(db.TBL_FEEDBACK.ToList().OrderByDescending(x => x.f_id));
        }

        // GET: Admin/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TBL_USER tBL_USER = db.TBL_USER.Find(id);
            if (tBL_USER == null)
            {
                return HttpNotFound();
            }
            return View(tBL_USER);
        }

        // GET: Admin/Create
        public ActionResult Create()
        {
            ViewBag.u_type = new SelectList(db.TBL_MEMBERSHIP, "MEM_ID", "MEM_TYPE");
            ViewBag.u_occupation = new SelectList(db.TBL_OCCUPATION, "OCC_ID", "OCC_TYPE");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "u_id,u_name,u_email,u_occupation,u_contact,u_password,u_cpassword,u_type,u_status")] TBL_USER tBL_USER)
        {
            if (ModelState.IsValid)
            {
                db.TBL_USER.Add(tBL_USER);
                db.SaveChanges();
                return RedirectToAction("ApproveSign");
            }

            ViewBag.u_type = new SelectList(db.TBL_MEMBERSHIP, "MEM_ID", "MEM_TYPE", tBL_USER.u_type);
            ViewBag.u_occupation = new SelectList(db.TBL_OCCUPATION, "OCC_ID", "OCC_TYPE", tBL_USER.u_occupation);
            return View(tBL_USER);
        }

        // GET: Admin/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TBL_USER tBL_USER = db.TBL_USER.Find(id);
            if (tBL_USER == null)
            {
                return HttpNotFound();
            }
            ViewBag.u_type = new SelectList(db.TBL_MEMBERSHIP, "MEM_ID", "MEM_TYPE", tBL_USER.u_type);
            ViewBag.u_occupation = new SelectList(db.TBL_OCCUPATION, "OCC_ID", "OCC_TYPE", tBL_USER.u_occupation);
            return View(tBL_USER);
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "u_id,u_name,u_email,u_occupation,u_contact,u_password,u_cpassword,u_type,u_status")] TBL_USER tBL_USER)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tBL_USER).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ApproveSign");
            }
            ViewBag.u_type = new SelectList(db.TBL_MEMBERSHIP, "MEM_ID", "MEM_TYPE", tBL_USER.u_type);
            ViewBag.u_occupation = new SelectList(db.TBL_OCCUPATION, "OCC_ID", "OCC_TYPE", tBL_USER.u_occupation);
            return View(tBL_USER);
        }

        // GET: Admin/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TBL_USER tBL_USER = db.TBL_USER.Find(id);
            if (tBL_USER == null)
            {
                return HttpNotFound();
            }
            return View(tBL_USER);
        }

        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TBL_USER tBL_USER = db.TBL_USER.Find(id);
            db.TBL_USER.Remove(tBL_USER);
            db.SaveChanges();
            return RedirectToAction("ApproveSign");
        }



        public ActionResult DeleteFeedback(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TBL_FEEDBACK tBL_Feedback = db.TBL_FEEDBACK.Find(id);
            if (tBL_Feedback == null)
            {
                return HttpNotFound();
            }
            return View(tBL_Feedback);
        }

        // POST: Admin/Delete/5
        [HttpPost, ActionName("DeleteFeedback")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteFeedbackConfirmed(int id)
        {
            TBL_FEEDBACK tBL_Feedback = db.TBL_FEEDBACK.Find(id);
            db.TBL_FEEDBACK.Remove(tBL_Feedback);
            db.SaveChanges();
            return RedirectToAction("Feedback");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public ActionResult Login()
        {

            return View();
        }


        [HttpPost]
        public ActionResult Login(TBL_ADMIN u)
        {


            TBL_ADMIN ui = db.TBL_ADMIN.Where(x => x.ad_email == u.ad_email && x.ad_password == u.ad_password).SingleOrDefault();
            if (ui != null)
            {
                Session["uid"] = ui.MEM_ID;
                Session["ad_email"] = u.ad_email.ToString();
                return RedirectToAction("AdminPanel");

            }
            else
            {
                ViewBag.error = "Invalid email or password or usertype..";
            }
            return View();
        }

        public ActionResult sendemail()
        {
            return View();
        }
        [HttpPost]
        public ActionResult sendemail(RiziqFinal.Models.gmail model)
        {
            MailMessage mm = new MailMessage("riziquewebsite@gmail.com", model.To);
            mm.Subject = model.Subject;
            mm.Body = model.Body;
            mm.IsBodyHtml = false;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.EnableSsl = true;

            NetworkCredential nc = new NetworkCredential("riziquewebsite@gmail.com", "180104031");
            smtp.UseDefaultCredentials = true;
            smtp.Credentials = nc;
            smtp.Send(mm);
            ViewBag.Messages = "Mail has Been Sent Successfully";
            return View();
        }


    }
}