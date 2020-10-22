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
    public class TeachersController : ControllerBase
    {
        private readonly SchoolDiaryDbContext _dataContext;

        private readonly IMapper _mapper;

        public TeachersController(SchoolDiaryDbContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }
        
         [HttpGet]
        public async Task<IEnumerable<TeacherDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var teachers = await _dataContext.Teachers.ToListAsync(cancellationToken);

            return teachers.Select(teacher => _mapper.Map<TeacherDto>(teacher));
        }

        [HttpPost]
        public async Task PostAsync(TeacherDto teacherDto, CancellationToken cancellationToken = default)
        {
            var teacher = new Teacher()
            {
                LastName = teacherDto.LastName,
                FirstName = teacherDto.FirstName,
                MiddleName = teacherDto.MiddleName,
                Phone = teacherDto.Phone,                
                Subjects = await Task.WhenAll(
                    teacherDto.SubjectsIds.Select(async subjectId =>
                        await _dataContext.Subjects.SingleAsync(subject => subject.Id == subjectId, cancellationToken)
                    )
                )
            };

            await _dataContext.Teachers.AddAsync(teacher, cancellationToken);
            await _dataContext.SaveChangesAsync(cancellationToken);
        }

        [HttpGet("{id}")]
        public async Task<TeacherDto> GetAsync(int id, CancellationToken cancellationToken = default)
        {
            return _mapper.Map<TeacherDto>(
                await _dataContext.Teachers.SingleAsync(subject => subject.Id == id, cancellationToken)
            );
        }

        [HttpPut("{id}")]
        public async Task PutAsync(int id, TeacherDto teacherDto, CancellationToken cancellationToken = default)
        {
            var teacherToUpdate =
                await _dataContext.Teachers.SingleAsync(teacher => teacher.Id == id, cancellationToken);

            teacherToUpdate.LastName = teacherDto.LastName;
            teacherToUpdate.FirstName = teacherDto.FirstName;
            teacherToUpdate.MiddleName = teacherDto.MiddleName;
            teacherToUpdate.Phone = teacherDto.Phone;
            teacherToUpdate.Subjects = await Task.WhenAll(
                teacherDto.SubjectsIds.Select(async subjectId =>
                    await _dataContext.Subjects.SingleAsync(subject => subject.Id == subjectId, cancellationToken)
                )
            );

            await _dataContext.SaveChangesAsync(cancellationToken);
        }

        [HttpDelete("{id}")]
        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            _dataContext.Teachers.Remove(
                await _dataContext.Teachers.SingleAsync(teacher => teacher.Id == id, cancellationToken)
            );

            await _dataContext.SaveChangesAsync(cancellationToken);
        }
    }
}