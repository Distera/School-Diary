using SchoolDiary.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace SchoolDiary.Data
{
    public class SchoolDiaryDbContext : DbContext
    {
        public SchoolDiaryDbContext(DbContextOptions<SchoolDiaryDbContext> options) : base(options)
        {
        }

        public virtual DbSet<Teacher> Teachers { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<Grade> Grades { get; set; }
        public virtual DbSet<Subject> Subjects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>()
                .HasMany(student => student.Grades)
                .WithOne(grade => grade.Student)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Teacher>()
                .HasMany(teacher => teacher.Subjects)
                .WithOne()
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}