using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AssetRepo.Models
{
    public partial class AssetRepoContext : DbContext
    {
        public AssetRepoContext()
        {
        }

        public AssetRepoContext(DbContextOptions<AssetRepoContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Asset> Assets { get; set; } = null!;
        public virtual DbSet<AssetSold> AssetSolds { get; set; } = null!;
        public virtual DbSet<File> Files { get; set; } = null!;
        public virtual DbSet<Folder> Folders { get; set; } = null!;
        public virtual DbSet<Project> Projects { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
                String ConnectionStr = config.GetConnectionString("DBConnection");

                optionsBuilder.UseSqlServer(ConnectionStr);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Asset>(entity =>
            {
                entity.Property(e => e.AssetId).HasColumnName("AssetID");

                entity.Property(e => e.AssetName).HasMaxLength(255);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<AssetSold>(entity =>
            {
                entity.ToTable("AssetSold");

                entity.HasIndex(e => e.AssetId, "UQ__AssetSol__43492373B95D686A")
                    .IsUnique();

                entity.Property(e => e.AssetSoldId).HasColumnName("AssetSoldID");

                entity.Property(e => e.AssetId).HasColumnName("AssetID");

                entity.Property(e => e.SalePrice).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.Asset)
                    .WithOne(p => p.AssetSold)
                    .HasForeignKey<AssetSold>(d => d.AssetId)
                    .HasConstraintName("FK__AssetSold__Asset__4316F928");
            });

            modelBuilder.Entity<File>(entity =>
            {
                entity.Property(e => e.FileId).HasColumnName("FileID");

                entity.Property(e => e.AssetId).HasColumnName("AssetID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.FileName).HasMaxLength(255);

                entity.Property(e => e.FolderId).HasColumnName("FolderID");

                entity.Property(e => e.OriginalFileName).HasMaxLength(255);

                entity.Property(e => e.TypeFile).HasMaxLength(50);

                entity.HasOne(d => d.Asset)
                    .WithMany(p => p.Files)
                    .HasForeignKey(d => d.AssetId)
                    .HasConstraintName("FK__Files__AssetID__3F466844");

                entity.HasOne(d => d.Folder)
                    .WithMany(p => p.Files)
                    .HasForeignKey(d => d.FolderId)
                    .HasConstraintName("FK__Files__FolderID__3E52440B");
            });

            modelBuilder.Entity<Folder>(entity =>
            {
                entity.Property(e => e.FolderId).HasColumnName("FolderID");

                entity.Property(e => e.AssetId).HasColumnName("AssetID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.FolderName).HasMaxLength(255);

                entity.Property(e => e.ParentFolderId).HasColumnName("ParentFolderID");

                entity.HasOne(d => d.Asset)
                    .WithMany(p => p.Folders)
                    .HasForeignKey(d => d.AssetId)
                    .HasConstraintName("FK__Folders__AssetID__3B75D760");
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.Property(e => e.ProjectId).HasColumnName("ProjectID");

                entity.Property(e => e.AssetId).HasColumnName("AssetID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ProjectName).HasMaxLength(255);

                entity.HasOne(d => d.Asset)
                    .WithMany(p => p.Projects)
                    .HasForeignKey(d => d.AssetId)
                    .HasConstraintName("FK__Projects__AssetI__38996AB5");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
