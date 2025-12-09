using System.ComponentModel.DataAnnotations;

namespace Hoshiko.Web.Models
{
    public class RegisterViewModel
    {
        [Required]
        public string UserName {get;set;} = null!;

        [Required]
        public string FirstName {get;set;} = null!;

        [Required]
        public string LastName {get;set;} = null!;

        [Required, DataType(DataType.Password)]
        public string Password {get;set;} = null!;

        [Required, DataType(DataType.Password), Compare("Password")]
        public string ConfirmPassword {get;set;} = null!;
    }
}