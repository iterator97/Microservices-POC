using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using PlatformService.Models;

namespace PlatformService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app,bool isProd)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(), isProd);
            }
        }

        private static void SeedData(AppDbContext context, bool isProd)
        {
            if (isProd)
            {
                Console.WriteLine("---- Database Migration ----");
                try
                {
                context.Database.Migrate();
                }
                catch (Exception e)
                {

                    Console.WriteLine($"---- Could not run migration: {e.Message} ----");
                }
            }

            if (!context.Platfroms.Any())
            {
                Console.WriteLine("---- Seeding data ----");

                context.AddRange(
                    new Platform() { Name = "Dot Net", Publisher = "Microsoft", Cost = "Free" },
                    new Platform() { Name = "SQL Server Express", Publisher = "Microsoft", Cost = "Free" },
                    new Platform() { Name = "Kubernetes", Publisher = "Cloud Native Computing Foundation", Cost = "Free" }
                                );

                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("---- Already have data ----");
            }
        }
    }
}
