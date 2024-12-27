namespace Reserves.Domain.Entities
{
    public class Reservation
    {
        public int ReservationId { get; set; }
        public int SpaceId { get; set; }
        public Space Space { get; set; } = null!;
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public void Validate()
        {
            if (StartDate >= EndDate)
                throw new ArgumentException("Fecha de inicio debe ser anterior a Fecha Final.");

            var duration = EndDate - StartDate;
            if (duration.TotalMinutes < 30 || duration.TotalHours > 8)
                throw new ArgumentException("Las reservas deben ser entre 30 minutos y 8 horas.");
        }
    }
}
