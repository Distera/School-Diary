using System.Collections.Generic;

namespace SchoolDiary.Models
{
    public class StudentDto
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }

        public IEnumerable<int> GradesIds { get; set; }
    }
}