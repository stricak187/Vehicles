using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Vehicles.Models;

namespace Vehicles.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Vehicles.Models.VehicleMake> VehicleMake { get; set; }
        public DbSet<Vehicles.Models.VehicleModel> VehicleModel { get; set; }
    }
}
