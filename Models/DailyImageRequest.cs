namespace WebApplication6.Models
{
    public class DailyImageRequest
    {
        public DailyImageSimplified Article { get; set; }
        public List<DailyImageLocalizationSimplified> Localizations { get; set; }
    }


}
