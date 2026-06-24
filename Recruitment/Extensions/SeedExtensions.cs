using Microsoft.AspNetCore.Identity;
using Recruitment.Domain.Entities;
using Recruitment.Domain.Enums;
using Recruitment.Infrastructure;
using Recruitment.Infrastructure.Identity;

namespace Recruitment.API.Extensions
{
    public static class SeedExtensions
    {
        public static async Task SeedAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var sp = scope.ServiceProvider;

            var context = sp.GetRequiredService<RecruitmentContext>();
            var roleManager = sp.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = sp.GetRequiredService<UserManager<User>>();

            // 1. Role
            foreach (var role in new[] { "Admin", "Candidate" })
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            // 2. Admin nalog
            var adminEmail = "admin@recruitment.local";
            var admin = await userManager.FindByEmailAsync(adminEmail);
            if (admin == null)
            {
                admin = new User
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FirstName = "Admin",
                    LastName = "Admin",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(admin, "Admin123!");
                await userManager.AddToRoleAsync(admin, "Admin");
            }

            // 3. Firme i pozicije (samo ako baza prazna)
            if (!context.Companies.Any())
            {
                var company = new Company
                {
                    Name = "Tech Solutions",
                    Industry = "IT",
                    City = "Beograd",
                    WebSite = "https://techsolutions.rs"
                };
                context.Companies.Add(company);

                context.JobPositions.Add(new JobPosition
                {
                    Title = "Junior .NET Developer",
                    Description = "Razvoj backend aplikacija u .NET-u.",
                    Type = JobType.Internship,
                    Requirements = "C#, osnove SQL-a"
                });
                context.JobPositions.Add(new JobPosition
                {
                    Title = "Frontend Developer",
                    Description = "Razvoj korisnickih interfejsa.",
                    Type = JobType.FullTime,
                    Requirements = "JavaScript, React"
                });

                await context.SaveChangesAsync();
            }
        }
    }
}
