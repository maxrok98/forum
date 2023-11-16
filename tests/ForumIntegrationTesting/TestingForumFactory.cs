using Forum;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Forum.DAL.Models;

namespace ForumIntegrationTesting
{
    public class TestingForumFactory<T> : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {

                var descriptor = services.SingleOrDefault(
                  d => d.ServiceType ==
                     typeof(DbContextOptions<ForumAppDbContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                var serviceProvider = new ServiceCollection()
                  .AddEntityFrameworkInMemoryDatabase()
                  .BuildServiceProvider();

                services.AddDbContext<ForumAppDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryEmployeeTest");
                    options.UseInternalServiceProvider(serviceProvider);
                });

                var sp = services.BuildServiceProvider();

                //using (var serviceScope = sp.CreateScope())
                //{
                //    var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                //    if (! roleManager.RoleExistsAsync("Admin").Result)
                //    {
                //        var adminRole = new IdentityRole("Admin");
                //        roleManager.CreateAsync(adminRole);
                //    }

                //    if (! roleManager.RoleExistsAsync("User").Result)
                //    {
                //        var userRole = new IdentityRole("User");
                //        roleManager.CreateAsync(userRole);
                //    }
                //}

                using (var scope = sp.CreateScope())
                {
                    using (var appContext = scope.ServiceProvider.GetRequiredService<ForumAppDbContext>())
                    {
                        try
                        {
                            appContext.Database.EnsureCreated();
                        }
                        catch (Exception ex)
                        {
                            //Log errors or do anything you think it's needed
                            throw;
                        }
                    }
                }
            
            });
        }

    }
}
