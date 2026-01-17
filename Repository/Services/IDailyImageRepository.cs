using WebApplication6.Models;

namespace WebApplication6.Repository.Services
{
    public interface IDailyImageRepository
    {
        Task<DailyImage> GetArticleByDateAsync(DateTime date);
        Task<IEnumerable<int>> GetAvailableYearsAsync();
        Task<IEnumerable<int>> GetAvailableMonthsAsync(int year);
        Task<IEnumerable<int>> GetCalendarDaysAsync(int year, int month);
        Task<IEnumerable<DailyImage>> GetArticlesForThreeDaysAsync(DateTime date);
        Task<DailyImage> GetArticleByIdAsync(int id);
        Task<DailyImage> CreateArticleAsync(DailyImage article);
        Task UpdateArticleAsync(DailyImage article);
        Task DeleteArticleByDateAsync(DateTime date);
        Task<string> UploadImageAsync(IFormFile file);
        Task UpdateArticleByDateAsync(DateTime date, string imagePath, List<DailyImageLocalization> localizations);
        Task<DailyImageLocalization> GetArticleLocalizationByIdAndLanguageAsync(int id, string languageCode);
        Task CreateArticleWithLocalizationAsync(DailyImage article, List<DailyImageLocalization> localizations);
        Task<List<DailyImageLocalization>> GetArticleLocalizationsByIdAsync(int id);
    }
}
