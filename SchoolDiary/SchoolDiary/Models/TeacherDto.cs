using System.Collections.Generic;

namespace SchoolDiary.Models
{
    public class TeacherDto
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string Phone { get; set; }
        public IEnumerable<int> SubjectsIds { get; set; }
    }
}