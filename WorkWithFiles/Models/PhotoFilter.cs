namespace WorkWithFiles.Models
{
    public class PhotoFilter
    {
        public int Page { get; set; } = 1;
        public int Limit { get; set; } = 1;

        public string? Search {  get; set; }
    }
}
