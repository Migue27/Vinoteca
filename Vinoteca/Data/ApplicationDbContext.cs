using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Vinoteca.Models;

namespace Vinoteca.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext() : base()
        {

        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        public DbSet<Vino> Vinos { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<DenominacionOrigen> Origenes { get; set; }
        public DbSet<ApplicationUser> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<DenominacionOrigen>().ToTable("Origenes");
            builder.Entity<ApplicationUser>().ToTable("Usuarios");
            builder.Entity<ApplicationUser>().Property("Bodeguero").HasDefaultValue(false);
        }

    }
}
