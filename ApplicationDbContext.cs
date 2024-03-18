using LeaveManagement.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace LeaveManagement
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base("Default")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<LeaveEntity> Leaves { get; set; }
        public DbSet<UserTypeEntity> UserTypes { get; set; }
    }
}