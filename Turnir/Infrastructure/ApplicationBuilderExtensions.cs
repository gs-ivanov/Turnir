﻿namespace Turnir.Infrastructure
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using System.Linq;
    using Turnir.Data;
    using Turnir.Data.Models;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder PrepareDatabase(
    this IApplicationBuilder app)
        {
            using var scopedServices = app.ApplicationServices.CreateScope();

            var data = scopedServices.ServiceProvider.GetService<TurnirDbContext>();

            data.Database.Migrate();

            SeedGroups(data);

            return app;
        }

        private static void SeedGroups(TurnirDbContext data)
        {
            if (data.Groups.Any())
            {
                return;
            }

            data.Groups.AddRange(new[]
            {
                new Group { Name = "Mini" },
                new Group { Name = "Economy" },
                new Group { Name = "Midsize" },
                new Group { Name = "Large" },
                new Group { Name = "SUV" },
                new Group { Name = "Vans" },
                new Group { Name = "Luxury" },
            });

            data.SaveChanges();
        }
    }
}
