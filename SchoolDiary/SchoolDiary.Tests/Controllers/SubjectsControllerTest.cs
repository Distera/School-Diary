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
    public class SubjectsControllerTest : ControllerTestBase
    {
        private static SubjectDto ComposeTestSubjectDto() => new SubjectDto
        {
            Name = "TestSubjectName",
            Description = "TestSubjectDescription"
        };

        private static void AssertAreEqual(SubjectDto subjectDto, Subject subject)
        {
            Assert.AreEqual(subjectDto.Name, subject.Name);
            Assert.AreEqual(subjectDto.Description, subject.Description);
        }

        [Test]
        public async Task ShouldGetAllSubjectsProperly()
        {
            var subjects = EntitiesGenerationRange.Select(_ => new Subject()).ToList();

            var context = ComposeEmptyDataContext();
            await context.Subjects.AddRangeAsync(subjects);
            await context.SaveChangesAsync();

            var mapperMock = new Mock<IMapper>();
            subjects.ForEach(subject =>
                mapperMock.Setup(mapper => mapper.Map<SubjectMinDto>(subject)).Verifiable());

            foreach (var _ in await new SubjectsController(context, mapperMock.Object).GetAllAsync())
            {
            }

            subjects.ForEach(subject =>
                mapperMock.Verify(mapper => mapper.Map<SubjectMinDto>(subject), Times.Once));
        }

        [Test]
        public async Task ShouldPostSubjectProperly()
        {
            var context = ComposeEmptyDataContext();

            var subjectDto = ComposeTestSubjectDto();

            await new SubjectsController(context, null).PostAsync(subjectDto);

            var subject = await context.Subjects.SingleAsync();
            AssertAreEqual(subjectDto, subject);
        }

        [Test]
        public async Task ShouldGetSubjectProperly()
        {
            var subject = new Subject();

            var context = ComposeEmptyDataContext();
            await context.Subjects.AddAsync(subject);
            await context.SaveChangesAsync();

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(mapper => mapper.Map<SubjectDto>(subject)).Verifiable();

            await new SubjectsController(context, mapperMock.Object).GetAsync(subject.Id);

            mapperMock.Verify(mapper => mapper.Map<SubjectDto>(subject), Times.Once);
        }

        [Test]
        public void ShouldThrowExceptionWhenTryingToGetSubjectThatDoesNotExist()
        {
            var context = ComposeEmptyDataContext();

            Assert.ThrowsAsync<InvalidOperationException>(
                () => new SubjectsController(context, null).GetAsync(ComposeRandomId())
            );
        }

        [Test]
        public async Task ShouldPutSubjectProperly()
        {
            var subject = new Subject();

            var context = ComposeEmptyDataContext();
            await context.Subjects.AddAsync(subject);
            await context.SaveChangesAsync();

            var subjectDto = ComposeTestSubjectDto();

            await new SubjectsController(context, null).PutAsync(subject.Id, subjectDto);

            AssertAreEqual(subjectDto, subject);
        }

        [Test]
        public void ShouldThrowExceptionWhenTryingToPutSubjectThatDoesNotExist()
        {
            var context = ComposeEmptyDataContext();

            Assert.ThrowsAsync<InvalidOperationException>(
                () => new SubjectsController(context, null).PutAsync(ComposeRandomId(), new SubjectDto())
            );
        }

        [Test]
        public async Task ShouldDeleteSubjectProperly()
        {
            var subject = new Subject();

            var context = ComposeEmptyDataContext();
            await context.Subjects.AddAsync(subject);
            await context.SaveChangesAsync();

            await new SubjectsController(context, null).DeleteAsync(subject.Id);

            Assert.IsEmpty(context.Subjects);
        }

        [Test]
        public void ShouldThrowExceptionWhenTryingToDeleteSubjectThatDoesNotExist()
        {
            var context = ComposeEmptyDataContext();

            Assert.ThrowsAsync<InvalidOperationException>(
                () => new SubjectsController(context, null).DeleteAsync(ComposeRandomId())
            );
        }
    }
}