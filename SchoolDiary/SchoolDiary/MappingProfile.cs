using System.Linq;
using AutoMapper;
using SchoolDiary.Data.Entities;
using SchoolDiary.Models;

namespace SchoolDiary
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Grade, GradeDto>()
                .ForMember(
                    dto => dto.StudentId,
                    memberConfiguration => memberConfiguration.MapFrom(grade => grade.Student.Id)
                )
                .ForMember(
                    dto => dto.SubjectId,
                    memberConfiguration => memberConfiguration.MapFrom(grade => grade.Subject.Id)
                )
                .ForMember(
                    dto => dto.TeacherId,
                    memberConfiguration => memberConfiguration.MapFrom(grade => grade.Teacher.Id)
                );
            CreateMap<Subject, SubjectDto>();

            CreateMap<Student, StudentDto>()
                .ForMember(
                    dto => dto.GradesIds,
                    memberConfiguration => memberConfiguration.MapFrom(
                        publication => publication.Grades.Select(grade => grade.Id)
                    )
                );

            CreateMap<Teacher, TeacherDto>()
                .ForMember(
                    dto => dto.SubjectsIds,
                    memberConfiguration => memberConfiguration.MapFrom(
                        publication => publication.Subjects.Select(subject => subject.Id)
                    )
                );
        }
    }
}