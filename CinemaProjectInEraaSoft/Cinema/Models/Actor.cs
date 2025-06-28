namespace Cinema.Models
{
    public class Actor
    {
        public int Id { get; set; }

        public string FullName { get; set; }=string.Empty;

        public string? Nickname { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string Nationality { get; set; } = string.Empty;

        public string? Biography { get; set; }

        public string? ImageUrl { get; set; }

        public string? Gender { get; set; }

        public List<FilmActor> FilmActors { get; set; }=new List<FilmActor>();
    }

}
