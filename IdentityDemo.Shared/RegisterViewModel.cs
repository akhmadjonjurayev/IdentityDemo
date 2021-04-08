using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IdentityDemo.Shared
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(60,MinimumLength =5)]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(60, MinimumLength = 5)]
        public string Password { get; set; }
        [Required]
        [StringLength(60, MinimumLength = 5)]
        public string ConfirmPassword { get; set; }
    }
}
