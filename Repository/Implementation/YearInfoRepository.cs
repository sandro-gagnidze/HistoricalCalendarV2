using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApplication6.Data;
using WebApplication6.Models;
using WebApplication6.Repository.Services;

namespace WebApplication6.Repository.Implementation
{
    public class YearInfoRepository : IYearInfoRepository
    {
        private readonly AppDbContext _context;

        public YearInfoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<YearInfo>> GetAllAsync()
        {
            return await _context.YearInfos.ToListAsync();
        }

        public async Task<YearInfo> GetByIdAsync(int id)
        {
            return await _context.YearInfos.FindAsync(id);
        }

        public async Task<YearInfo> GetByYearAsync(int year)
        {
            return await _context.YearInfos.FirstOrDefaultAsync(yd => yd.Year == year);
        }

        public async Task<YearInfo> CreateAsync(YearInfo YearInfo)
        {
            await _context.YearInfos.AddAsync(YearInfo);
            await _context.SaveChangesAsync();
            return YearInfo;
        }

        public async Task UpdateAsync(YearInfo YearInfo)
        {
            _context.Entry(YearInfo).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var YearInfo = await _context.YearInfos.FindAsync(id);
            if (YearInfo != null)
            {
                _context.YearInfos.Remove(YearInfo);
                await _context.SaveChangesAsync();
            }
        }
    }
}
