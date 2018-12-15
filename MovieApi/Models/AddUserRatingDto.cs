using System;
using System.ComponentModel.DataAnnotations;

namespace MovieApi.Models
{
    public class AddUserRatingDto
    {        
        [Required]
        public Guid userId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int MovieId { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }
    }
}
