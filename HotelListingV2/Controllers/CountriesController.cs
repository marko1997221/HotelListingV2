using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelListingV2.Data;
using HotelListingV2.Implementacija;
using AutoMapper;
using HotelListingV2.Interfejsi;
using HotelListingV2.Models.Country;

namespace HotelListingV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        
        private readonly ICountyInterface implementation;
        private readonly IMapper mapper;

        public CountriesController(ICountyInterface implementation, IMapper mapper )
        {
            
            this.implementation = implementation;
            this.mapper = mapper;
        }

        // GET: api/Countries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetCountryDto>>> GetCountries()
        {
            var lista = await implementation.GetAllAsync();
          if (lista==null)
          {
              return NotFound();
          }
            var countryDto = mapper.Map<List<GetCountryDto>>(lista);
            return Ok(countryDto);
            
        }

        // GET: api/Countries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GetCountryDto>> GetCountry(int id)
        {
          var country=await implementation.GetAsync(id);
            if (country == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<GetCountryDto>(country));
        }

        // PUT: api/Countries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCountry(int id, UpdateCountryDto country)
        {
            var country1 = mapper.Map<Country>(country);
           

            try
            {
                await implementation.UpdateAsync(country1);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!(await CountryExists(id)))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Countries
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CreateCountryDto>> PostCountry(CreateCountryDto country)
        {
            var country1 = mapper.Map<Country>(country);
          if (await implementation.GetAllAsync()==null)
          {
              return Problem("Entity set 'HotelListingDbContext.Countries'  is null.");
          }
            await implementation.CreateAsync(country1);

            return CreatedAtAction("GetCountry", new { id = country1.Id }, country1);
        }

        // DELETE: api/Countries/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            if (await implementation.GetAllAsync() == null)
            {
                return NotFound();
            }
            var country = await implementation.GetAsync(id);
            if (country == null)
            {
                return NotFound();
            }

            await implementation.DeleteAsync(id);

            return NoContent();
        }

        private Task<bool> CountryExists(int id)
        {
            return implementation.Exixts(id);
        }
    }
}
