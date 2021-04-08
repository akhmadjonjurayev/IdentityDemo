using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IdentityDemo.Shared
{
    public class LoginViewModel
    {
        [Required]
        [StringLength(60)]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(60,MinimumLength =5)]
        public string Password { get; set; }
    }
}
