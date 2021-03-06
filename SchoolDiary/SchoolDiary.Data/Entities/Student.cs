﻿using System.Collections.Generic;

namespace SchoolDiary.Data.Entities
{
    public class Student
    {
        public int Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }

        public List<Grade> Grades { get; set; }
    }
}