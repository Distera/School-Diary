using AutoMapper;
using SchoolDiary.Data.Entities;
using SchoolDiary.Models;

namespace SchoolDiary
{
    public class MappingProfile : Profile
        {
        public MappingProfile()
        {
            CreateMap<Grade, GradeDto>();
        }
    }
}