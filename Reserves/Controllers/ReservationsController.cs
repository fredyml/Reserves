using Microsoft.AspNetCore.Mvc;
using Reserves.Application.Interfaces;
using Reserves.Application.Services;
using Reserves.Domain.Entities;

namespace Reserves.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly IRepository<Reservation> _reservationRepository;
        private readonly ReservationService _reservationService;

        public ReservationsController(IRepository<Reservation> reservationRepository, ReservationService reservationService)
        {
            _reservationRepository = reservationRepository;
            _reservationService = reservationService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Reservation reservation)
        {
            try
            {
                await _reservationService.ValidateReservationAsync(reservation);
                await _reservationRepository.AddAsync(reservation);
                await _reservationRepository.SaveChangesAsync();
                return Ok(new { reservation.ReservationId, Status = "Created" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var reservation = await _reservationRepository.GetByIdAsync(id);
            if (reservation == null) return NotFound();

            await _reservationRepository.DeleteAsync(reservation);
            await _reservationRepository.SaveChangesAsync();
            return Ok(new { reservation.ReservationId, Status = "Cancelled" });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int? spaceId, [FromQuery] int? userId, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            var reservations = await _reservationRepository.GetAllAsync(r =>
                (!spaceId.HasValue || r.SpaceId == spaceId) &&
                (!userId.HasValue || r.UserId == userId) &&
                (!startDate.HasValue || r.StartDate >= startDate) &&
                (!endDate.HasValue || r.EndDate <= endDate));

            return Ok(reservations);
        }
    }
}
