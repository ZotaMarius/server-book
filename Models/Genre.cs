using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BookstoreApi.Models;

public class Genre
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;

    [StringLength(200)]
    public string? Description { get; set; }

    // Navigation property for the many-to-many relationship
    [JsonIgnore] // Prevent circular references in JSON serialization
    public ICollection<Book> Books { get; set; } = new List<Book>();
}
