using Reserves.Application.Interfaces;
using Reserves.Domain.Entities;

namespace Reserves.Application.Services
{
    public class ReservationService
    {
        private readonly IRepository<Reservation> _reservationRepository;
        private readonly IRepository<Space> _spaceRepository;
        private readonly IRepository<User> _userRepository;

        public ReservationService(
            IRepository<Reservation> reservationRepository,
            IRepository<Space> spaceRepository,
            IRepository<User> userRepository)
        {
            _reservationRepository = reservationRepository;
            _spaceRepository = spaceRepository;
            _userRepository = userRepository;
        }

        public async Task ValidateReservationAsync(Reservation reservation)
        {
            reservation.Validate();

            var overlappingReservations = await _reservationRepository.GetAllAsync(r =>
                r.SpaceId == reservation.SpaceId &&
                ((reservation.StartDate >= r.StartDate && reservation.StartDate < r.EndDate) ||
                 (reservation.EndDate > r.StartDate && reservation.EndDate <= r.EndDate)));

            if (overlappingReservations.Any())
                throw new InvalidOperationException("Reservation conflicts with an existing reservation.");

            var userReservations = await _reservationRepository.GetAllAsync(r =>
                r.UserId == reservation.UserId &&
                ((reservation.StartDate >= r.StartDate && reservation.StartDate < r.EndDate) ||
                 (reservation.EndDate > r.StartDate && reservation.EndDate <= r.EndDate)));

            if (userReservations.Any())
                throw new InvalidOperationException("User already has a reservation in the same time frame.");
        }
    }
}
