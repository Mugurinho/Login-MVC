using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace UserAuthentication.Models
{
    [MetadataType(typeof(UserMetadata))]
    public partial class User
    {

    }

    public partial class UserMetadata
    {
        //user email validation
        [Display(Name = "Email")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        //user password validation
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password required")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Minimum 6 characters required")]
        public string Password { get; set; }
    }
}