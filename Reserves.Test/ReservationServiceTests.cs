using Moq;
using Reserves.Application.Interfaces;
using Reserves.Application.Services;
using Reserves.Domain.Entities;
using Reserves.Domain.Exceptions;

namespace Reserves.Tests.Services
{
    public class ReservationServiceTests
    {
        private readonly Mock<IRepository<Reservation>> _mockReservationRepo;
        private readonly ReservationService _reservationService;

        public ReservationServiceTests()
        {
            _mockReservationRepo = new Mock<IRepository<Reservation>>();
            _reservationService = new ReservationService(_mockReservationRepo.Object);
        }

        [Fact]
        public async Task ValidateReservationAsync_ShouldThrowArgumentException_WhenStartDateIsAfterEndDate()
        {
            // Arrange
            var reservation = new Reservation
            {
                SpaceId = 1,
                UserId = 1,
                StartDate = DateTime.Now.AddHours(2),
                EndDate = DateTime.Now.AddHours(1)  
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(async () =>
                await _reservationService.ValidateReservationAsync(reservation));
            Assert.Equal("Fecha de inicio debe ser anterior a Fecha Final.", exception.Message);
        }

        [Fact]
        public async Task ValidateReservationAsync_ShouldThrowArgumentException_WhenDurationIsLessThan30Minutes()
        {
            // Arrange
            var reservation = new Reservation
            {
                SpaceId = 1,
                UserId = 1,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMinutes(15)  
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(async () =>
                await _reservationService.ValidateReservationAsync(reservation));
            Assert.Equal("Las reservas deben ser entre 30 minutos y 8 horas.", exception.Message);
        }

        [Fact]
        public async Task ValidateReservationAsync_ShouldThrowArgumentException_WhenDurationExceeds8Hours()
        {
            // Arrange
            var reservation = new Reservation
            {
                SpaceId = 1,
                UserId = 1,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddHours(9)  
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(async () =>
                await _reservationService.ValidateReservationAsync(reservation));
            Assert.Equal("Las reservas deben ser entre 30 minutos y 8 horas.", exception.Message);
        }

        [Fact]
        public async Task ValidateReservationAsync_ShouldThrowInvalidOperationException_WhenUserHasOverlappingReservationsAndSpaceIsOccupied()
        {
            // Arrange
            _mockReservationRepo.Setup(repo => repo.GetAllAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Reservation, bool>>>(), false))
                .ReturnsAsync([new Reservation { SpaceId = 1, UserId = 2, StartDate = DateTime.Now.AddHours(1), EndDate = DateTime.Now.AddHours(3) }]);

            var newReservation = new Reservation
            {
                SpaceId = 1,
                UserId = 2,
                StartDate = DateTime.Now.AddHours(1),
                EndDate = DateTime.Now.AddHours(3)
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<DataValidationException>(() =>
                _reservationService.ValidateReservationAsync(newReservation));


            Assert.Contains("La reserva entra en conflicto con una reserva existente para el mismo espacio.", exception.Message);
            Assert.Contains("El usuario ya tiene una reserva en el mismo período de tiempo.", exception.Message);
        }
    }
}
