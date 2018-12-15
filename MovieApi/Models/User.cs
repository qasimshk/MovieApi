using System;

namespace MovieApi.Models
{
    public class User
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; }
    }
}