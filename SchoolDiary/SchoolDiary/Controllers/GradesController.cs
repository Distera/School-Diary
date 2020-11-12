using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolDiary.Data;
using SchoolDiary.Data.Entities;
using SchoolDiary.Models;

namespace SchoolDiary.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GradesController : ControllerBase
    {
        private readonly SchoolDiaryDbContext _dataContext;

        private readonly IMapper _mapper;

        public GradesController(SchoolDiaryDbContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task PostAsync(GradeDto gradeDto, CancellationToken cancellationToken = default)
        {
            var grade = new Grade
            {
                Student = await _dataContext.Students.SingleAsync(student => student.Id == gradeDto.StudentId,
                    cancellationToken),
                Subject = await _dataContext.Subjects.SingleAsync(subject => subject.Id == gradeDto.SubjectId,
                    cancellationToken),
                Teacher = await _dataContext.Teachers.SingleAsync(teacher => teacher.Id == gradeDto.TeacherId,
                    cancellationToken)
            };

            await _dataContext.Grades.AddAsync(grade, cancellationToken);
            await _dataContext.SaveChangesAsync(cancellationToken);
        }

        [HttpGet("{id}")]
        public async Task<GradeDto> GetAsync(int id, CancellationToken cancellationToken = default)
        {
            return _mapper.Map<GradeDto>(
                await _dataContext.Grades.SingleAsync(grade => grade.Id == id, cancellationToken)
            );
        }
        
        [HttpPut("{id}")]
        public async Task PutAsync(int id, GradeDto gradeDto, CancellationToken cancellationToken = default)
        {
            var gradeToUpdate =
                await _dataContext.Grades.SingleAsync(grade => grade.Id == id, cancellationToken);

            gradeToUpdate.Student =
                await _dataContext.Students.SingleAsync(student => student.Id == gradeDto.StudentId, cancellationToken);
            gradeToUpdate.Subject =
                await _dataContext.Subjects.SingleAsync(subject => subject.Id == gradeDto.SubjectId, cancellationToken);
            gradeToUpdate.Teacher =
                await _dataContext.Teachers.SingleAsync(teacher => teacher.Id == gradeDto.TeacherId, cancellationToken);

            await _dataContext.SaveChangesAsync(cancellationToken);
        }

        [HttpDelete("{id}")]
        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            _dataContext.Grades.Remove(
                await _dataContext.Grades.SingleAsync(grade => grade.Id == id, cancellationToken)
            );

            await _dataContext.SaveChangesAsync(cancellationToken);
        }
    }
}