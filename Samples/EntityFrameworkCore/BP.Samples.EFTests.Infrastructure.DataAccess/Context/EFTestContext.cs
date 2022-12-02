using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using BP.Samples.EFTests.Infrastructure.DataAccess.Entities;

namespace BP.Samples.EFTests.Infrastructure.DataAccess.Context
{
    public partial class EFTestContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public EFTestContext()
        {
        }

        public EFTestContext(DbContextOptions<EFTestContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Address> Addresses { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>(entity =>
            {
                entity.ToTable("Address");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.Idcard).HasColumnName("IDCard");

                entity.HasMany(d => d.Addresses)
                    .WithMany(p => p.Users)
                    .UsingEntity<Dictionary<string, object>>(
                        "UserAddress",
                        l => l.HasOne<Address>().WithMany().HasForeignKey("AddressId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_UserAddress_Address"),
                        r => r.HasOne<User>().WithMany().HasForeignKey("UserId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_UserAddress_User"),
                        j =>
                        {
                            j.HasKey("UserId", "AddressId");

                            j.ToTable("UserAddress");

                            j.IndexerProperty<Guid>("UserId").HasColumnName("UserID");

                            j.IndexerProperty<Guid>("AddressId").HasColumnName("AddressID");
                        });
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
