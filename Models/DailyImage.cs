namespace WebApplication6.Models
{
    public class DailyImage
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string ImagePath { get; set; }
        public ICollection<DailyImageLocalization> Localizations { get; set; }
    }

    public class DailyImageSimplified
    {
        public DateTime Date { get; set; }
        public string ImagePath { get; set; }
    }

    public class DailyImageLocalizationSimplified
    {
        public string LanguageCode { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }


}
