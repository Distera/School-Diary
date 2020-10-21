using System.Collections.Generic;

namespace SchoolDiary.Data.Entities
{
    public class Teacher
    {
        public int Id { get; set; }
        public string LastName { get; set; }
        public string Name { get; set; }
        public string MiddleName { get; set; }
        public string Phone { get; set; }
        public IEnumerable<Subject> Subjects { get; set; }
    }
}