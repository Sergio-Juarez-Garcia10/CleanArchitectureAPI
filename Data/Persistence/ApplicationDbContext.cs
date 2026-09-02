using Domain;
using Domain.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Persistence
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {

        }

        public DbSet<PersonEntity> Persons { get; set; }

        public DbSet<VisitEntity> Visits { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PersonEntity>(entity =>
            {
                entity.ToTable("Persons");

                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).IsRequired().ValueGeneratedOnAdd();

                entity.Property(e => e.Code).IsRequired().HasMaxLength(20);
                entity.HasIndex(e => e.Code).IsUnique();

                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(50);

                entity.Property(e => e.LastName).IsRequired().HasMaxLength(50);

                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);

                entity.Property(e => e.PhoneNumber).IsRequired().HasMaxLength(15);

                entity.Ignore(e => e.FullName);

                entity.Property<DateTime>("CreateAt").IsRequired().HasDefaultValueSql("GETUTCDATE()");

                entity.Property<DateTime>("UpdateAt").IsRequired().HasDefaultValueSql("GETUTCDATE()");

            });

            modelBuilder.Entity<VisitEntity>(entity =>
            {
                entity.ToTable("Visits");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).IsRequired().ValueGeneratedOnAdd();

                entity.Property(e => e.PersonId).IsRequired();

                entity.Property(e => e.EntryTime).IsRequired();

                entity.Property(e => e.ExitTime).IsRequired(false);

                entity.Ignore(e => e.isActive);
                entity.Ignore(e => e.Duration);
                
                entity.HasOne(e => e.Person)
                      .WithMany()
                      .HasForeignKey(e => e.PersonId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(e => e.PersonId);
                entity.HasIndex(e => e.EntryTime);
                entity.HasIndex(e => new { e.PersonId, e.EntryTime });

                entity.Property<DateTime>("CreateAt").IsRequired().HasDefaultValueSql("GETUTCDATE()");
                entity.Property<DateTime>("UpdateAt").IsRequired().HasDefaultValueSql("GETUTCDATE()");
            });
        }
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();

            try
            {
                return await base.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException ex) when (IsForeignKeyViolation(ex))
            {
                throw new EntityHasRelatedRecordsException(innerException: ex);
            }
        }

        private static bool IsForeignKeyViolation(DbUpdateException ex)
        {
            // 547 = "The DELETE/UPDATE statement conflicted with the FOREIGN KEY constraint"
            return ex.InnerException is SqlException sqlEx && sqlEx.Number == 547;
        }
        private void UpdateTimestamps()
        {
            var entries = ChangeTracker.Entries().Where(e=> e.State == EntityState.Modified);

            foreach (var entry in entries) 
            { 
                if (entry.Metadata.FindProperty("UpdateAt") != null)
                {
                    entry.Property("UpdateAt").CurrentValue = DateTime.UtcNow;
                }              
            }
        }
    }
}
