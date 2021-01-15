using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using CompanyBookstore.Models;
using Microsoft.AspNetCore.Authorization;
using PagedList;

namespace CompanyBookstore.Controllers
{
    [Authorize]
    public class BooksController : Controller
    {
         BookstoreContext db;

        public BooksController(BookstoreContext db)
        {
            this.db = db;
        }

        public async Task <IActionResult> Index(string sortOrder, string searchString, string currentFilter, string searchBy, int? pageNumber)
        {
            ViewBag.Message = HttpContext.Session.GetString("Session_1");

            ViewBag.CurrentSort = sortOrder;
            ViewBag.TitleSortParm = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewBag.CategorySortParm = sortOrder == "Category" ? "category_desc" : "Category";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.AuthorSortParm = sortOrder == "Author" ? "author_desc" : "Author";
            ViewBag.PriceSortParm = sortOrder == "Price" ? "price_desc" : "Price";
            ViewBag.PublisherSortParm = sortOrder == "Publisher" ? "publisher_desc" : "Publisher";
            ViewBag.BookIdSortParm = sortOrder == "BookId" ? "id_desc" : "BookId";
            ViewBag.BestsellerSortParm = sortOrder == "Bestseller" ? "bestseller_desc" : "Bestseller";

            

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.currentFilter = searchString;

            var books = db.Books.AsQueryable();

            if (searchBy == "Title")
            {
                books = books.Where(b => b.BookTitle.Contains(searchString) || searchString == null);
            }
            else if (searchBy == "Author")
            {
                books = books.Where(b => b.BookAuthor.StartsWith(searchString) || searchString == null);
            }
            else if (searchBy == "Publisher")
            {
                books = books.Where(b => b.Publisher.Contains(searchString) || searchString == null);
            }
            switch (sortOrder)
            {
                case "title_desc":
                    books = books.OrderByDescending(b => b.BookTitle);
                    break;
                case "Category":
                    books = books.OrderBy(b => b.Category);
                    break;
                case "category_desc":
                    books = books.OrderByDescending(b => b.Category);
                    break;
                case "Author":
                    books = books.OrderBy(b => b.BookAuthor);
                    break;
                case "author_desc":
                    books = books.OrderByDescending(b => b.BookAuthor);
                    break;
                case "Date":
                    books = books.OrderBy(b => b.ReleaseDate);
                    break;
                case "date_desc":
                    books = books.OrderByDescending(b => b.ReleaseDate);
                    break;
                case "price_desc":
                    books = books.OrderByDescending(b => b.BookPrice);
                    break;
                case "Price":
                    books = books.OrderBy(b => b.BookPrice);
                    break;
                case "publisher_desc":
                    books = books.OrderByDescending(b => b.Publisher);
                    break;
                case "Publisher":
                    books = books.OrderBy(b => b.Publisher);
                    break;
                case "id_desc":
                    books = books.OrderByDescending(b => b.BookID);
                    break;
                case "BookId":
                    books = books.OrderBy(b => b.BookID);
                    break;
                case "bestseller_desc":
                    books = books.OrderByDescending(b => b.Bestseller);
                    break;
                case "Besteller":
                    books = books.OrderBy(b => b.Bestseller);
                    break;

                default:
                    books = books.OrderBy(b => b.BookAuthor);
                    break;
            }
            int pageSize = 15;

            return View( await PaginatedList <Book>.CreateAsync(books.AsNoTracking(), pageNumber ?? 1, pageSize));
        
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                Response.StatusCode = 404;
                return View("errorNullId");
            }

            var book = await db.Books
                .FirstOrDefaultAsync(m => m.BookID == id);
            if (book == null)
            {
                Response.StatusCode = 404;
                return View("errorBookNotFound", id.Value);
            }
            return View(book);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Category,BookTitle,BookAuthor,BookPrice,BookDescription,Bestseller,Publisher,ReleaseDate,Pages")] Book book)
        {
            bool NotUniqueTitle = db.Books.Any(x => x.BookTitle == book.BookTitle);
            if (NotUniqueTitle)
            {
                ModelState.AddModelError("Title", "Książka o tym tytule już istnieje");
            }
            try
            {
                if (ModelState.IsValid)
                {
                    db.Add(book);
                    await db.SaveChangesAsync();
                    TempData["Message"] = "Dodano książkę o tytule " + book.BookTitle + ", której autorem jest " + book.BookAuthor;
                    var title = book.BookTitle;
                    var author = book.BookAuthor;
                    HttpContext.Session.SetString("Session_1", "Niedawno dodano informacje o książce: " + title + ", której autorem jest " + author);
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (System.Data.DataException)
            {
                ModelState.AddModelError("", "Dodawanie książki nie powiodło się");
            }
            return View(book);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {

                Response.StatusCode = 404;
                return View("errorNullId");
            }

            var book = await db.Books.FindAsync(id);
            if (book == null)
            {
                Response.StatusCode = 404;
                return View("errorBookNotFound", id.Value);
            }
            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookID,Category,BookTitle,BookAuthor,BookPrice,BookDescription,Bestseller,Publisher,ReleaseDate,Pages")] Book book)
        {
            if (id != book.BookID)
            {
                Response.StatusCode = 404;
                return View("errorWrongId");
            }
            /*bool NotUniqueTitle = db.Books.Any(x => x.BookTitle == book.BookTitle && x.BookID != id);

            if (NotUniqueTitle)
            {
                ModelState.AddModelError("Title", "Książka o tym tytule już istnieje");
            }*/

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(book);
                    await db.SaveChangesAsync();
                    TempData["Message"] = "Informację dotyczące książki " + book.BookTitle + " zostały zmienione";
                    var title = book.BookTitle;
                    var author = book.BookAuthor;
                    HttpContext.Session.SetString("Session_1", "Niedawno zmodyfiowano informacje o książce: " + title + ", której autorem jest " + author);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.BookID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                Response.StatusCode = 404;
                return View("errorWrongId");
            }

            var book = await db.Books
                .FirstOrDefaultAsync(m => m.BookID == id);
            if (book == null)
            {
                Response.StatusCode = 404;
                return View("errorBookNotFound", id.Value);
            }

            return View(book);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                Book bookToDelete = new Book { BookID = id };
                db.Entry(bookToDelete).State = EntityState.Deleted;
                await db.SaveChangesAsync();
                TempData["Message"] = "Usunięto książkę";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                return RedirectToAction("Delete", new { id = id, saveChangesError = true });
            }
        }

        private bool BookExists(int id)
        {
            return db.Books.Any(e => e.BookID == id);
        }
    }
}


