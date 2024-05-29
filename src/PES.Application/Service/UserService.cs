using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PES.Application.IService;
using PES.Application.Utilities;
using PES.Domain.DTOs.Product;
using PES.Domain.DTOs.User;
using PES.Domain.Entities.Model;
using PES.Infrastructure.Common;


namespace PES.Application.Service
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory;
        private readonly IAuthorizationService _authorizationService;

        private readonly IConfiguration _configuration;

        public UserService(UserManager<ApplicationUser> userManager, IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory, IAuthorizationService authorizationService, IConfiguration configuration)
        {
            _authorizationService = authorizationService;
            _userManager = userManager;
            _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
            _configuration = configuration;
        }
        public Task<string> ForgetPassword(string Email)
        {
            throw new NotImplementedException();
        }

        public async Task<ApplicationUser> GetUser(string UserId)
        {
            var user = await _userManager.Users.Where(x => x.Id == UserId).FirstOrDefaultAsync();
            return user;
        }

        public async Task<Pagination<UserDTO>> GetUsers(GetProductRequest request)
        {
            var users = _userManager.Users.Select(x => new UserDTO(x.Id, x.UserName, x.Email, x.LockoutEnabled)).ToList();
            request.Filter!.Remove("pageSize");
            request.Filter!.Remove("pageNumber");
            var result = request.Filter.Count > 0 ? [] :users;
            if (request.Filter?.Count > 0)
            {
                foreach (var filter in request.Filter)
                {
                   result = result.Union(FilterUtilities.SelectItems(users,filter.Key,filter.Value)).ToList();
                }
            }
            return new Pagination<UserDTO>
            {
                PageIndex = request.PageNumber,
                PageSize = request.PageSize,
                TotalItemsCount = users.Count(),
                Items = PaginatedList<UserDTO>.Create(
                       source: result.AsQueryable(),
                       pageIndex: request.PageNumber,
                       pageSize: request.PageSize)


            };
        }

        public async Task<string> Login(LoginRequest loginRequest)
        {
            var user = await _userManager.FindByEmailAsync(loginRequest.Email);
            if (user.LockoutEnabled) throw new Exception("Your account have been locked");
            bool checkPassword = await _userManager.CheckPasswordAsync(user, loginRequest.Password);
            var principal = await _userClaimsPrincipalFactory.CreateAsync(user);
            string secretKeyConfig = _configuration["JWTSecretKey:SecretKey"];
            DateTime secretKeyDatetime = DateTime.UtcNow;
            //var newToken = await _userManager.Login(user, "Jwt Bearer", "JWT");
            // var result = await _authorizationService.AuthorizeAsync(principal, user.UserName);
            string jwt = GenerateJWT(user, secretKeyConfig, secretKeyDatetime);
            //  Console.WriteLine(newToken);
            return jwt;
        }


        private static string GenerateJWT(ApplicationUser user, string secretKey, DateTime now)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
                new("UserId", user.Id.ToString()),
                new(ClaimTypes.Name, user.UserName)
            };
            // claims.Add( new Claim((await _userManager.GetRolesAsync(user)).Select(x => new Claim(ClaimTypes.Role, x))));



            var token = new JwtSecurityToken(
                issuer: secretKey,
                audience: secretKey,
                claims: claims,
                expires: now.AddMinutes(86400),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public async Task<bool> Register(RegisterRequest request)
        {
            ApplicationUser user = new ApplicationUser
            {
                UserName = request.UserName,
                Email = request.Email,
                LockoutEnabled = false,
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            return result.Succeeded;
        }

        public async Task<ApplicationUser> UpdateUser(string userid, UpdateUseRequest request)
        {
            var user = await _userManager.Users.Where(x => x.Id == userid).FirstOrDefaultAsync() ?? throw new Exception("");

            user.PhoneNumber = request.PhoneNumber;
            user.Email = request.Email;
            user.UserName = request.UserName;

            await _userManager.UpdateAsync(user);

            return await _userManager.Users.Where(x => x.Id == userid).FirstOrDefaultAsync();
        }

        public async Task<ApplicationUser> DisableUser(string UserId)
        {
            var user = await _userManager.Users.Where(x => x.Id.Equals(UserId)).FirstOrDefaultAsync() ?? throw new Exception("");
            user.LockoutEnabled = true;
            await _userManager.UpdateAsync(user);

            return await _userManager.Users.Where(x => x.Id.Equals(UserId)).FirstOrDefaultAsync();

        }

        public async Task<ApplicationUser> EnableUser(string UserId)
        {
            var user = await _userManager.Users.Where(x => x.Id.Equals(UserId)).FirstOrDefaultAsync() ?? throw new Exception("");
            user.LockoutEnabled = false;
            await _userManager.UpdateAsync(user);

            return await _userManager.Users.Where(x => x.Id.Equals(UserId)).FirstOrDefaultAsync();
        }
    }
}