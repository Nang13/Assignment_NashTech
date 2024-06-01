using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PES.Application.IService;
using PES.Domain.DTOs.ProductDTO;
using PES.Domain.DTOs.User;

namespace PES.Presentation.Controllers.V1
{
    [ApiController]
    public class UserController : DefaultController
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] Dictionary<string, string> filter, [FromQuery] int pageNumber = 0, [FromQuery] int pageSize = 10)
        {

            return Ok( await _userService.GetUsers(new GetProductRequest { Filter = filter, PageNumber = pageNumber, PageSize = pageSize }));
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