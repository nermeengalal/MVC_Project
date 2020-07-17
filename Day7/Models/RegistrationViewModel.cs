using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Day7.Models
{
    public class RegistrationViewModel
    {
        [Required]
        public string Username { get; set; }
       
        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }
       
        [DataType(DataType.Password)]
        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        [Required]
        public string Adress { get; set; }
        
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}