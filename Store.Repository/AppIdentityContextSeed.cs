using Microsoft.AspNetCore.Identity;
using Store.Data.Entities.IdentityEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository
{
    public class AppIdentityContextSeed
    {
        public static async Task SeedUserAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new AppUser
                {
                    DisplayName = "karim mohamed",
                    Email = "karim@gmail.com",
                    UserName = "karimmohamed",
                    Address = new Address
                    {
                        FirstName = "karim",
                        LastName = "mohamed",
                        City = "alex1",
                        State = "alex2",
                        Street = "7",
                        ZipCode = "1234"
                    }
                };
                await userManager.CreateAsync(user, "Password123!");
            }
        }
    }
}
