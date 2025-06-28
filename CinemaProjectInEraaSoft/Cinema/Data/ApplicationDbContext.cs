using Cinema.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Data
{
    public class ApplicationDbContext:IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options) { }
        public ApplicationDbContext()
        {
            
        }

        public DbSet<Actor> Actors { get; set; }

        public DbSet<Cinemaa> Cinemas  { get; set; }
        public DbSet<Film> Films { get; set; }
        public DbSet<FilmActor> ActorFilmRelation { get; set; }
        public DbSet<Image> images { get; set; }
        public DbSet<ApplicationUserOTP> ApplicationUserOTPs { get; set; }
        public DbSet<Cart>  Carts { get; set; }   
        public DbSet<Order>   Orders { get; set; } 
        public DbSet<OrderItems>   OrderItems { get; set; }
        public DbSet<Favorite>   Favorites { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=CinamaData;Integrated Security=True;TrustServerCertificate=True");

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ===== علاقة Many-to-Many: Film <> Actor (بجدول FilmActor) =====
            modelBuilder.Entity<FilmActor>()
                .HasKey(a => new { a.FilmId, a.ActorId }); // مفتاح مركب

            modelBuilder.Entity<FilmActor>()
                .HasOne(e => e.Film)
                .WithMany(g => g.FilmActors)
                .HasForeignKey(a => a.FilmId);

            modelBuilder.Entity<FilmActor>()
                .HasOne(a => a.Actor)
                .WithMany(a => a.FilmActors)
                .HasForeignKey(a => a.ActorId);
            modelBuilder.Entity<Film>(entity =>
            {
                entity.Property(e => e.Rating)
                      .IsRequired(false); // ✨ يسمح بالقيمة null
            });
        }
      
        
    }

}

