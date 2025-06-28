using System.ComponentModel.DataAnnotations;

namespace Cinema.Models.ViewModel
{
    public class ResendEmailVM
    {

        [Required]
        public string UserNameOrEmail { get; set; } = null!;
       
    }
}
