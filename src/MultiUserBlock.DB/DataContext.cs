using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using MultiUserBlock.DB.Entitys;

namespace MultiUserBlock.DB
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        { }

        public DbSet<Mieter> Mieters { get; set; }
        //public DbSet<Person> Persons { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<RoleToUser> RoleToUsers { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<LayoutTheme> LayoutThemes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RoleToUser>().HasKey(t => new { t.UserId, t.RoleId });
            modelBuilder.Entity<RoleToUser>().HasOne(rtu => rtu.Role).WithMany(r => r.RoleToUsers).HasForeignKey(rtu => rtu.RoleId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<RoleToUser>().HasOne(rtu => rtu.User).WithMany(r => r.RoleToUsers).HasForeignKey(rtu => rtu.UserId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<LayoutTheme>().ToTable("LayoutTheme").HasKey(lt => lt.ThemeId);
            modelBuilder.Entity<User>().ToTable("User").HasOne(u => u.LayoutTheme);
            modelBuilder.Entity<Mieter>().ToTable("Mieter");

        }
    }
}
