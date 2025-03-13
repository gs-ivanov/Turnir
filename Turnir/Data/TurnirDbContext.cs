namespace Turnir.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class TurnirDbContext : IdentityDbContext
    {
        public TurnirDbContext(DbContextOptions<TurnirDbContext> options)
            : base(options)
        {
        }
    }
}
