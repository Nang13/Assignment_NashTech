using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PES.Application.IService;
using PES.Domain.DTOs.User;

namespace PES.Presentation.Controllers.v1
{
    public class UserController : DefaultController
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {

            return Ok(_userService.GetUsers());
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            return Ok(await _userService.GetUser(id));
        }


        [HttpPatch("{userId}")]
        public async Task<IActionResult> Update(string userId, UpdateUseRequest request)
        {
            return Ok(await _userService.UpdateUser(userId, request));
        }


        [HttpDelete("{userId}")]
        public async Task<IActionResult> Delete(string userId)
        {
            return Ok(await _userService.DisableUser(userId));
        }


        [HttpPost("{userId}/enable")]
        public async Task<IActionResult> EnableUser(string userId) { 
            return Ok(await _userService.EnableUser(userId));
        }

    }
}