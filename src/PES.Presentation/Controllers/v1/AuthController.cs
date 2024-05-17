using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PES.Application.IService;
using PES.Domain.DTOs.User;

namespace PES.Presentation.Controllers.V1
{
    public class AuthController : DefaultController
    {

        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            await _userService.Register(request);
            return Ok("Register Successfully");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request) { 
            string accessToken = await _userService.Login(request);
            return Ok(accessToken);

        }
    }
}