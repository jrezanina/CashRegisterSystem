using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PokladniSystem.Domain.Entities;
using PokladniSystem.Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokladniSystem.Infrastructure.Database
{
    public class CRSDbContext : IdentityDbContext<User, Role, int>
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Supply> Supplies { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<OrderVATPrice> OrderVATPrices { get; set; }
        public DbSet<VATRate> VATRates { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Company> Company { get; set; }

        public CRSDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions){ }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            DatabaseInit dbInit = new DatabaseInit();

            //modelBuilder.Entity<VATRate>().HasData(dbInit.GetDefaultVATRates());
            //modelBuilder.Entity<Category>().HasData(dbInit.GetDefaultCategories());
            //modelBuilder.Entity<Product>().HasData(dbInit.GetDefaultProducts());
            //modelBuilder.Entity<ProductCategory>().HasData(dbInit.GetDefaultProductCategories());
            //modelBuilder.Entity<Contact>().HasData(dbInit.GetUTBCompanyContact());
            //modelBuilder.Entity<Company>().HasData(dbInit.GetUTBCompanyInformations());
            //modelBuilder.Entity<Contact>().HasData(dbInit.GetFAIStoreContact());
            //modelBuilder.Entity<Store>().HasData(dbInit.GetFAIStoreInformations());

            modelBuilder.Entity<Contact>().HasData(dbInit.GetDefaultCompanyContact());
            modelBuilder.Entity<Company>().HasData(dbInit.GetDefaultCompanyInformations());
            modelBuilder.Entity<Order>().HasOne<User>(e => e.User as User);
        }
    }
}