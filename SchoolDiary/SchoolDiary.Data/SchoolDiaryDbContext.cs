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
    }
}