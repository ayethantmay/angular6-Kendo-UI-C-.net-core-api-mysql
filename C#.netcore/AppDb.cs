using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace dotnet2_1WebAPI
{
    public class AppDb : DbContext
    {
        public string _connectionString = "";
        public AppDb(DbContextOptions<AppDb> options) : base(options)
        {
            //context.Database.EnsureCreated();   //uncomment to create new database 
            var appsettingbuilder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
            var Configuration = appsettingbuilder.Build();
            _connectionString = Configuration.GetConnectionString("DefaultConnection");
        }

        public DbSet<Inventory> Inventory { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

             modelBuilder.Entity<Inventory>()
                .HasKey(c => new { c.inventory_id });

        }


    }
}