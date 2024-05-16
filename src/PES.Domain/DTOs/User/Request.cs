using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PES.Domain.DTOs.User
{

    public class RegisterRequest : IValidatableObject
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
        public string? Email { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Password != ConfirmPassword)
            {
                yield return new ValidationResult("Password not match with ConfirmPassword");
            }
        }

    }



    public class UpdateUseRequest { 
        public string? UserName { get; set;}    

        public string? Email {get; set;}    


        public string? PhoneNumber {get; set;}
    }

    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}