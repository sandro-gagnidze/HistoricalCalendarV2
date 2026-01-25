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
    public class YearInfoController : ControllerBase
    {
        private readonly IYearInfoRepository _repository;

        public YearInfoController(IYearInfoRepository repository)
        {
            _repository = repository;
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var YearInfos = await _repository.GetAllAsync();
            return Ok(YearInfos);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var YearInfo = await _repository.GetByIdAsync(id);
            if (YearInfo == null)
            {
                return NotFound();
            }
            return Ok(YearInfo);
        }

        [AllowAnonymous]
        [HttpGet("by-year")]
        public async Task<IActionResult> GetByYear([FromQuery] int year)
        {
            var YearInfo = await _repository.GetByYearAsync(year);
            if (YearInfo == null)
            {
                return NotFound();
            }
            return Ok(YearInfo);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(YearInfo yearInfo)
        {
            var createdYearInfo = await _repository.CreateAsync(yearInfo);
            return CreatedAtAction(nameof(GetById), new { id = createdYearInfo.Id }, createdYearInfo);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, YearInfo yearInfo)
        {
            if (id != yearInfo.Id)
            {
                return BadRequest();
            }

            await _repository.UpdateAsync(yearInfo);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }
}
