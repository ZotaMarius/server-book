using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookstoreApi.Models;

public class Book
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    // Foreign key to Author
    [ForeignKey(nameof(Author))]
    public int AuthorId { get; set; }

    public Author? Author { get; set; }

    [Required]
    [MaxLength(20)]
    public string ISBN { get; set; } = string.Empty;

    [Range(0, 1000)]
    public decimal Price { get; set; }

    [Range(1000, 3000)]
    public int Year { get; set; }

    // Many-to-many relationship with Genre
    public ICollection<Genre> Genres { get; set; } = new List<Genre>();
}

public class CreateBookDto
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public int AuthorId { get; set; }

    [Required]
    [MaxLength(20)]
    public string ISBN { get; set; } = string.Empty;

    [Range(0, 1000)]
    public decimal Price { get; set; }

    [Range(1000, 3000)]
    public int Year { get; set; }

    public List<int> GenreIds { get; set; } = new();
}