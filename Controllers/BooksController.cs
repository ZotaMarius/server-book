using BookstoreApi.Data;
using BookstoreApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookstoreApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly BookstoreContext _context;

    public BooksController(BookstoreContext context)
    {
        _context = context;
    }

    // TODO - Check difference for the HTTP GET request for getBook by id and search
    // TODO - How do we associate the book with the author and genres?

    // GET: api/Books
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
    {
        return await _context.Books
            .Include(b => b.Author)  // Include the Author data
            .Include(b => b.Genres)  // Include the Genres data
            .ToListAsync();
    }    
    
    // GET: api/Books/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Book>> GetBook(int id)
    {
        // Define the SQL query to get book with its author
        var sql = @"
            SELECT b.Id, b.Title, b.AuthorId, b.ISBN, b.Price, b.Year,
                   a.Id AS Author_Id, a.Name AS Author_Name, a.Biography AS Author_Biography, a.Country AS Author_Country
            FROM Books b
            LEFT JOIN Authors a ON b.AuthorId = a.Id
            WHERE b.Id = {0}";
        
        // Log the SQL query that will be executed
        Console.WriteLine($"Executing SQL: {sql.Replace("{0}", id.ToString())}");
        
        // Execute the raw SQL query
        var bookWithAuthor = await _context.Books
            .FromSqlRaw(sql, id)
            .Include(b => b.Author)
            .AsNoTracking() // Use AsNoTracking for read-only queries
            .FirstOrDefaultAsync();
            
        if (bookWithAuthor == null)
        {
            return NotFound();
        }
        
        // Get the genres for the book with a separate query for demonstration
        var genresSql = @"
            SELECT g.*
            FROM Genres g
            INNER JOIN BookGenres bg ON g.Id = bg.GenresId
            WHERE bg.BooksId = {0}";
            
        Console.WriteLine($"Executing SQL for genres: {genresSql.Replace("{0}", id.ToString())}");
        
        // Load the genres manually
        bookWithAuthor.Genres = await _context.Genres
            .FromSqlRaw(genresSql, id)
            .AsNoTracking() // Use AsNoTracking for read-only queries
            .ToListAsync();
            
        return bookWithAuthor;
    }

    // GET: api/Books/search
    // GET /api/Books/search?title=Great
    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<Book>>> SearchBooks([FromQuery] string title)
    {
        if (string.IsNullOrEmpty(title))
        {
            return await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Genres)
                .ToListAsync();
        }

        return await _context.Books
            .Include(b => b.Author)
            .Include(b => b.Genres)
            .Where(book => book.Title.Contains(title))
            .ToListAsync();
    }

    //POST: api/Books
  [HttpPost]
public async Task<ActionResult<Book>> CreateBook(CreateBookDto dto)
{
    var author = await _context.Authors.FindAsync(dto.AuthorId);
    if (author == null) return BadRequest("Author not found");

    var book = new Book
    {
        Title = dto.Title,
        ISBN = dto.ISBN,
        Price = dto.Price,
        Year = dto.Year,
        AuthorId = dto.AuthorId,
        Genres = await _context.Genres
            .Where(g => dto.GenreIds.Contains(g.Id))
            .ToListAsync()
    };

    _context.Books.Add(book);
    await _context.SaveChangesAsync();

    return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
}

    // DELETE: api/Books/5
    // This requires authentication
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteBook(int id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book == null)
        {
            return NotFound();
        }

        _context.Books.Remove(book);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // TODO Homework - Create a new book with POST

    // TODO Homework - Associate a book with a genres

    // TODO Homework - Update a book with PUT
	// PUT: api/Books/5
[HttpPut("{id}")]
public async Task<IActionResult> UpdateBook(int id, [FromBody] CreateBookDto updatedBook)
{
    var existingBook = await _context.Books
        .Include(b => b.Genres)
        .FirstOrDefaultAsync(b => b.Id == id);

    if (existingBook == null)
    {
        return NotFound();
    }

    // Actualizează câmpurile de bază
    existingBook.Title = updatedBook.Title;
    existingBook.AuthorId = updatedBook.AuthorId;
    existingBook.ISBN = updatedBook.ISBN;
    existingBook.Price = updatedBook.Price;
    existingBook.Year = updatedBook.Year;

    // Actualizează genurile (many-to-many)
    existingBook.Genres.Clear();
    if (updatedBook.GenreIds != null && updatedBook.GenreIds.Any())
    {
        var genres = await _context.Genres
            .Where(g => updatedBook.GenreIds.Contains(g.Id))
            .ToListAsync();
        foreach (var genre in genres)
        {
            existingBook.Genres.Add(genre);
        }
    }

    await _context.SaveChangesAsync();
    return NoContent();
}
	
	
	

    // TODO Homework - Update a book with PATCH
}