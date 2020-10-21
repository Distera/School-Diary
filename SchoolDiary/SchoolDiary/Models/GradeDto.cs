using SchoolDiary.Data.Entities;

namespace SchoolDiary.Models
{
    public class GradeDto
    {
        public int Value { get; set; }
        public Student Student { get; set; }
        public Teacher Teacher { get; set; }
        public Subject Subject { get; set; }
    }
}