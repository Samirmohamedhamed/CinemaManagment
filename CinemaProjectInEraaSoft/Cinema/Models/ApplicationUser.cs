using Microsoft.AspNetCore.Identity;

namespace Cinema.Models
{
    public class ApplicationUser: IdentityUser
    {
        public String? Addrress {  get; set; }
        public int Age { get; set; }
        public List<Favorite> Favorites { get; set; } = new List<Favorite>();


    }
}
