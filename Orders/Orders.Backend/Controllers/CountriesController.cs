using Microsoft.AspNetCore.Mvc;
using Orders.Backend.UnitOfWork.Implementation;
using Orders.Backend.UnitOfWork.Interfaces;
using Orders.Shared.DTOs;
using Orders.Shared.Entities;

namespace Orders.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CountriesController : GenericController<Country>
    {
        private readonly ICountriesUnitOfWork _countriesUnitOfWork;

        public CountriesController(IGenericUnitOfWork<Country> unitOfWork, ICountriesUnitOfWork countriesUnitOfWork) : base(unitOfWork)
        {
            _countriesUnitOfWork = countriesUnitOfWork;
        }

        [HttpGet("full")]
        public override async Task<IActionResult> GetAsync()
        {
            var response = await _countriesUnitOfWork.GetAsync();
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest();
        }

        public override async Task<IActionResult> GetAsync(PaginationDTO pagination)
        {
            var response = await _countriesUnitOfWork.GetAsync(pagination);
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest();
        }

        [HttpGet("{id}")]
        public override async Task<IActionResult> GetAsync(int id)
        {
            var response = await _countriesUnitOfWork.GetAsync(id);
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return NotFound(response.Messagge);
        }

        [HttpGet("totalPages")]
        public override async Task<IActionResult> GetPagesAsync([FromQuery] PaginationDTO pagination)
        {
            var action = await _countriesUnitOfWork.GetTotalPagesAsync(pagination);
            if (action.WasSuccess)
            {
                return Ok(action.Result);
            }
            return BadRequest();
        }

        ////VERSION DADA DE BAJA EN VIDEOS 10 - 11

        //private readonly DataContext _context;
        //public CountriesController(DataContext context)
        //{
        //    _context = context;
        //}
        //[HttpGet]
        //public async Task<IActionResult> GetAsync()
        //{
        //    return Ok(await _context.Countries.AsNoTracking().ToListAsync());
        //}

        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetAsync(int id)
        //{
        //    var country = await _context.Countries.FindAsync(id);
        //    if (country == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(country);
        //}

        //////version 1
        ////[HttpPost]
        ////public IActionResult Post (Country country)
        ////{
        ////    _context.Add(country);
        ////    _context.SaveChanges();
        ////    return Ok();
        ////}

        ////version 2
        //[HttpPost]
        //public async Task <IActionResult> PostAsync(Country country)
        //{
        //    _context.Add(country);
        //    await _context.SaveChangesAsync();
        //    return Ok(country);
        //}

        ////version 3
        ////[HttpPost]
        ////public async Task<IActionResult> PostAsync(Country country)
        ////{
        ////    _context.Add(country);
        ////    await _context.SaveChangesAsync();
        ////    return Ok();
        ////}

        //[HttpPut]
        //public async Task<IActionResult> PutAsync(Country country)
        //{
        //    _context.Update(country);
        //    await _context.SaveChangesAsync();
        //    return NoContent();
        //}

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteAsync(int id)
        //{
        //    var country = await _context.Countries.FindAsync(id);
        //    if (country == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Remove(country);
        //    await _context.SaveChangesAsync();
        //    return NoContent();
        //}
    }
}