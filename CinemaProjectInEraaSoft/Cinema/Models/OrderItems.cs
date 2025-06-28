using Microsoft.EntityFrameworkCore;

namespace Cinema.Models
{
    [PrimaryKey(nameof(FilmId), nameof(OrderId))]

    public class OrderItems
    {
        public int FilmId { get; set; }
        public Film Film { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public decimal ItemPrice { get; set; }
        public int Count { get; set; }
    }
}
