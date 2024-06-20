using FptJobBack.Models;
using Microsoft.EntityFrameworkCore;

namespace FptJobBack.Data
{
    public class SeedData
    {


        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                if (context.Users.Any())
                {
                    return;   // DB has been seeded
                }

                context.Users.AddRange(
                    new Admin
                    {
                        Username = "admin",
                        Password = "admin123", // Hash mật khẩu trong thực tế
                        Email = "admin@example.com",
                        DateCreated = DateTime.Now,
                        Role = "Admin"
                    },
                    new Company
                    {
                        Username = "company",
                        Password = "company123", // Hash mật khẩu trong thực tế
                        Email = "company@example.com",
                        DateCreated = DateTime.Now,
                        Role = "Company",
                        CompanyName = "Example Company",
                        CompanyAddress = "123 Street",
                        ContactNumber = "1234567890",
                        Website = "https://example.com"
                    },
                    new Users
                    {
                        Username = "user",
                        Password = "user123", // Hash mật khẩu trong thực tế
                        Email = "user@example.com",
                        DateCreated = DateTime.Now,
                        Role = "User"
                    }
                );

                context.SaveChanges();
            }
        }
    }
}
