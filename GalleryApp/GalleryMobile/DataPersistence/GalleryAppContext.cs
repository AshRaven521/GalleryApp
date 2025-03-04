using GalleryMobile.DataPersistence.Entities;
using GalleryMobile.UnsplashAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace GalleryMobile.DataPersistence
{
    public class GalleryAppContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UnsplashPhoto> Photos { get; set; }

        public GalleryAppContext()
        {
            // The SQLitePCL.Batteries_V2.Init() is needed in the constructor to initiate SQLite on iOS.
            SQLitePCL.Batteries_V2.Init();

            this.Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder
                .UseSqlite($"Filename={DataConstants.DataBasePath}");
        }
    }
}
