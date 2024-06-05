using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PES.Application.Helper.EmailHandler;
using PES.Application.Helper.ErrorHandler;
using PES.Application.IService;
using PES.Application.Utilities;
using PES.Domain.DTOs.ProductDTO;
using PES.Domain.DTOs.User;
using PES.Domain.Entities.Model;
using PES.Infrastructure.Common;
using StackExchange.Redis;
using static PES.Domain.DTOs.User.RegisterRequest;


namespace PES.Application.Service
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory;
        private readonly IAuthorizationService _authorizationService;
        private readonly IDatabase _database;
        private readonly IClaimsService _claimService;

        private readonly IConfiguration _configuration;

        public UserService(UserManager<ApplicationUser> userManager, IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory, IAuthorizationService authorizationService, IConfiguration configuration, IDatabase database, IClaimsService claimsService)
        {
            _authorizationService = authorizationService;
            _userManager = userManager;
            _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
            _configuration = configuration;
            _database = database;
            _claimService = claimsService;
        }
        public async Task<string> ForgetPassword(string Email)
        {
            SendMailHandler sendMail = new SendMailHandler(_configuration);
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == Email);
            if (user is null)
            {
                throw new HttpStatusCodeException(System.Net.HttpStatusCode.BadRequest, "Your Account have been locked");
            }
            int otp = await GenerateOTP(user.UserName);
            Task.Run(() => sendMail.SendMail(OTP: otp.ToString(), UserName: user.UserName));
            return otp.ToString();
        }


        public async Task<int> GenerateOTP(string userName)
        {
            Random random = new Random();
            int randomNumber = random.Next(100000, 999999);
            await _database.StringSetAndGetAsync(userName + "_OTPKey", randomNumber);
            return randomNumber;

        }
        public async Task<bool> ChangePassword(ChangePasswordRequest request, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            string resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            if (await CheckOTPMatch(user.UserName, request.OTP))
            {
                await _userManager.ResetPasswordAsync(user, resetToken, request.Password);
                return true;
            }
            return false;
        }

        public async Task<bool> CheckOTPMatch(string userName, string OTP)
        {
            string otpRedis = await _database.StringGetAsync(userName + "_OTPKey");
            if (otpRedis == OTP)
            {
                return true;
            }
            return false;

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
            var result = request.Filter.Count > 0 ? [] : users;
            if (request.Filter?.Count > 0)
            {
                foreach (var filter in request.Filter)
                {
                    result = result.Union(FilterUtilities.SelectItems(users, filter.Key, filter.Value)).ToList();
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

        public async Task<AuthDTO> Login(LoginRequest loginRequest)
        {
            var user = await _userManager.FindByEmailAsync(loginRequest.Email);
            if (user.LockoutEnabled) throw new HttpStatusCodeException(System.Net.HttpStatusCode.BadRequest, "Your Account have been locked");
            bool checkPassword = await _userManager.CheckPasswordAsync(user, loginRequest.Password);
            var roles = await _userManager.GetRolesAsync(user);
            var principal = await _userClaimsPrincipalFactory.CreateAsync(user);
          

            //var newToken = await _userManager.Login(user, "Jwt Bearer", "JWT");
            // var result = await _authorizationService.AuthorizeAsync(principal, user.UserName);
            string secretKeyConfig = _configuration["JWTSecretKey:SecretKey"];
            string jwt = GenerateJWT(user, secretKeyConfig,roles);
            //  Console.WriteLine(newToken);
            return new AuthDTO(user.Id, user.UserName, user.Email, user.LockoutEnabled,roles.First(), new UserToken(jwt, "comsuonbitalon"));
        }


        private static string GenerateJWT(ApplicationUser user, string secretKeyConfig,IList<string> userRoles)
        {

            DateTime secretKeyDatetime = DateTime.UtcNow;
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKeyConfig));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
                new("UserId", user.Id.ToString()),
                new(ClaimTypes.Name, user.UserName),
                new (ClaimTypes.Role,userRoles.First()),
            };
            // claims.Add( new Claim((await _userManager.GetRolesAsync(user)).Select(x => new Claim(ClaimTypes.Role, x))));



            var token = new JwtSecurityToken(
                issuer: secretKeyConfig,
                audience: secretKeyConfig,
                claims: claims,
                expires: secretKeyDatetime.AddMinutes(86400),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public async Task<AuthDTO> Register(RegisterRequest request)
        {
            ApplicationUser user = new ApplicationUser
            {
                UserName = request.UserName,
                Email = request.Email,
                LockoutEnabled = false,
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Errors.Any())
            {
                throw new HttpStatusCodeException(System.Net.HttpStatusCode.BadRequest, string.Join(", ", result.Errors.Select(x => "Code " + x.Code + " Description" + x.Description)));
            }
            var roles = await _userManager.GetRolesAsync(user);

            string secretKeyConfig = _configuration["JWTSecretKey:SecretKey"];
            string jwt = GenerateJWT(user, secretKeyConfig,roles);
            var userData = await _userManager.Users.Where(x => x.Email == request.Email).FirstOrDefaultAsync();
            return new AuthDTO(userData.Id, userData.UserName, userData.Email, userData.LockoutEnabled, roles.First(),new UserToken(jwt, "comsuonbitalon"));

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


        public async Task<bool> ChangePasswordRequest(ChangePasswordByUserRequest request)
        {
            var user = await _userManager.Users.Where(x => x.Id == _claimService.GetCurrentUserId).FirstOrDefaultAsync();
            if (user is null)
            {
                throw new HttpStatusCodeException(System.Net.HttpStatusCode.BadRequest, "Not Found this User");
            }
            var result = await _userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
            if (result.Errors.Any())
            {
                throw new HttpStatusCodeException(System.Net.HttpStatusCode.BadRequest, string.Join(", ", result.Errors.Select(x => "Code " + x.Code + " Description" + x.Description)));
            }
            return true;
        }
    }
}