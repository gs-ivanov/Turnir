namespace Turnir.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Turnir.Data.Models;

    public class TurnirDbContext : IdentityDbContext
    {
        public TurnirDbContext(DbContextOptions<TurnirDbContext> options)
            : base(options)
        {
        }

        public DbSet<Team> Teams { get; init; }

        public DbSet<Group> Groups { get; init; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .Entity<Team>()
                .HasOne(t => t.Group)
                .WithMany(t => t.Teams)
                .HasForeignKey(t => t.GroupId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);
        }
    }
}
