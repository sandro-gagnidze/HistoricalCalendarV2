namespace WebApplication6.Models.ImageSimplify
{
    public class ArticleWithImageRequest
    {
        public DailyImageSimplified Article { get; set; }
        public List<DailyImageLocalizationSimplified> Localizations { get; set; }
        public IFormFile Image { get; set; }
    }
}
