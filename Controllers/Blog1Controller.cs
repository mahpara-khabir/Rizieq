using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RiziqFinal.Models;

namespace RiziqFinal.Controllers
{
    public class Blog1Controller : Controller
    {
        RiziqFinalEntities nd = new RiziqFinalEntities();
        // GET: Volunteer
        public ActionResult Index()
        {
            using (RiziqFinalEntities nd = new RiziqFinalEntities())
            {
                return View(nd.BlogPosts.ToList().OrderByDescending(x => x.BID));

            }
        }
    }
}