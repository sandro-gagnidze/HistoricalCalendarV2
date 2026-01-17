using WebApplication6.Models;

namespace WebApplication6.Request
{
    public class DailyImageRequest
    {
        public DailyImage Article { get; set; }
        public List<DailyImageLocalization> Localizations { get; set; }
    }
}
