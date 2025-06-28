using System.ComponentModel.DataAnnotations;

namespace Cinema.Models.ViewModel
{
    public class ForgetPasswordVM
    {

        [Required]
        public string UserNameOrEmail { get; set; } = null!;
    }
}
