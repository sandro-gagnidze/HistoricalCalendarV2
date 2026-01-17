namespace WebApplication6.Models
{
    public class DailyImageLocalization
    {
        public int Id { get; set; }
        public int DailyImageId { get; set; }
        public DailyImage DailyImage { get; set; }
        public string LanguageCode { get; set; } // "ka" ან "en"
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
