using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Biluthyrning.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Biluthyrning.Models.Booking> Bookings { get; set; }
        public DbSet<Biluthyrning.Models.Car> Cars { get; set; }
        public DbSet<Biluthyrning.Models.Cartype> Cartypes { get; set; }
    }
}
