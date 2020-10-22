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
    public class StudentsController : ControllerBase
    {
        private readonly SchoolDiaryDbContext _dataContext;

        private readonly IMapper _mapper;

        public StudentsController(SchoolDiaryDbContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }
        
        [HttpGet]
        public async Task<IEnumerable<StudentMinDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var students = await _dataContext.Students.ToListAsync(cancellationToken);

            return students.Select(student => _mapper.Map<StudentMinDto>(student));
        }

        [HttpPost]
        public async Task PostAsync(StudentDto studentDto, CancellationToken cancellationToken = default)
        {
            var student = new Student
            {
                LastName = studentDto.LastName,
                FirstName = studentDto.FirstName,
                MiddleName = studentDto.MiddleName,
                Grades = await Task.WhenAll(
                    studentDto.GradesIds.Select(async gradeId =>
                        await _dataContext.Grades.SingleAsync(grade => grade.Id == gradeId, cancellationToken)
                    )
                )
            };

            await _dataContext.Students.AddAsync(student, cancellationToken);
            await _dataContext.SaveChangesAsync(cancellationToken);
        }

        [HttpGet("{id}")]
        public async Task<StudentDto> GetAsync(int id, CancellationToken cancellationToken = default)
        {
            return _mapper.Map<StudentDto>(
                await _dataContext.Students.SingleAsync(student => student.Id == id, cancellationToken)
            );
        }

        [HttpPut("{id}")]
        public async Task PutAsync(int id, StudentDto studentDto, CancellationToken cancellationToken = default)
        {
            var studentToUpdate =
                await _dataContext.Students.SingleAsync(student => student.Id == id, cancellationToken);

            studentToUpdate.LastName = studentDto.LastName;
            studentToUpdate.FirstName = studentDto.FirstName;
            studentToUpdate.MiddleName = studentDto.MiddleName;
            studentToUpdate.Grades = await Task.WhenAll(
                studentDto.GradesIds.Select(async gradeId =>
                    await _dataContext.Grades.SingleAsync(grade => grade.Id == gradeId, cancellationToken)
                )
            );

            await _dataContext.SaveChangesAsync(cancellationToken);
        }

        [HttpDelete("{id}")]
        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            _dataContext.Students.Remove(
                await _dataContext.Students.SingleAsync(student => student.Id == id, cancellationToken)
            );

            await _dataContext.SaveChangesAsync(cancellationToken);
        }
    }
}