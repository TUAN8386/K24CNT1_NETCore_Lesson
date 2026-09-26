using DatNetCoreLAB6_EF.Models;
using Microsoft.EntityFrameworkCore;

namespace DatNetCoreLAB6_EF.Data
{
    // CSDL DatStudentManager: DatStdClass, DatStudent, DatSubjects, DatMarks
    public class DatStudentDbContext : DbContext
    {
        public DatStudentDbContext(DbContextOptions<DatStudentDbContext> options) : base(options) { }

        public DbSet<DatStdClass> DatStdClasses => Set<DatStdClass>();
        public DbSet<DatStudent> DatStudents => Set<DatStudent>();
        public DbSet<DatSubject> DatSubjects => Set<DatSubject>();
        public DbSet<DatMark> DatMarks => Set<DatMark>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Khóa chính trên 2 cột (SubjectId, StudentId)
            modelBuilder.Entity<DatMark>()
                .HasKey(datM => new { datM.SubjectId, datM.StudentId });

            // Ràng buộc không trùng
            modelBuilder.Entity<DatStudent>().HasIndex(datS => datS.StudentEmail).IsUnique();
            modelBuilder.Entity<DatStudent>().HasIndex(datS => datS.StudentPhone).IsUnique();
            modelBuilder.Entity<DatSubject>().HasIndex(datS => datS.SubjectName).IsUnique();

            // StdClass 1 - n Student
            modelBuilder.Entity<DatStudent>()
                .HasOne(datS => datS.StdClass)
                .WithMany(datC => datC.Students)
                .HasForeignKey(datS => datS.ClassId)
                .OnDelete(DeleteBehavior.Restrict);

            // Student 1 - n Marks (xóa sinh viên thì xóa điểm)
            modelBuilder.Entity<DatMark>()
                .HasOne(datM => datM.Student)
                .WithMany(datS => datS.Marks)
                .HasForeignKey(datM => datM.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            // Subjects 1 - n Marks
            modelBuilder.Entity<DatMark>()
                .HasOne(datM => datM.Subject)
                .WithMany(datS => datS.Marks)
                .HasForeignKey(datM => datM.SubjectId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
