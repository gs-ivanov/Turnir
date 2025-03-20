namespace Turnir.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Turnir.Data.Models;

    public class TurnirDbContext : IdentityDbContext<User>
    {
        public TurnirDbContext(DbContextOptions<TurnirDbContext> options)
            : base(options)
        {
        }

        public DbSet<Team> Teams { get; init; }

        public DbSet<Group> Groups { get; init; }

        public DbSet<Trener> Treners { get; init; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .Entity<Team>()
                .HasOne(t => t.Group)
                .WithMany(t => t.Teams)
                .HasForeignKey(t => t.GroupId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Team>()
                .HasOne(t => t.Trener)
                .WithMany(d => d.Teams)
                .HasForeignKey(t => t.TrenerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Trener>()
                .HasOne<User>()
                .WithOne()
                .HasForeignKey<Trener>(t => t.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);
        }
    }
}
