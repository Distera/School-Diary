using System;
using System.Collections.Generic;
using System.Linq;
using SchoolDiary.Data;
using Microsoft.EntityFrameworkCore;

namespace SchoolDiary.Tests.Controllers
{
    public abstract class ControllerTestBase
    {
        private static readonly Random Random = new Random();

        protected static int ComposeRandomId() => Random.Next(1, 100);

        protected static readonly IEnumerable<int> EntitiesGenerationRange = Enumerable.Range(0, Random.Next(2, 10));

        protected static SchoolDiaryDbContext ComposeEmptyDataContext() =>
            new SchoolDiaryDbContext(
                new DbContextOptionsBuilder<SchoolDiaryDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .Options
            );
    }
}