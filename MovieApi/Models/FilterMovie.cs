namespace MovieApi.Models
{
    public class FilterMovie
    {
        public string Title { get; set; }
        public int Year { get; set; } = 0;
        public string Genres { get; set; }
    }
}
