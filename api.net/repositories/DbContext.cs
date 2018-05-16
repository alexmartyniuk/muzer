using System.Data.Entity;

namespace MuzerAPI
{
    using MuzerAPI.Models;

    public class DbContext : System.Data.Entity.DbContext
    {
        public DbContext() : base(@"Server=localhost\SQLEXPRESS;Database=Muzer;Trusted_Connection=True;")
        {
        }

        public DbSet<ArtistModel> Artists { get; set; }
        public DbSet<AlbumModel> Albums { get; set; }
        public DbSet<TrackModel> Tracks { get; set; }
        public DbSet<TrackDataModel> TrackDatas { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ArtistModel>().HasKey(a => a.Id);
            modelBuilder.Entity<ArtistModel>().Property(a => a.Source).IsRequired();
            modelBuilder.Entity<ArtistModel>().Property(a => a.SourceId).IsRequired();
            modelBuilder.Entity<ArtistModel>().Property(a => a.Name).IsRequired();
            
            modelBuilder.Entity<AlbumModel>().HasKey(a => a.Id);
            modelBuilder.Entity<AlbumModel>().Property(a => a.Source).IsRequired();
            modelBuilder.Entity<AlbumModel>().Property(a => a.SourceId).IsRequired();
            modelBuilder.Entity<AlbumModel>().Property(a => a.Title).IsRequired();

            modelBuilder.Entity<TrackModel>().HasKey(t => t.Id);
            modelBuilder.Entity<TrackModel>().Property(t => t.Title).IsRequired();
            
            modelBuilder.Entity<TrackDataModel>().HasKey(t => t.Id);
            modelBuilder.Entity<TrackDataModel>().Property(t => t.Source).IsRequired();
            modelBuilder.Entity<TrackDataModel>().Property(t => t.SourceUrl).IsRequired();
        }
    }
}
