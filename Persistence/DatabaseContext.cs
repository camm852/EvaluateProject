using EvaluationProjects.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace EvaluationProjects.Persistence
{
    public partial class DatabaseContext : DbContext
    {

        public DatabaseContext()
        {
        }

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Assignment> Assignments { get; set; }

        public virtual DbSet<Evaluation> Evaluations { get; set; }

        public virtual DbSet<Project> Projects { get; set; }

        public virtual DbSet<Status> Statuses { get; set; }

        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Assignment>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("assignment_pkey");

                entity.ToTable("assignment");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.AssignmentDate).HasColumnName("assignment_date");
                entity.Property(e => e.ProjectId).HasColumnName("project_id");
                entity.Property(e => e.TeacherId).HasColumnName("teacher_id");

                entity.HasOne(d => d.Project).WithOne(p => p.Assignment)
                    .HasForeignKey<Assignment>(d => d.ProjectId)
                    .HasConstraintName("assignment_project_id_fkey");

                entity.HasOne(d => d.Teacher).WithMany(p => p.Assignments)
                    .HasForeignKey(d => d.TeacherId)
                    .HasConstraintName("assignment_teacher_id_fkey");
            });

            modelBuilder.Entity<Evaluation>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("evaluation_pkey");

                entity.ToTable("evaluation");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Approved).HasColumnName("approved");
                entity.Property(e => e.AssignmentId).HasColumnName("assignment_id");
                entity.Property(e => e.EvaluationDate).HasColumnName("evaluation_date");
                entity.Property(e => e.Feedback).HasColumnName("feedback");
                entity.Property(e => e.StatusId).HasColumnName("status_id");

                entity.HasOne(e => e.Assignment)
                            .WithOne(a => a.Evaluations)
                            .HasForeignKey<Evaluation>(e => e.AssignmentId)
                            .HasConstraintName("evaluation_assignment_id_fkey");

                entity.HasOne(d => d.Status).WithMany(p => p.Evaluations)
                    .HasForeignKey(d => d.StatusId)
                    .HasConstraintName("evaluation_status_id_fkey");
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("project_pkey");

                entity.ToTable("project");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.DeliveryDate).HasColumnName("delivery_date");
                entity.Property(e => e.Description).HasColumnName("description");
                entity.Property(e => e.File)
                    .HasMaxLength(255)
                    .HasColumnName("file");
                entity.Property(e => e.StudentId).HasColumnName("student_id");
                entity.Property(e => e.Title)
                    .HasMaxLength(200)
                    .HasColumnName("title");

                entity.HasOne(d => d.Student).WithMany(p => p.Projects)
                    .HasForeignKey(d => d.StudentId)
                    .HasConstraintName("project_student_id_fkey");
            });

            modelBuilder.Entity<Status>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("status_pkey");

                entity.ToTable("status");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Description)
                    .HasMaxLength(100)
                    .HasColumnName("description");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("user_pkey");

                entity.ToTable("user");

                entity.HasIndex(e => e.Email, "user_email_key").IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .HasColumnName("email");
                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("name");
                entity.Property(e => e.Password)
                    .HasMaxLength(256)
                    .HasColumnName("password");
                entity.Property(e => e.Role)
                    .HasMaxLength(20)
                    .HasColumnName("role");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}