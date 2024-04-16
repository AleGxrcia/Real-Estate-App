using Microsoft.EntityFrameworkCore;
using RealEstateApp.Core.Domain.Common;
using RealEstateApp.Core.Domain.Entities;

namespace RealEstateApp.Infrastructure.Persistence.Contexts
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

        public DbSet<Property> Properties { get; set; }
        public DbSet<PropertyType> PropertyTypes { get; set; }
        public DbSet<PropertyImage> PropertyImages { get; set; }
        public DbSet<Improvement> Improvements { get; set; }
        public DbSet<ImprovementProperty> ImprovementProperties { get; set; }
        public DbSet<FavoriteProperty> FavoriteProperties { get; set; }
        public DbSet<SaleType> SaleTypes { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableBaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.Created = DateTime.Now;
                        entry.Entity.CreatedBy = "DefaultAppUser";
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModified = DateTime.Now;
                        entry.Entity.LastModifiedBy = "DefaultAppUser";
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //FLUENT API

            #region tables
            modelBuilder.Entity<Property>().ToTable("Properties");
            modelBuilder.Entity<PropertyType>().ToTable("PropertyTypes");
            modelBuilder.Entity<PropertyImage>().ToTable("PropertyImages");
            modelBuilder.Entity<Improvement>().ToTable("Improvements");
            modelBuilder.Entity<ImprovementProperty>().ToTable("ImprovementProperties");
            modelBuilder.Entity<FavoriteProperty>().ToTable("FavoriteProperties");
            modelBuilder.Entity<SaleType>().ToTable("SaleTypes");
            #endregion

            #region "primary keys"
            modelBuilder.Entity<Property>().HasKey(p => p.Id);
            modelBuilder.Entity<PropertyType>().HasKey(p => p.Id);
            modelBuilder.Entity<Improvement>().HasKey(p => p.Id);
            modelBuilder.Entity<FavoriteProperty>().HasKey(p => p.Id);
            modelBuilder.Entity<SaleType>().HasKey(p => p.Id);
            modelBuilder.Entity<ImprovementProperty>().HasKey(ip => new { ip.ImprovementId, ip.PropertyId });
            modelBuilder.Entity<PropertyImage>().HasKey(p => p.id);
            #endregion

            #region relationships
            modelBuilder.Entity<Property>()
                .HasMany<PropertyImage>(p => p.Images)
                .WithOne()
                .HasForeignKey(pi => pi.PropertyId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PropertyType>()
                .HasMany<Property>(pt => pt.Properties)
                .WithOne(p => p.PropertyType)
                .HasForeignKey(p => p.PropertyTypeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Property>()
                .HasMany<FavoriteProperty>(p => p.FavoriteProperties)
                .WithOne(fp => fp.Property)
                .HasForeignKey(fp => fp.PropertyId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SaleType>()
                .HasMany<Property>(st => st.Properties)
                .WithOne(p => p.SaleType)
                .HasForeignKey(p => p.SaleTypeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Property>()
                .HasMany<ImprovementProperty>(p => p.ImprovementProperties)
                .WithOne(ip => ip.Property)
                .HasForeignKey(ip => ip.PropertyId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Improvement>()
                .HasMany<ImprovementProperty>(i => i.ImprovementProperties)
                .WithOne(ip => ip.Improvement)
                .HasForeignKey(ip => ip.ImprovementId)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion

            #region "property configuration"

            #region Properties
            modelBuilder.Entity<Property>()
                .HasIndex(p => p.Code)
                .IsUnique();

            modelBuilder.Entity<Property>().
                Property(p => p.Price)
                .IsRequired();

            modelBuilder.Entity<Property>().
                Property(p => p.LandSize)
                .IsRequired();

            modelBuilder.Entity<Property>().
                Property(p => p.NumberOfBathrooms)
                .IsRequired();

            modelBuilder.Entity<Property>().
                Property(p => p.Description)
                .IsRequired();

            modelBuilder.Entity<Property>().
                Property(p => p.SaleTypeId)
                .IsRequired();

            modelBuilder.Entity<Property>().
                Property(p => p.PropertyTypeId)
                .IsRequired();
            #endregion

            #region "Property Types"
            modelBuilder.Entity<PropertyType>().
                Property(pt => pt.Name)
                .IsRequired();

            modelBuilder.Entity<PropertyType>().
                Property(pt => pt.Description)
                .IsRequired();
            #endregion

            #region "Property Images"
            modelBuilder.Entity<PropertyImage>().
                Property(pi => pi.ImageUrl)
                .IsRequired();

            modelBuilder.Entity<PropertyImage>().
                Property(pi => pi.PropertyId)
                .IsRequired();
            #endregion

            #region "Improvements"
            modelBuilder.Entity<Improvement>().
                Property(i => i.Name)
                .IsRequired();

            modelBuilder.Entity<Improvement>().
                Property(i => i.Description)
                .IsRequired();
            #endregion

            #region "Favorite Properties"
            modelBuilder.Entity<FavoriteProperty>().
                Property(fp => fp.ClientId)
                .IsRequired();

            modelBuilder.Entity<FavoriteProperty>().
                Property(fp => fp.PropertyId)
                .IsRequired();
            #endregion

            #region "Sale Types"
            modelBuilder.Entity<SaleType>().
                Property(st => st.Name)
                .IsRequired();

            modelBuilder.Entity<SaleType>().
                Property(st => st.Description)
                .IsRequired();
            #endregion

            #region "Improvement Properties"
            modelBuilder.Entity<ImprovementProperty>().
                Property(st => st.ImprovementId)
                .IsRequired();

            modelBuilder.Entity<ImprovementProperty>().
                Property(st => st.PropertyId)
                .IsRequired();
            #endregion

            #endregion
        }
    }
}
