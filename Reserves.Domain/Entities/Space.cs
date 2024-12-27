namespace Reserves.Domain.Entities
{
    public class Space
    {
        public int SpaceId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}
