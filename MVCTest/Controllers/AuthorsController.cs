using MVCTest.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCTest.Controllers
{
    public class AuthorsController : Controller
    {
        private BookContext db = new BookContext();
        //
        // GET: /Authors/

        public ActionResult Index()
        {
            return View(db.Authors.ToList());
        }

    }
}
