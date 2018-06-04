using System.Data.Entity;
using System.Linq;

namespace MuzerAPI
{
    using MuzerAPI.Models;

    public class DatabaseContext : DbContext
    {
        public DatabaseContext() : base(@"Server=localhost\SQLEXPRESS;Database=Muzer;Trusted_Connection=True;")
        {
            Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
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
            modelBuilder.Entity<AlbumModel>()
                .HasRequired(a => a.Artist)
                .WithMany(a => a.Albums)
                .HasForeignKey(al => al.ArtistId);

            modelBuilder.Entity<TrackModel>().HasKey(t => t.Id);
            modelBuilder.Entity<TrackModel>().Property(t => t.Title).IsRequired();
            modelBuilder.Entity<TrackModel>()
                .HasRequired(a => a.Album)
                .WithMany(a => a.Tracks)
                .HasForeignKey(al => al.AlbumId);

            modelBuilder.Entity<TrackDataModel>().HasKey(t => t.Id);
            modelBuilder.Entity<TrackDataModel>().Property(t => t.Source).IsRequired();
            modelBuilder.Entity<TrackDataModel>().Property(t => t.SourceUrl).IsRequired();
            modelBuilder.Entity<TrackDataModel>()
                .HasRequired(a => a.Track)
                .WithMany(a => a.TrackDatas)
                .HasForeignKey(al => al.TrackId);
        }
    }
}
