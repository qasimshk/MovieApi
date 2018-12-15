using System;

namespace MovieApi.Models
{
    public class Rating
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public Guid UserId { get; set; }
        public int MovieRating { get; set; }
    }
}