using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CompanyBookstore.Models
{
    public class BookstoreContext: DbContext
    {
        public BookstoreContext(DbContextOptions<BookstoreContext> options): base(options)
        {
        }
        public DbSet<Book> Books { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>().HasKey(b => b.BookID);
            modelBuilder.Entity<Book>().ToTable("Book","Dbo");
        }
    }
}
