using Microsoft.AspNetCore.Mvc;
using Reserves.Application.Dtos;
using Reserves.Application.Interfaces;
using Reserves.Application.Services;
using Reserves.Domain.Entities;

namespace Reserves.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationManagementController : ControllerBase
    {
        private readonly IRepository<Reservation> _reservationRepository;
        private readonly IRepository<Space> _spaceRepository;
        private readonly IRepository<User> _userRepository;
        private readonly ReservationService _reservationService;

        public ReservationManagementController(IRepository<Reservation> reservationRepository, IRepository<Space> spaceRepository, IRepository<User> userRepository,ReservationService reservationService)
        {
            _reservationRepository = reservationRepository;
            _reservationService = reservationService;
            _spaceRepository = spaceRepository;
            _userRepository = userRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ReservationCreateDto reservationDto)
        {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                
                var reservation = new Reservation
                {
                    SpaceId = reservationDto.SpaceId,
                    UserId = reservationDto.UserId,
                    StartDate = reservationDto.StartDate,
                    EndDate = reservationDto.EndDate
                };

                await _reservationService.ValidateReservationAsync(reservation);
                await _reservationRepository.AddAsync(reservation);
                await _reservationRepository.SaveChangesAsync();
                return Ok(new { reservation.ReservationId, Status = "Reserva creada" });
            
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var reservation = await _reservationRepository.GetByIdAsync(id);
            if (reservation == null) return NotFound();

            await _reservationRepository.DeleteAsync(reservation);
            await _reservationRepository.SaveChangesAsync();
            return Ok(new { reservation.ReservationId, Status = "Reserva cancelada" });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int? spaceId, [FromQuery] int? userId, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            var reservations = await _reservationRepository.GetAllAsync(r =>
            (!spaceId.HasValue || r.SpaceId == spaceId) &&
            (!userId.HasValue || r.UserId == userId) &&
            (!startDate.HasValue || r.StartDate >= startDate) &&
            (!endDate.HasValue || r.EndDate <= endDate),
             includeRelated: true);

            var result = reservations.Select(r => new
            {
                r.ReservationId,
                r.SpaceId,
                SpaceName = r.Space.Name,
                SpaceDescription = r.Space.Description,
                r.UserId,
                UserName = r.User.Name,
                UserEmail = r.User.Email,
                r.StartDate,
                r.EndDate
            });

            return Ok(result);
        }

        [HttpGet("available-spaces")]
        public async Task<IActionResult> GetAvailableSpaces()
        {
            var spaces = await _spaceRepository.GetAllAsync();

            if (spaces == null || !spaces.Any())
            {
                return NotFound("No se encontraron espacios.");
            }
           
            var availableSpaces = spaces.Select(s => new
            {
                s.SpaceId,
                s.Name,
                s.Description
            });

            return Ok(availableSpaces);
        }

        [HttpGet("available-users")]
        public async Task<IActionResult> GetAvailableUsers()
        {
            var users = await _userRepository.GetAllAsync();

            if (users == null || !users.Any())
            {
                return NotFound("No se encontraron usuarios.");
            }
           
            var availableUsers = users.Select(u => new
            {
                u.UserId,
                u.Name,
                u.Email
            });

            return Ok(availableUsers);
        }

    }
}
