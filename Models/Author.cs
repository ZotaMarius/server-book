using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BookstoreApi.Models;

public class Author
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)] // Prefer MaxLength over StringLength for EF Core
    public string Name { get; set; } = string.Empty;

    [MaxLength(200)]
    public string? Biography { get; set; }

    [MaxLength(100)]
    public string? Country { get; set; }

    [JsonIgnore]
    public ICollection<Book> Books { get; set; } = new List<Book>();
}
