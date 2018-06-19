using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace UserAuthentication.Models
{
    public class UserLogin
    {
        //email validation
        [Display(Name = "Email")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email required")]
        public string Email { get; set; }

        //password validation
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password required")]
        public string Password { get; set; }
    }
}