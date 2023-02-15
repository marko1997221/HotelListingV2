using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelListingV2.Data;
using AutoMapper;
using HotelListingV2.Interfejsi;
using HotelListingV2.Models.Country;
using HotelListingV2.Models.Hotels;
using System.Diagnostics.Metrics;

namespace HotelListingV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelsController : ControllerBase
    {

        private readonly IHotelInterface implementation;
        private readonly IMapper mapper;

        public HotelsController(IHotelInterface implementation, IMapper mapper)
        {
            this.implementation = implementation;
            this.mapper = mapper;
        }

        // GET: api/Hotels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetHotelsDto>>> GetHotels()
        {
            var lista = await implementation.GetAllAsync();
            if (lista == null)
            {
                return NotFound();
            }
            var countryDto = mapper.Map<List<GetHotelsDto>>(lista);
            return Ok(countryDto);
        }

        // GET: api/Hotels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GetHotelsDto>> GetHotel(int id)
        {
            var hotel = await implementation.GetAsync(id);
            if (hotel == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<GetHotelsDto>(hotel));
        }

        // PUT: api/Hotels/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHotel(int id, UpdateHotelsDto hotel)
        {
            var hotel1 = mapper.Map<Hotel>(hotel);


            try
            {
                await implementation.UpdateAsync(hotel1);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!(await HotelExists(id)))
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

        // POST: api/Hotels
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CreateHotelDto>> PostHotel(CreateHotelDto hotel)
        {
            var hotel1 = mapper.Map<Hotel>(hotel);
            if (await implementation.GetAllAsync() == null)
            {
                return Problem("Entity set 'HotelListingDbContext.Countries'  is null.");
            }
            await implementation.CreateAsync(hotel1);

            return CreatedAtAction("GetHotels", new { id = hotel1.Id }, hotel1);
        }

        // DELETE: api/Hotels/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            if (await implementation.GetAllAsync() == null)
            {
                return NotFound();
            }
            var hotel = await implementation.GetAsync(id);
            if (hotel == null)
            {
                return NotFound();
            }

            await implementation.DeleteAsync(id);

            return NoContent();
        }

        private Task<bool> HotelExists(int id)
        {
            return implementation.Exixts(id);
        }
    }
}
