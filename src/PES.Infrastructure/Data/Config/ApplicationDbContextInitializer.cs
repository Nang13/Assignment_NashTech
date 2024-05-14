using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PES.Domain.Entities.Model;
using PES.Domain.Enum;


namespace PES.Infrastructure.Data.Config
{
    public static class InitialiserExtensions
    {
        public static async Task InitialiseDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitializer>();

            await initialiser.InitialiseAsync();

            await initialiser.SeedAsync();
        }
    }

    
    public class ApplicationDbContextInitializer
    {
        private readonly ILogger<ApplicationDbContextInitializer> _logger;
        private readonly PlantManagementContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;


        public ApplicationDbContextInitializer(ILogger<ApplicationDbContextInitializer> logger, PlantManagementContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }


        public async Task InitialiseAsync()
        {
            try
            {
                await _context.Database.MigrateAsync();
            }
            catch (System.Exception ex)
            {

                _logger.LogError(ex, "An error occurred while initialising the database.");
                throw;
            }
        }


        public async Task SeedAsync()
        {
            try
            {
                await TrySeedAsync();
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "An error occurred while seeding the database.");
                throw;
            }
        }

        public async Task TrySeedAsync()
        {
            var administratorsRole = new IdentityRole(Role.Administrator);
            var userRole = new IdentityRole(Role.User);

            await _roleManager.CreateAsync(administratorsRole);
            await _roleManager.CreateAsync(userRole);

            var administrator = new ApplicationUser { UserName = "administrator@localhost", Email = "administrator@localhost" };
            await _userManager.CreateAsync(administrator, "Administrator1!");
            if (!string.IsNullOrWhiteSpace(administratorsRole.Name))
            {
                await _userManager.AddToRolesAsync(administrator, new[] { administratorsRole.Name });
            }

            await _context.SaveChangesAsync();
        }
    }
}