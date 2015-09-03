using MVCTest.DAL;
using MVCTest.Models;
using MVCTest.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace MVCTest.Controllers.Api
{
    public class AuthorsController : ApiController
    {
        private BookContext db = new BookContext();

        public ResultList<AuthorViewModel> Get([FromUri] QueryOptions qo)
        {
            var start = (qo.CurrentPage - 1) * qo.PageSize;
            var authors = db.Authors.
                OrderBy(qo.Sort).
                Skip(start).
                Take(qo.PageSize);

            qo.TotalPages = (int)Math.Ceiling((double)db.Authors.Count() / qo.PageSize);

            AutoMapper.Mapper.CreateMap<Author, AuthorViewModel>();

            return new ResultList<AuthorViewModel>()
            {
                QueryOptions = qo,
                Results = AutoMapper.Mapper.Map<List<Author>, List<AuthorViewModel>>(authors.ToList())
            };
        }

        //  Get : api/Authors/id
        [ResponseType(typeof(AuthorViewModel))]
        public IHttpActionResult Get(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Author author = db.Authors.Find(id);
            if (author == null)
            {
                return NotFound();
            }
            AutoMapper.Mapper.CreateMap<Author, AuthorViewModel>();

            return Ok(AutoMapper.Mapper.Map<Author, AuthorViewModel>(author));
        }

        public IHttpActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Author author = db.Authors.Find(id);
            if (author == null)
            {
                return NotFound();
            }
            db.Authors.Remove(author);

            return StatusCode(HttpStatusCode.NoContent);
        }

        //  Put: api/Authors/id
        [ResponseType(typeof(void))]
        public IHttpActionResult Put(AuthorViewModel author)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            AutoMapper.Mapper.CreateMap<AuthorViewModel, Author>();
            db.Entry(AutoMapper.Mapper.Map<AuthorViewModel, Author>(author)).State = EntityState.Modified;
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }


        //  Post: api/Authors
        [ResponseType(typeof(AuthorViewModel))]
        public IHttpActionResult Post(AuthorViewModel author)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            AutoMapper.Mapper.CreateMap<AuthorViewModel, Author>();
            db.Authors.Add(AutoMapper.Mapper.Map<AuthorViewModel, Author>(author));
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = author.Id }, author);
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
