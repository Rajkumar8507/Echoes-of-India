using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Tourist_places.Models
{
    public class TouristContext : DbContext
    {
        public TouristContext() : base()
        {
   
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<TouristPlaceType>().ToTable("TouristPlaceType");
            modelBuilder.Entity<TouristPlace>().ToTable("TouristPlace");
            modelBuilder.Entity<users>().ToTable("Users_Table");
        }
        public DbSet<TouristPlaceType> touristPlaceTypes { get; set; }
        public DbSet<TouristPlace> touristPlaces { get; set; }

        public DbSet<users> users_c { get; set; }
    }

}