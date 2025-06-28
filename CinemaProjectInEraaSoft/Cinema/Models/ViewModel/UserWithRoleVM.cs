using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Cinema.Models.ViewModel
{
    public class UserWithRoleVM
    {
        public ApplicationUser ApplicationUser { get; set; }
        [ValidateNever]
        public List<IdentityRole> identityRoles { get; set; }
        [ValidateNever]

        public string CurrentRole {  get; set; }
    }
}
