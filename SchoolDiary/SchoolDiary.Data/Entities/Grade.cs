namespace SchoolDiary.Data.Entities
{
    public class Grade
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public Student Student { get; set; }
        public Teacher Teacher { get; set; }
        public Subject Subject { get; set; }
    }
}