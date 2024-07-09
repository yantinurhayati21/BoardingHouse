using BoardingHouse.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace BoardingHouse.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<DataKost> KostData { get; set; }
        public DbSet<Admin> Admin { get; set; }
        public DbSet<DataBooking> BookingDates { get; set; }
    }
}
