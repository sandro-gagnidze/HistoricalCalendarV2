using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication6.Models;

namespace WebApplication6.Repository.Services
{
    public interface IYearInfoRepository
    {
        Task<IEnumerable<YearInfo>> GetAllAsync();
        Task<YearInfo> GetByIdAsync(int id);
        Task<YearInfo> GetByYearAsync(int year);
        Task<YearInfo> CreateAsync(YearInfo yearDescription);
        Task UpdateAsync(YearInfo yearDescription);
        Task DeleteAsync(int id);
    }
}
