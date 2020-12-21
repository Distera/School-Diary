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
    public class TeachersControllerTest : ControllerTestBase
    {
        private static TeacherDto ComposeTestTeacherDto(IEnumerable<Subject> subjects) => new TeacherDto
        {
            LastName = "TestTeacherLastName",
            FirstName = "TestTeacherFirstName",
            MiddleName = "TestTeacherMiddleName",
            Phone = "TestTeacherPhone",
            SubjectsIds = subjects.Select(subject => subject.Id)
        };

        private static void AssertAreEqual(TeacherDto teacherDto, Teacher teacher)
        {
            Assert.AreEqual(teacherDto.LastName, teacher.LastName);
            Assert.AreEqual(teacherDto.FirstName, teacher.FirstName);
            Assert.AreEqual(teacherDto.MiddleName, teacher.MiddleName);
            Assert.AreEqual(teacherDto.Phone, teacher.Phone);
            Assert.AreEqual(teacherDto.SubjectsIds, teacher.Subjects.Select(subject => subject.Id));
        }

        [Test]
        public async Task ShouldGetAllTeachersProperly()
        {
            var teachers = EntitiesGenerationRange.Select(_ => new Teacher()).ToList();

            var context = ComposeEmptyDataContext();
            await context.Teachers.AddRangeAsync(teachers);
            await context.SaveChangesAsync();

            var mapperMock = new Mock<IMapper>();
            teachers.ForEach(teacher =>
                mapperMock.Setup(mapper => mapper.Map<TeacherMinDto>(teacher)).Verifiable());

            foreach (var _ in await new TeachersController(context, mapperMock.Object).GetAllAsync())
            {
            }

            teachers.ForEach(teacher =>
                mapperMock.Verify(mapper => mapper.Map<TeacherMinDto>(teacher), Times.Once));
        }

        [Test]
        public async Task ShouldPostTeacherProperly()
        {
            var subjects = EntitiesGenerationRange.Select(_ => new Subject()).ToList();

            var context = ComposeEmptyDataContext();
            await context.Subjects.AddRangeAsync(subjects);
            await context.SaveChangesAsync();

            var teacherDto = ComposeTestTeacherDto(subjects);

            await new TeachersController(context, null).PostAsync(teacherDto);

            var teacher = await context.Teachers.SingleAsync();
            AssertAreEqual(teacherDto, teacher);
        }

        [Test]
        public async Task ShouldGetTeacherProperly()
        {
            var teacher = new Teacher();

            var context = ComposeEmptyDataContext();
            await context.Teachers.AddAsync(teacher);
            await context.SaveChangesAsync();

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(mapper => mapper.Map<TeacherDto>(teacher)).Verifiable();

            await new TeachersController(context, mapperMock.Object).GetAsync(teacher.Id);

            mapperMock.Verify(mapper => mapper.Map<TeacherDto>(teacher), Times.Once);
        }

        [Test]
        public void ShouldThrowExceptionWhenTryingToGetTeacherThatDoesNotExist()
        {
            var context = ComposeEmptyDataContext();

            Assert.ThrowsAsync<InvalidOperationException>(
                () => new TeachersController(context, null).GetAsync(ComposeRandomId())
            );
        }

        [Test]
        public async Task ShouldPutTeacherProperly()
        {
            var teacher = new Teacher();
            var subjects = EntitiesGenerationRange.Select(_ => new Subject()).ToList();

            var context = ComposeEmptyDataContext();
            await context.Teachers.AddAsync(teacher);
            await context.Subjects.AddRangeAsync(subjects);
            await context.SaveChangesAsync();

            var teacherDto = ComposeTestTeacherDto(subjects);

            await new TeachersController(context, null).PutAsync(teacher.Id, teacherDto);

            AssertAreEqual(teacherDto, teacher);
        }

        [Test]
        public void ShouldThrowExceptionWhenTryingToPutTeacherThatDoesNotExist()
        {
            var context = ComposeEmptyDataContext();

            Assert.ThrowsAsync<InvalidOperationException>(
                () => new TeachersController(context, null).PutAsync(ComposeRandomId(), new TeacherDto())
            );
        }

        [Test]
        public async Task ShouldDeleteTeacherProperly()
        {
            var teacher = new Teacher();

            var context = ComposeEmptyDataContext();
            await context.Teachers.AddAsync(teacher);
            await context.SaveChangesAsync();

            await new TeachersController(context, null).DeleteAsync(teacher.Id);

            Assert.IsEmpty(context.Teachers);
        }

        [Test]
        public void ShouldThrowExceptionWhenTryingToDeleteTeacherThatDoesNotExist()
        {
            var context = ComposeEmptyDataContext();

            Assert.ThrowsAsync<InvalidOperationException>(
                () => new TeachersController(context, null).DeleteAsync(ComposeRandomId())
            );
        }
    }
}