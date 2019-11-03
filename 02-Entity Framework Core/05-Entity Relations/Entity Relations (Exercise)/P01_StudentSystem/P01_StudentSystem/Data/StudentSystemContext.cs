using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using P01_StudentSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace P01_StudentSystem.Data
{
    public class StudentSystemContext : DbContext
    {
        

        public DbSet<Student> Students { get; set; }

        public DbSet<Course> Courses { get; set; }

        public DbSet<Resource> Resources { get; set; }

        public DbSet<Homework> HomeworkSubmissions { get; set; }

        public DbSet<StudentCourse> StudentCourses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.DefaultConnection);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(k => k.StudentId);

                entity.Property(p => p.Name)
                .HasMaxLength(100)
                .IsUnicode(true)
                .IsRequired(true);

                entity.Property(p => p.PhoneNumber)
                .HasMaxLength(10)
                .IsUnicode(false);

                entity.Property(p => p.RegisteredOn)
                .IsRequired(true);

                entity.Property(p => p.Birthday)
                .IsRequired(false);
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.HasKey(k => k.CourseId);

                entity.Property(p => p.Name)
                .HasMaxLength(80)
                .IsUnicode(true)
                .IsRequired(true);

                entity.Property(p => p.Description)
                .IsUnicode(true)
                .IsRequired(false);

                entity.Property(p => p.StartDate)
                .IsRequired(true);

                entity.Property(p => p.EndDate)
                .IsRequired(true);

                entity.Property(p => p.Price)
                .IsRequired(true);
            });

            modelBuilder.Entity<Resource>(entity =>
            {
                entity.HasKey(k => k.ResourceId);

                entity.Property(p => p.Name)
                .HasMaxLength(50)
                .IsUnicode(true)
                .IsRequired(true);

                entity.Property(p => p.Url)
                .IsUnicode(false)
                .IsRequired(true);

                entity.Property(p => p.ResourceType)
                .IsRequired(true);

                entity.HasOne(c => c.Course)
                .WithMany(r => r.Resources)
                .HasForeignKey(c => c.CourseId);
            });

            modelBuilder.Entity<Homework>(entity =>
            {
                entity.HasKey(k => k.HomeworkId);

                entity.Property(p => p.Content)
                .IsUnicode(false)
                .IsRequired(true);

                entity.Property(p => p.ContentType)
                .IsRequired(true);

                entity.Property(p => p.SubmissionTime)
                .IsRequired(true);

                entity.HasOne(s => s.Student)
                .WithMany(h => h.HomeworkSubmissions)
                .HasForeignKey(s => s.StudentId);

                entity.HasOne(c => c.Course)
                .WithMany(h => h.HomeworkSubmissions)
                .HasForeignKey(c => c.CourseId);
            });

            modelBuilder.Entity<StudentCourse>(entity =>
            {
                entity.HasKey(sc => new { sc.StudentId, sc.CourseId });

                entity.HasOne(s => s.Student)
                .WithMany(c => c.CourseEnrollments)
                .HasForeignKey(s => s.StudentId);

                entity.HasOne(c => c.Course)
                .WithMany(s => s.StudentsEnrolled)
                .HasForeignKey(c => c.CourseId);
            });
        }
    }
}
