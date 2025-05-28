using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BookstoreApi.Models;

public class Author
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [StringLength(200)]
    public string? Biography { get; set; }

    [StringLength(100)]
    public string? Country { get; set; }

    // Navigation property for the books by this author
    // Using JsonIgnore is one approach to prevent circular references
    [JsonIgnore]
    public ICollection<Book> Books { get; set; } = new List<Book>();
}
