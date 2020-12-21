using System;
using System.Collections.Generic;
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
    public class StudentsControllerTest : ControllerTestBase
    {
        private static StudentDto ComposeTestStudentDto(IEnumerable<Grade> grades) => new StudentDto
        {
            LastName = "TestStudentLastName",
            FirstName = "TestStudentFirstName",
            MiddleName = "TestStudentMiddleName",
            GradesIds = grades.Select(grade => grade.Id)
        };

        private static void AssertAreEqual(StudentDto studentDto, Student student)
        {
            Assert.AreEqual(studentDto.LastName, student.LastName);
            Assert.AreEqual(studentDto.FirstName, student.FirstName);
            Assert.AreEqual(studentDto.MiddleName, student.MiddleName);
            Assert.AreEqual(studentDto.GradesIds, student.Grades.Select(grade => grade.Id));
        }

        [Test]
        public async Task ShouldGetAllStudentsProperly()
        {
            var students = EntitiesGenerationRange.Select(_ => new Student()).ToList();

            var context = ComposeEmptyDataContext();
            await context.Students.AddRangeAsync(students);
            await context.SaveChangesAsync();

            var mapperMock = new Mock<IMapper>();
            students.ForEach(student =>
                mapperMock.Setup(mapper => mapper.Map<StudentMinDto>(student)).Verifiable());

            foreach (var _ in await new StudentsController(context, mapperMock.Object).GetAllAsync())
            {
            }

            students.ForEach(student =>
                mapperMock.Verify(mapper => mapper.Map<StudentMinDto>(student), Times.Once));
        }

        [Test]
        public async Task ShouldPostStudentProperly()
        {
            var grades = EntitiesGenerationRange.Select(_ => new Grade()).ToList();

            var context = ComposeEmptyDataContext();
            await context.Grades.AddRangeAsync(grades);
            await context.SaveChangesAsync();

            var studentDto = ComposeTestStudentDto(grades);

            await new StudentsController(context, null).PostAsync(studentDto);

            var student = await context.Students.SingleAsync();
            AssertAreEqual(studentDto, student);
        }

        [Test]
        public async Task ShouldGetStudentProperly()
        {
            var student = new Student();

            var context = ComposeEmptyDataContext();
            await context.Students.AddAsync(student);
            await context.SaveChangesAsync();

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(mapper => mapper.Map<StudentDto>(student)).Verifiable();

            await new StudentsController(context, mapperMock.Object).GetAsync(student.Id);

            mapperMock.Verify(mapper => mapper.Map<StudentDto>(student), Times.Once);
        }

        [Test]
        public void ShouldThrowExceptionWhenTryingToGetStudentThatDoesNotExist()
        {
            var context = ComposeEmptyDataContext();

            Assert.ThrowsAsync<InvalidOperationException>(
                () => new StudentsController(context, null).GetAsync(ComposeRandomId())
            );
        }

        [Test]
        public async Task ShouldPutStudentProperly()
        {
            var student = new Student();
            var grades = EntitiesGenerationRange.Select(_ => new Grade()).ToList();

            var context = ComposeEmptyDataContext();
            await context.Students.AddAsync(student);
            await context.Grades.AddRangeAsync(grades);
            await context.SaveChangesAsync();

            var studentDto = ComposeTestStudentDto(grades);

            await new StudentsController(context, null).PutAsync(student.Id, studentDto);

            AssertAreEqual(studentDto, student);
        }

        [Test]
        public void ShouldThrowExceptionWhenTryingToPutStudentThatDoesNotExist()
        {
            var context = ComposeEmptyDataContext();

            Assert.ThrowsAsync<InvalidOperationException>(
                () => new StudentsController(context, null).PutAsync(ComposeRandomId(), new StudentDto())
            );
        }

        [Test]
        public async Task ShouldDeleteStudentProperly()
        {
            var student = new Student();

            var context = ComposeEmptyDataContext();
            await context.Students.AddAsync(student);
            await context.SaveChangesAsync();

            await new StudentsController(context, null).DeleteAsync(student.Id);

            Assert.IsEmpty(context.Students);
        }

        [Test]
        public void ShouldThrowExceptionWhenTryingToDeleteStudentThatDoesNotExist()
        {
            var context = ComposeEmptyDataContext();

            Assert.ThrowsAsync<InvalidOperationException>(
                () => new StudentsController(context, null).DeleteAsync(ComposeRandomId())
            );
        }
    }
}