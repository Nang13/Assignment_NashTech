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


        public struct ChangePasswordRequest : IValidatableObject
        {
            [Required]
            public string Password { get; set; }
            [Required]
            public string ConfirmPassword { get; set;}
            [Required]
            public string OTP { get; set; }

            public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
            {
               if(Password != ConfirmPassword)
                {
                    yield return new ValidationResult("Password not match with ConfirmPassword");
                }
            }
        }


        public struct ChangePasswordByUserRequest : IValidatableObject
        {
            public string OldPassword { get; set; }

            public string NewPassword { get; set; } 

            public string CofirmNewPassword { get; set; }
            public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
            {
                if(CofirmNewPassword != NewPassword)
                {
                    yield return new ValidationResult("Password not match with ConfirmPassword");
                }
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