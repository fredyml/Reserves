using Reserves.Application.Interfaces;
using Reserves.Domain.Entities;
using Reserves.Domain.Exceptions;

namespace Reserves.Application.Services
{
    public class ReservationService
    {
        private readonly IRepository<Reservation> _reservationRepository;

        public ReservationService(
            IRepository<Reservation> reservationRepository)
        {
            _reservationRepository = reservationRepository;
        }

        public async Task ValidateReservationAsync(Reservation reservation)
        {
            reservation.Validate();

            var errors = new List<string>();
            
            var overlappingSpaceReservations = await _reservationRepository.GetAllAsync(r =>
                r.SpaceId == reservation.SpaceId &&
                reservation.StartDate < r.EndDate && reservation.EndDate > r.StartDate);

            if (overlappingSpaceReservations.Any())
            {
                errors.Add("La reserva entra en conflicto con una reserva existente para el mismo espacio.");
            }
            
            var overlappingUserReservations = await _reservationRepository.GetAllAsync(r =>
                r.UserId == reservation.UserId &&
                reservation.StartDate < r.EndDate && reservation.EndDate > r.StartDate);

            if (overlappingUserReservations.Any())
            {
                errors.Add("El usuario ya tiene una reserva en el mismo período de tiempo.");
            }
            
            if (errors.Any())
            {
                throw new DataValidationException(string.Join(", ", errors));
            }
        }
    }
}
