using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication6.Dtos;
using WebApplication6.Models;
using WebApplication6.Repository.Services;
using WebApplication6.Request;

namespace WebApplication6.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class CalendarController : ControllerBase
    {
        private readonly IDailyImageRepository _repository;

        public CalendarController(IDailyImageRepository repository)
        {
            _repository = repository;
        }

        [AllowAnonymous]
        [HttpGet("years")]
        public async Task<IActionResult> GetAvailableYears()
        {
            var years = await _repository.GetAvailableYearsAsync();
            return Ok(years);
        }

        [AllowAnonymous]
        [HttpGet("{year}/months")]
        public async Task<IActionResult> GetAvailableMonths(int year)
        {
            var months = await _repository.GetAvailableMonthsAsync(year);
            return Ok(months);
        }

        [AllowAnonymous]
        [HttpGet("{year}/{month}/days")]
        public async Task<IActionResult> GetCalendarDays(int year, int month)
        {
            if (month < 1 || month > 12)
                return BadRequest("Invalid month. Month must be between 1 and 12.");

            var days = await _repository.GetCalendarDaysAsync(year, month);
            return Ok(days);
        }

        [AllowAnonymous]
        [HttpGet("articles")]
        public async Task<IActionResult> GetArticles([FromQuery] DateTime date, [FromQuery] string lang = "ka")
        {
            var articles = await _repository.GetArticlesForThreeDaysAsync(date);

            if (!articles.Any())
                return NotFound("No articles found for the specified date or adjacent days.");

            var articlesWithLocalization = new List<object>();

            foreach (var article in articles)
            {
                var localization = await _repository.GetArticleLocalizationByIdAndLanguageAsync(article.Id, lang);
                articlesWithLocalization.Add(new
                {
                    id = article.Id,
                    date = article.Date,
                    imagePath = article.ImagePath,
                    title = localization?.Title,
                    description = localization?.Description
                });
            }

            return Ok(articlesWithLocalization);
        }

        [HttpPost]
        public async Task<IActionResult> CreateArticle([FromBody] Models.DailyImageRequest request)
        {
            if (request == null || request.Article == null || request.Localizations == null)
                return BadRequest("Invalid request data.");

            var article = new DailyImage
            {
                Date = request.Article.Date.Date, // მხოლოდ თარიღი, საათების გარეშე
                ImagePath = request.Article.ImagePath
            };

            var localizations = request.Localizations.Select(l => new DailyImageLocalization
            {
                LanguageCode = l.LanguageCode,
                Title = l.Title,
                Description = l.Description
            }).ToList();
            try    
            {
            await _repository.CreateArticleWithLocalizationAsync(article, localizations);

            return CreatedAtAction(nameof(GetTodayAndAdjacentDaysArticles), new { lang = "ka" }, article);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message); 
            }
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetArticleById(int id, [FromQuery] string lang = "ka")
        {
            var articleLocalization = await _repository.GetArticleLocalizationByIdAndLanguageAsync(id, lang);
            if (articleLocalization == null)
                return NotFound("Article not found.");

            return Ok(articleLocalization);
        }

        [AllowAnonymous]
        [HttpGet("today-and-adjacent-days")]
        public async Task<IActionResult> GetTodayAndAdjacentDaysArticles([FromQuery] string lang = "ka")
        {
            // დღევანდელი დღე
            DateTime today = DateTime.Now.Date;

            // მივიღოთ სტატიები სამი დღისთვის რეპოზიტორიის მეთოდის გამოყენებით
            var articles = await _repository.GetArticlesForThreeDaysAsync(today);

            if (!articles.Any())
                return NotFound("No articles found for today and adjacent days.");

            var articlesWithLocalization = new List<object>();

            foreach (var article in articles)
            {
                var localization = await _repository.GetArticleLocalizationByIdAndLanguageAsync(article.Id, lang);
                articlesWithLocalization.Add(new
                {
                    id = article.Id,
                    date = article.Date,
                    imagePath = article.ImagePath,
                    title = localization?.Title,
                    description = localization?.Description
                });
            }

            return Ok(articlesWithLocalization);
        }

        [AllowAnonymous]
        [HttpGet("localizations/{id}")]
        public async Task<IActionResult> GetArticleLocalizations(int id)
        {
            var localizations = await _repository.GetArticleLocalizationsByIdAsync(id);
            if (localizations == null || !localizations.Any())
                return NotFound("Localizations not found.");

            return Ok(localizations);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateArticle(int id, [FromBody] DailyImage article)
        {
            if (id != article.Id)
                return BadRequest("ID mismatch.");

            await _repository.UpdateArticleAsync(article);
            return NoContent();
        }

        [HttpPut("update-by-date")]
        public async Task<IActionResult> UpdateArticleByDate(
    [FromQuery] string date,
    [FromBody] UpdateArticleByDateRequest request)
        {
            if (!DateTime.TryParse(date, out DateTime parsedDate))
            {
                return BadRequest("Invalid date format. Use format: YYYY-MM-DD");
            }

            var localizations = request.Localizations.Select(l => new DailyImageLocalization
            {
                LanguageCode = l.LanguageCode,
                Title = l.Title,
                Description = l.Description
            }).ToList();

            await _repository.UpdateArticleByDateAsync(parsedDate, request.ImagePath, localizations);

            // პოვნა განახლებული სტატიისთვის
            var updatedArticles = await _repository.GetArticlesForThreeDaysAsync(parsedDate);
            var updatedArticle = updatedArticles.FirstOrDefault(a => a.Date.Date == parsedDate.Date);

            if (updatedArticle == null)
            {
                return NotFound("Article not found.");
            }

            // გადაყვანა DTO-ში
            var articleDto = new DailyImageDto
            {
                Id = updatedArticle.Id,
                Date = updatedArticle.Date,
                ImagePath = updatedArticle.ImagePath,
                Localizations = updatedArticle.Localizations.Select(l => new DailyImageLocalizationDto
                {
                    Id = l.Id,
                    LanguageCode = l.LanguageCode,
                    Title = l.Title,
                    Description = l.Description
                }).ToList()
            };

            return Ok(articleDto);
        }


        [HttpDelete]
        public async Task<IActionResult> DeleteArticleByDate([FromQuery] string date)
        {
            if (!DateTime.TryParse(date, out DateTime parsedDate))
            {
                return BadRequest("Invalid date format. Use format: YYYY-MM-DD");
            }

            await _repository.DeleteArticleByDateAsync(parsedDate);
            return NoContent();
        }

        [HttpPost("upload-image")]
        public async Task<IActionResult> UploadImage([FromForm]IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            try
            {
                var imageUrl = await _repository.UploadImageAsync(file);
                return Ok(new { url = imageUrl });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}

