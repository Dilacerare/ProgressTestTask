using System;
using Microsoft.EntityFrameworkCore;
using ProgressTestTask.Domain.Entity;

namespace ProgressTestTask.DAL
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Patient> Patients { get; set; }

        public DbSet<Visit> Visits { get; set; }

        public DbSet<MKB10> MKB10 { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            modelBuilder.Entity<Patient>(builder =>
            {
                builder.ToTable("Patients").HasKey(x => x.PatientId);

                builder.HasData(new Patient[]
                {
                    new Patient
                    {
                        PatientId = Guid.NewGuid(),
                        LastName = "Чумаков",
                        FirstName = "Михаил",
                        Patronymic = "Денисович",
                        DateOfBirth = new DateTime(1995, 7, 20),
                        Phone = "+7 (800) 555-35-35"
                    },

                    new Patient
                    {
                        PatientId = Guid.NewGuid(),
                        LastName = "Захарова",
                        FirstName = "Мария",
                        Patronymic = "Артёмовна",
                        DateOfBirth = new DateTime(1997, 9, 12),
                        Phone = "+7 (800) 333-55-55"
                    },
                });
            });

        }
    }
}
