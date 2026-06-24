using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Recruitment.Domain.Entities;
using Recruitment.Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Recruitment.Infrastructure
{
    public class RecruitmentContext : IdentityDbContext<User>
    {
        public RecruitmentContext(DbContextOptions<RecruitmentContext> options) : base(options)
        {
        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<JobPosition> JobPositions { get; set; }
        public DbSet<JobAdvert> JobAdverts { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Interview> Interviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // JobAdvert i Company 
            modelBuilder.Entity<JobAdvert>()
                .HasOne(a => a.Company)
                .WithMany(c => c.JobAdverts)
                .HasForeignKey(a => a.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            // JobAdvert i JobPosition 
            modelBuilder.Entity<JobAdvert>()
                .HasOne(a => a.JobPosition)
                .WithMany(p => p.JobAdverts)
                .HasForeignKey(a => a.JobPositionId)
                .OnDelete(DeleteBehavior.Restrict);

            // Jedan korisnik moze samo jednom aplicirati na isti oglas
            modelBuilder.Entity<Application>()
                        .HasIndex(app => new { app.UserId, app.JobAdvertId })
                        .IsUnique();

            // Application i JobAdvert
            modelBuilder.Entity<Application>()
                .HasOne(app => app.JobAdvert)
                .WithMany(a => a.Applications)
                .HasForeignKey(app => app.JobAdvertId)
                .OnDelete(DeleteBehavior.Cascade);

            // Application i Interview 
            modelBuilder.Entity<Interview>()
                .HasOne(i => i.Application)
                .WithMany(app => app.Interviews)
                .HasForeignKey(i => i.ApplicationId)
                .OnDelete(DeleteBehavior.Cascade);

            // Application i Document
            modelBuilder.Entity<Application>()
                .HasMany(app => app.Documents)
                .WithMany(d => d.Applications);

            // Enumi kao tekst u bazi 
            modelBuilder.Entity<Application>()
                .Property(a => a.Status)
                .HasConversion<string>();

            modelBuilder.Entity<JobPosition>()
                .Property(p => p.Type)
                .HasConversion<string>();

            modelBuilder.Entity<Interview>()
                .Property(i => i.Type)
                .HasConversion<string>();

            modelBuilder.Entity<Interview>()
                .Property(i => i.Status)
                .HasConversion<string>();

            modelBuilder.Entity<Document>()
                .Property(d => d.DocumentType)
                .HasConversion<string>();
        }
    }
}
