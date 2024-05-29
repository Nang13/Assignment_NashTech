using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PES.Domain.DTOs.Product;
using PES.Domain.DTOs.User;
using PES.Domain.Entities.Model;
using PES.Infrastructure.Common;
using static PES.Domain.DTOs.User.RegisterRequest;

namespace PES.Application.IService
{
    public interface IUserService
    {
        public Task<bool> Register(RegisterRequest request);

        public Task<string> Login(LoginRequest loginRequest);

        public Task<string> ForgetPassword(string Email);

        public Task<ApplicationUser> UpdateUser(string userid, UpdateUseRequest request);
        public Task<Pagination<UserDTO>> GetUsers(GetProductRequest request);
        public Task<ApplicationUser> GetUser(string UserId);

        public Task<ApplicationUser> DisableUser(string UserId);
        public Task<ApplicationUser> EnableUser(string UserId);
    }
}