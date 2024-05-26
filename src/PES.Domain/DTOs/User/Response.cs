using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PES.Domain.DTOs.User
{
    public class Response
    {
        
    }


    public record UserDTO(string UserId, string Name, string Email,bool IsInactive);
}