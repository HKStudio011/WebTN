using System;
using Microsoft.EntityFrameworkCore;

namespace WebTN.Models
{
    public class MyBlogContext : DbContext
    {
        public DbSet<Article> Articles { get; set; }
        
        public MyBlogContext(DbContextOptions<MyBlogContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}