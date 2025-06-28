namespace Cinema.Models.ViewModel
{
    public class RelatedFilmsVM
    {

      public Film film { get; set; }
      public List<Film> Relatedfilms { get; set; }
      public List<Film> SameActorFilms { get; set; }



    }
}
