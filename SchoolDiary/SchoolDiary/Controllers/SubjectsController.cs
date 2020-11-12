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
    public class SubjectsController : ControllerBase
    {
        private readonly SchoolDiaryDbContext _dataContext;

        private readonly IMapper _mapper;

        public SubjectsController(SchoolDiaryDbContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<SubjectDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var subjects = await _dataContext.Subjects.ToListAsync(cancellationToken);

            return subjects.Select(subject => _mapper.Map<SubjectDto>(subject));
        }

        [HttpGet("{id}")]
        public async Task<SubjectDto> GetAsync(int id, CancellationToken cancellationToken = default)
        {
            return _mapper.Map<SubjectDto>(
                await _dataContext.Subjects.SingleAsync(subject => subject.Id == id, cancellationToken)
            );
        }

        [HttpPut("{id}")]
        public async Task PutAsync(int id, SubjectDto subjectDto, CancellationToken cancellationToken = default)
        {
            var subjectToUpdate =
                await _dataContext.Subjects.SingleAsync(subject => subject.Id == id, cancellationToken);

            subjectToUpdate.Name = subjectDto.Name;

            await _dataContext.SaveChangesAsync(cancellationToken);
        }

        [HttpPost]
        public async Task PostAsync(SubjectDto subjectDto, CancellationToken cancellationToken = default)
        {
            var subject = new Subject
            {
                Name = subjectDto.Name,
            };

            await _dataContext.Subjects.AddAsync(subject, cancellationToken);
            await _dataContext.SaveChangesAsync(cancellationToken);
        }

        [HttpDelete("{id}")]
        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            _dataContext.Subjects.Remove(
                await _dataContext.Subjects.SingleAsync(subject => subject.Id == id, cancellationToken)
            );

            await _dataContext.SaveChangesAsync(cancellationToken);
        }
    }
}