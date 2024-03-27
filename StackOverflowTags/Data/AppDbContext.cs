using Microsoft.EntityFrameworkCore;
using StackOverflowTags.Models;

namespace StackOverflowTags.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Tag> Tag { get; set; }
        public DbSet<Tags> Tags { get; set; }
    }
}
