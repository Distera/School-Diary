using SchoolDiary.Data.Entities;

namespace SchoolDiary.Models
{
    public class GradeDto
    {
        public int Value { get; set; }
        public int? StudentId { get; set; }
        public int? TeacherId { get; set; }
        public int? SubjectId { get; set; }
    }
}