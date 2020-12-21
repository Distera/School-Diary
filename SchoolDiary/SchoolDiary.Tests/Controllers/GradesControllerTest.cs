using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using SchoolDiary.Controllers;
using SchoolDiary.Data.Entities;
using SchoolDiary.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace SchoolDiary.Tests.Controllers
{
    public class GradesControllerTest : ControllerTestBase
    {
        private static GradeDto ComposeTestGradeDto(Student student, Teacher teacher, Subject subject) => new GradeDto
        {
            Value = 5,
            StudentId = student.Id,
            TeacherId = teacher.Id,
            SubjectId = subject.Id
        };

        private static void AssertAreEqual(GradeDto gradeDto, Grade grade)
        {
            Assert.AreEqual(gradeDto.Value, grade.Value);
            Assert.AreEqual(gradeDto.StudentId, grade.Student.Id);
            Assert.AreEqual(gradeDto.TeacherId, grade.Teacher.Id);
            Assert.AreEqual(gradeDto.SubjectId, grade.Subject.Id);
        }

        [Test]
        public async Task ShouldGetAllGradesProperly()
        {
            var grades = EntitiesGenerationRange.Select(_ => new Grade()).ToList();

            var context = ComposeEmptyDataContext();
            await context.Grades.AddRangeAsync(grades);
            await context.SaveChangesAsync();

            var mapperMock = new Mock<IMapper>();
            grades.ForEach(grade =>
                mapperMock.Setup(mapper => mapper.Map<GradeMinDto>(grade)).Verifiable());

            foreach (var _ in await new GradesController(context, mapperMock.Object).GetAllAsync())
            {
            }

            grades.ForEach(grade =>
                mapperMock.Verify(mapper => mapper.Map<GradeMinDto>(grade), Times.Once));
        }

        [Test]
        public async Task ShouldPostGradeProperly()
        {
            var student = new Student();
            var teacher = new Teacher();
            var subject = new Subject();

            var context = ComposeEmptyDataContext();
            await context.Students.AddAsync(student);
            await context.Teachers.AddAsync(teacher);
            await context.Subjects.AddAsync(subject);
            await context.SaveChangesAsync();

            var gradeDto = ComposeTestGradeDto(student, teacher, subject);

            await new GradesController(context, null).PostAsync(gradeDto);

            var grade = await context.Grades.SingleAsync();
            AssertAreEqual(gradeDto, grade);
        }

        [Test]
        public async Task ShouldGetGradeProperly()
        {
            var grade = new Grade();

            var context = ComposeEmptyDataContext();
            await context.Grades.AddAsync(grade);
            await context.SaveChangesAsync();

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(mapper => mapper.Map<GradeDto>(grade)).Verifiable();

            await new GradesController(context, mapperMock.Object).GetAsync(grade.Id);

            mapperMock.Verify(mapper => mapper.Map<GradeDto>(grade), Times.Once);
        }

        [Test]
        public void ShouldThrowExceptionWhenTryingToGetGradeThatDoesNotExist()
        {
            var context = ComposeEmptyDataContext();

            Assert.ThrowsAsync<InvalidOperationException>(
                () => new GradesController(context, null).GetAsync(ComposeRandomId())
            );
        }

        [Test]
        public async Task ShouldPutGradeProperly()
        {
            var grade = new Grade();
            var student = new Student();
            var teacher = new Teacher();
            var subject = new Subject();

            var context = ComposeEmptyDataContext();
            await context.Grades.AddAsync(grade);
            await context.Students.AddAsync(student);
            await context.Teachers.AddAsync(teacher);
            await context.Subjects.AddAsync(subject);
            await context.SaveChangesAsync();

            var gradeDto = ComposeTestGradeDto(student, teacher, subject);

            await new GradesController(context, null).PutAsync(grade.Id, gradeDto);

            AssertAreEqual(gradeDto, grade);
        }

        [Test]
        public void ShouldThrowExceptionWhenTryingToPutGradeThatDoesNotExist()
        {
            var context = ComposeEmptyDataContext();

            Assert.ThrowsAsync<InvalidOperationException>(
                () => new GradesController(context, null).PutAsync(ComposeRandomId(), new GradeDto())
            );
        }

        [Test]
        public async Task ShouldDeleteGradeProperly()
        {
            var grade = new Grade();

            var context = ComposeEmptyDataContext();
            await context.Grades.AddAsync(grade);
            await context.SaveChangesAsync();

            await new GradesController(context, null).DeleteAsync(grade.Id);

            Assert.IsEmpty(context.Grades);
        }

        [Test]
        public void ShouldThrowExceptionWhenTryingToDeleteGradeThatDoesNotExist()
        {
            var context = ComposeEmptyDataContext();

            Assert.ThrowsAsync<InvalidOperationException>(
                () => new GradesController(context, null).DeleteAsync(ComposeRandomId())
            );
        }
    }
}