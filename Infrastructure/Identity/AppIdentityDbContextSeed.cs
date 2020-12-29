using System.Linq;
using System.Threading.Tasks;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUsersAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new AppUser
                {
                    DisplayName = "Joe",
                    Email = "joe@test.com",
                    UserName = "joe@test.com",
                    Address = new Address
                    {
                        FirstName = "Joe",
                        LastName = "Ng",
                        Street = "14, Jalan Ch 4, Cheras Hartamas",
                        City = "Cheras",
                        State = "Selangor",
                        Zipcode = "43200"
                    }
                };

                await userManager.CreateAsync(user, "Pa$$w0rd");
            }
        }
    }
}