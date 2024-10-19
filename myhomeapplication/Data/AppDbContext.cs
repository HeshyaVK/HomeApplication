using Microsoft.EntityFrameworkCore;
using myhomeapplication.Model;
using System.Collections.Generic;

namespace myhomeapplication.Data
{
    public class AppDbContext: DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<NewBooking> NewBookings { get; set; }

        public DbSet<Setting> Settings { get; set; }

        public DbSet<StaticPage> StaticPages { get; set; }
        public DbSet<Testimonal> Testimonals { get; set; }

        public DbSet<Status> Statuses { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
}
