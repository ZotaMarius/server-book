using BookstoreApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BookstoreApi.Data
{
    public class BookstoreContext : DbContext
    {
        public BookstoreContext(DbContextOptions<BookstoreContext> options)
            : base(options)
        {
        }        public DbSet<Book> Books { get; set; } = null!;
        public DbSet<Author> Authors { get; set; } = null!;
        public DbSet<Genre> Genres { get; set; } = null!;protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure the Book entity
            modelBuilder.Entity<Book>()
                .Property(b => b.Price)
                .HasColumnType("decimal(18,2)");

            // Configure the one-to-many relationship between Author and Book
            modelBuilder.Entity<Book>()
                .HasOne(b => b.Author)
                .WithMany(a => a.Books)
                .HasForeignKey(b => b.AuthorId);

            // Configure the many-to-many relationship between Book and Genre
            modelBuilder.Entity<Book>()
                .HasMany(b => b.Genres)
                .WithMany(g => g.Books)
                .UsingEntity(j => j.ToTable("BookGenres"));

            // Seed author data first
            modelBuilder.Entity<Author>().HasData(
                new Author { Id = 1, Name = "F. Scott Fitzgerald", Country = "United States" },
                new Author { Id = 2, Name = "Harper Lee", Country = "United States" },
                new Author { Id = 3, Name = "George Orwell", Country = "United Kingdom" }
            );            // Seed genre data
            modelBuilder.Entity<Genre>().HasData(
                new Genre { Id = 1, Name = "Classic", Description = "Books that have stood the test of time" },
                new Genre { Id = 2, Name = "Fiction", Description = "Literary works created from imagination" },
                new Genre { Id = 3, Name = "Fantasy", Description = "Fiction with magical or supernatural elements" },
                new Genre { Id = 4, Name = "Romance", Description = "Stories focused on romantic relationships" },
                new Genre { Id = 5, Name = "Science Fiction", Description = "Fiction based on scientific discoveries or advanced technology" }
            );

            // Seed book data with author IDs
            modelBuilder.Entity<Book>().HasData(
                new Book { Id = 1, Title = "The Great Gatsby", AuthorId = 1, ISBN = "9780743273565", Price = 12.99M, Year = 1925 },
                new Book { Id = 2, Title = "To Kill a Mockingbird", AuthorId = 2, ISBN = "9780061120084", Price = 14.99M, Year = 1960 },
                new Book { Id = 3, Title = "1984", AuthorId = 3, ISBN = "9780451524935", Price = 11.99M, Year = 1949 }
            );            // Seed the join table for many-to-many relationship
            modelBuilder.Entity<Book>()
                .HasMany(b => b.Genres)
                .WithMany(g => g.Books)
                .UsingEntity(j => j.HasData(
                    new { BooksId = 1, GenresId = 1 }, // The Great Gatsby is a Classic
                    new { BooksId = 2, GenresId = 2 }, // To Kill a Mockingbird is Fiction
                    new { BooksId = 3, GenresId = 3 }  // 1984 is now Fantasy (was Dystopian)
                ));
        }
    }
}
