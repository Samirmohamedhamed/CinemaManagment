namespace Cinema.Models
{
    public class FilmActor
    {
        
            public int FilmId { get; set; }  // Foreign Key (Film)
            public Film Film { get; set; }

            public int ActorId { get; set; }  // Foreign Key (Actor)
            public Actor Actor { get; set; }

                      
        

    }
}
