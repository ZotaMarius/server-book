using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BookstoreApi.Models;

public class Genre
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)] // Use MaxLength for PostgreSQL compatibility
    public string Name { get; set; } = string.Empty;

    [MaxLength(200)]
    public string? Description { get; set; }

    [JsonIgnore] // Avoid circular reference during JSON serialization
    public ICollection<Book> Books { get; set; } = new List<Book>();
}
