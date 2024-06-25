using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Project_PRN231_API.Models
{
    public partial class Project_PRN231_SE1710Context : DbContext
    {
        public Project_PRN231_SE1710Context()
        {
        }

        public Project_PRN231_SE1710Context(DbContextOptions<Project_PRN231_SE1710Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Task> Tasks { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UserTemp> UserTemps { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder()
                              .SetBasePath(Directory.GetCurrentDirectory())
                              .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfigurationRoot configuration = builder.Build();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("MyConnect"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");

                entity.Property(e => e.CategoryName).HasMaxLength(100);
            });

            modelBuilder.Entity<Task>(entity =>
            {
                entity.ToTable("_Tasks");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("smalldatetime")
                    .HasColumnName("Created_at");

                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.DueDate)
                    .HasColumnType("date")
                    .HasColumnName("Due_date");

                entity.Property(e => e.Title).HasMaxLength(250);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("smalldatetime")
                    .HasColumnName("Updated_at");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Tasks)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Tasks_Category");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Tasks)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Tasks_Users");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.CreatedAt)
                    .HasColumnType("date")
                    .HasColumnName("Created_at");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UserTemp>(entity =>
            {
                entity.ToTable("User_Temp");

                entity.Property(e => e.Email)
                    .HasMaxLength(250)
                    .IsFixedLength();

                entity.Property(e => e.ExpiresAt).HasColumnType("smalldatetime");

                entity.Property(e => e.Password)
                    .HasMaxLength(100)
                    .IsFixedLength();

                entity.Property(e => e.Token)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Username).HasMaxLength(100);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
