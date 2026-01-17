using Microsoft.EntityFrameworkCore;
using WebApplication6.Data;
using WebApplication6.Models;
using WebApplication6.Repository.Services;

namespace WebApplication6.Repository.Implementation
{
    public class DailyImageRepository : IDailyImageRepository
    {
        private readonly AppDbContext _context;

        public DailyImageRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<int>> GetAvailableYearsAsync()
        {
            return await _context.DailyImages
                           .Select(d => d.Date.Year)
                           .Distinct()
                           .OrderBy(y => y)
                           .ToListAsync();
        }

        public async Task<IEnumerable<int>> GetAvailableMonthsAsync(int year)
        {
            return await _context.DailyImages
                           .Where(d => d.Date.Year == year)
                           .Select(d => d.Date.Month)
                           .Distinct()
                           .OrderBy(m => m)
                           .ToListAsync();
        }

        public async Task<IEnumerable<int>> GetCalendarDaysAsync(int year, int month)
        {
            return await _context.DailyImages
                           .Where(d => d.Date.Year == year && d.Date.Month == month)
                           .Select(d => d.Date.Day)
                           .Distinct()
                           .OrderBy(d => d)
                           .ToListAsync();
        }

        public async Task<IEnumerable<DailyImage>> GetArticlesForThreeDaysAsync(DateTime date)
        {
            DateTime previousDay = date.Date.AddDays(-1);
            DateTime currentDay = date.Date;
            DateTime nextDay = date.Date.AddDays(1);

            return await _context.DailyImages
                           .Where(d => d.Date.Date == previousDay || d.Date.Date == currentDay || d.Date.Date == nextDay)
                           .OrderBy(d => d.Date)
                           .ToListAsync();
        }

        public async Task<DailyImage> GetArticleByIdAsync(int id)
        {
            return await _context.DailyImages.FindAsync(id);
        }

        public async Task<DailyImage> CreateArticleAsync(DailyImage article)
        {
            article.Date = article.Date.Date; // მხოლოდ თარიღი, საათების გარეშე
            await _context.DailyImages.AddAsync(article);
            await _context.SaveChangesAsync();
            return article;
        }

        public async Task UpdateArticleAsync(DailyImage article)
        {
            article.Date = article.Date.Date; // მხოლოდ თარიღი, საათების გარეშე
            _context.Entry(article).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        public async Task UpdateArticleByDateAsync(DateTime date, string imagePath, List<DailyImageLocalization> localizations)
        {
            // პოვნათ სტატია მხოლოდ თარიღის მიხედვით
            var article = await _context.DailyImages
                .Include(a => a.Localizations) // ლოკალიზაციების ჩატვირთვა
                .FirstOrDefaultAsync(d => d.Date.Date == date.Date);

            if (article == null)
            {
                throw new KeyNotFoundException("Article not found for the specified date.");
            }

            // განაახლოთ სტატიის სურათის ბილიკი
            article.ImagePath = imagePath;

            // განაახლოთ ლოკალიზაციები
            foreach (var localization in localizations)
            {
                var existingLocalization = article.Localizations.FirstOrDefault(l => l.LanguageCode == localization.LanguageCode);

                if (existingLocalization != null)
                {
                    existingLocalization.Title = localization.Title;
                    existingLocalization.Description = localization.Description;
                }
                else
                {
                    localization.DailyImageId = article.Id;
                    article.Localizations.Add(localization);
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteArticleByDateAsync(DateTime date)
        {
            var articles = await _context.DailyImages
                                        .Where(d => d.Date.Date == date.Date)
                                        .ToListAsync();

            if (articles.Any())
            {
                foreach (var article in articles)
                {
                    // წაშლა ლოკალიზაციების ცხრილიდან
                    var localizations = await _context.DailyImageLocalizations
                                                     .Where(l => l.DailyImageId == article.Id)
                                                     .ToListAsync();

                    _context.DailyImageLocalizations.RemoveRange(localizations);

                    // წაშლა სტატიის ცხრილიდან
                    _context.DailyImages.Remove(article);
                }

                await _context.SaveChangesAsync();
            }
        }

        public async Task<string> UploadImageAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("No file uploaded.");

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"/uploads/{fileName}";
        }

        public async Task<DailyImageLocalization> GetArticleLocalizationByIdAndLanguageAsync(int id, string languageCode)
        {
            return await _context.DailyImageLocalizations.FirstOrDefaultAsync(l => l.DailyImageId == id && l.LanguageCode == languageCode);
        }

        public async Task CreateArticleWithLocalizationAsync(DailyImage article, List<DailyImageLocalization> localizations)
        {
            article.Date = article.Date.Date; // მხოლოდ თარიღი, საათების გარეშე
            await _context.DailyImages.AddAsync(article);
            await _context.SaveChangesAsync();

            foreach (var localization in localizations)
            {
                localization.DailyImageId = article.Id;
                await _context.DailyImageLocalizations.AddAsync(localization);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<List<DailyImageLocalization>> GetArticleLocalizationsByIdAsync(int id)
        {
            return await _context.DailyImageLocalizations.Where(l => l.DailyImageId == id).ToListAsync();
        }
    }
}
