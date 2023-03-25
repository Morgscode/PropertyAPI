using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PropertyApi.Models
{
    public class ApplicationRegistration
    {
        public ApplicationRegistration(string email, string password, string passwordConfirm, string firstName, string lastName)
        {
            Email = email;
            Password = password;
            PasswordConfirm = passwordConfirm;
            FirstName = firstName;
            LastName = lastName;
        }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
    }
}
