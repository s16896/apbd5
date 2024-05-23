using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using apbd5.Models.DTO.Request;
using apbd5.Repositories.Interfaces;

namespace apbd5.Controllers
{
    [Route("api/trip")]
    [ApiController]
    public class TripController : ControllerBase
    {
        private readonly ITripDbRepository repository;

        public TripController(ITripDbRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetTrips()
        {
            try
            {
                var trips = await repository.GetTripsAsync();
                return Ok(trips);
            }
            catch (Exception)
            {
                return NoContent();
            }
        }

        [HttpPost("{idTrip}/clients")]
        public async Task<IActionResult> AddTripToClient([FromRoute] int idTrip, [FromBody] AddTripToClientRequestDto dto)
        {
            try
            {
                await repository.AddTripToClientAsync(idTrip, dto);
                return Ok("Your request processed successfully!");
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }
    }
}