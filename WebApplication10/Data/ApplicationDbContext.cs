using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication10.Models;

namespace WebApplication10.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
           


            builder.Entity<ApplicationUser>().ToTable("Users");
            builder.Entity<Instruction>().ToTable("Instructions");
            builder.Entity<InstructionStep>().ToTable("InstuctionSteps");
            builder.Entity<Category>().ToTable("Category");
            builder.Entity<Comment>().ToTable("Commetns");
            builder.Entity<Achievements>().ToTable("Achievements");

        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Instruction> Instructions { get; set; }
        public DbSet<InstructionStep> InstructionSteps { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Achievements> Achievements { get; set; }
    }
}
