using System.Collections.Generic;

namespace SchoolDiary.Data.Entities
{
    public class Student
    {
        public int Id { get; set; }
        public string LastName { get; set; }
        public string Name { get; set; }
        public string MiddleName { get; set; }

        public IEnumerable<Grade> Grades { get; set; }
    }
}