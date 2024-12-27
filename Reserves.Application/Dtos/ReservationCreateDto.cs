using System.ComponentModel.DataAnnotations;

namespace Reserves.Application.Dtos
{
    public class ReservationCreateDto
    {
        [Required]
        public int SpaceId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }
    }

}
