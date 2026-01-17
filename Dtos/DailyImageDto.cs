namespace WebApplication6.Dtos
{
    public class DailyImageDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string ImagePath { get; set; }
        public List<DailyImageLocalizationDto> Localizations { get; set; }
    }

}
