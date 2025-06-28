namespace Cinema.Models
{
    public class Image
    {
  
            public int Id { get; set; }
            public string ImageUrl { get; set; }
            public Film Film { get; set; }
            public int FilmId { get; set; }
        }
    }

