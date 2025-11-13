using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PRN_PE.Models
{
    public class PostEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string? ImageUrl { get; set; }
        // Average rating for the post (nullable if no ratings yet)
        // Mapped to DECIMAL(3,2) in the database (e.g. values like 4.25)
        [Column(TypeName = "decimal(3,2)")]
        [Range(1.00, 5.00)]
        public decimal? Rating { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}