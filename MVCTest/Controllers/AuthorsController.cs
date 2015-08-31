using MVCTest.DAL;
using MVCTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Net;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Mvc;

namespace MVCTest.Controllers
{
    public class AuthorsController : Controller
    {
        private BookContext db = new BookContext();

        //  GET: /Authors/
        public ActionResult Index([Form] QueryOptions qo)
        {
            var start = (qo.CurrentPage - 1) * qo.PageSize;
            var authors = db.Authors.
                OrderBy(qo.Sort).
                Skip(start).
                Take(qo.PageSize);

            qo.TotalPages = (int)Math.Ceiling((double)db.Authors.Count() / qo.PageSize);

            ViewBag.QueryOptions = qo;
            return View(authors.ToList());
        }

        //  GET: /Authors/Edit/id
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Author author = db.Authors.Find(id);
            if (author == null)
            {
                return HttpNotFound();
            }
            return View("Form", author);
        }

        //  GET: /Authors/Detail/id
        public ActionResult Detail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Author author = db.Authors.Find(id);
            if (author == null)
            {
                return HttpNotFound();
            }
            return View(author);
        }


        // GET: /Authors/Create
        public ActionResult Create()
        {
            return View("Form", new Author());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Author author)
        {
            if (ModelState.IsValid)
            {
                db.Authors.Add(author);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(author);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Author author = db.Authors.Find(id);
            if (author == null)
            {
                return HttpNotFound();
            }
            return View(author);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            Author author = db.Authors.Find(id);
            db.Authors.Remove(author);
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
