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
using Microsoft.AspNetCore.Authorization;
using HotelListingV2.Exceptions;

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
                throw new NotFoundException(nameof(GetCountries), null);
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
                throw new NotFoundException(nameof(GetCountry), id);
            }

            return Ok(mapper.Map<GetCountryDto>(country));
        }

        // PUT: api/Countries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
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
                    throw new NotFoundException(nameof(PutCountry), id);
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
        [Authorize]
        public async Task<ActionResult<CreateCountryDto>> PostCountry(CreateCountryDto country)
        {
            var country1 = mapper.Map<Country>(country);
          if (await implementation.GetAllAsync()==null)
          {
              return Problem("Entity set 'HotelListingDbContext.Countries'  is null.");
          }
            var country2=await implementation.CreateAsync(country1);

            return CreatedAtAction("GetCountry", new { id = country2.Id }, country2);
        }

        // DELETE: api/Countries/5
        [HttpDelete("{id}")]
        [Authorize(Roles ="Administrator")]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            if (await implementation.GetAllAsync() == null)
            {
                throw new NotFoundException(nameof(DeleteCountry), id);
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
