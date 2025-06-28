using System.ComponentModel.DataAnnotations;

namespace Cinema.Models
{
    public class Cinemaa
    {
        public int Id { get; set; }
        //[Required]
        //[MinLength(3)]
        //[MaxLength(10)]
        public string Name { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public string? PhoneNumber { get; set; }


        public double? Rating { get; set; }


        public List<Film> CinemaFilms { get; set; } = new List<Film>();
    }
}
