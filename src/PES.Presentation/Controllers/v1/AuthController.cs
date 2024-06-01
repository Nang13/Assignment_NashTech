using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PES.Application.IService;
using PES.Domain.DTOs.User;
using static PES.Domain.DTOs.User.RegisterRequest;

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
          var response =  await _userService.Register(request);
            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request) { 
            AuthDTO response = await _userService.Login(request);
            return Ok(response);

        }

        [HttpPost("{email}/forgetpassword")]
        public async Task<IActionResult> ForgetPasswrod(string email)
        {
            var response = await _userService.ForgetPassword(email);
            return Ok(new
            {
                OTP  = response
            });
        }


        [HttpPost("{email}/changepassword")]
        public async Task<IActionResult> ChangPassword(string email, [FromBody] ChangePasswordRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(await _userService.ChangePassword(request, email))
            {
                return Ok(new
                {
                    message = "Change password Successfully"
                }) ;
            }
            //var response = await _userService.ForgetPassword(email);
            //return Ok(new
            //{
            //    OTP = response
            //});


            return BadRequest(new
            {
                message = "Change password Unsuccessfully"
            });

        }
        [HttpPost("{email}/cofirmemail")]
        public async Task<IActionResult> ConfirmEmail(string email)
        {
            var response = await _userService.ForgetPassword(email);
            return Ok(response);
        }
    }
}