namespace Reserves.Domain.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}
