using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using MVCTest.Models;

namespace MVCTest.DAL
{
    public class BookInitializer:DropCreateDatabaseIfModelChanges<BookContext>
    {
        protected override void Seed(BookContext context)
        {
            var author = new Author
            {
                Name = "Tester"
            };
            var books = new List<Book>
            {
                new Book
                {
                    Author = author,
                    Title = "TextBook"
                },
                new Book
                {
                    Author = author,
                    Title= "PicBook"
                }
            };
            books.ForEach(b => context.Books.Add(b));
            context.SaveChanges();
        }
    }
}