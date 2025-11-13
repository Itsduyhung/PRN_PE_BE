using System;

namespace PRN_PE.DTOs.Response
{
    public class PostResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string? ImageUrl { get; set; }
        // Average rating exposed to clients; nullable when there are no ratings yet
        public decimal? Rating { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}