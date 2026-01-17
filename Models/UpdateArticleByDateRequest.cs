namespace WebApplication6.Models
{
    public class UpdateArticleByDateRequest
    {
        public string ImagePath { get; set; }
        public List<DailyImageLocalizationSimplified> Localizations { get; set; }
    }
}
