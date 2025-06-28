namespace Cinema.Models
{
    public class Film
    {
        public int Id { get; set; }

        // عنوان الفيلم
        public string Title { get; set; }

        // تاريخ الإصدار
        public DateTime ReleaseDate { get; set; }

        // مدة الفيلم (بالدقائق)
        public int DurationInMinutes { get; set; }
        public int CountOfTicket {  get; set; }
        public decimal PriceOfTicket {  get; set; }

        // النوع (Action, Comedy, Drama, etc.)
        public string Genre { get; set; }

        // تصنيف الفيلم (مثلاً PG, R, etc.)
        public string? Rating { get; set; }

        // ملخص الفيلم
        public string? Description { get; set; }

        // رابط الصورة الدعائية (بوستر)
        public string? PosterUrl { get; set; }

      
        // لغة الفيلم (مثلاً: English, Arabic, etc.)
        public string Language { get; set; }

        // الميزانية (لو حابب تضيفها)
        public decimal? Budget { get; set; }

        public int CinemaaId { get; set; }

        // علاقة Many-to-Many مع السينمات
        public Cinemaa Cinemaa { get; set; }
        // علاقة Many-to-Many مع الممثلين
        public List<FilmActor> FilmActors { get; set; }  = new List<FilmActor>();
        public List<Image>  images { get; set; }  = new List<Image>();
        public List<Favorite> Favorites { get; set; } = new List<Favorite>();

    }
}
