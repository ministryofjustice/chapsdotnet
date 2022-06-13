using ChapsDotNET.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using System;

namespace ChapsDotNET.Business.Tests.Common
{
    public class DataContextFactory
    {
        public static DataContext Create()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new DataContext(options);
            context.Database.EnsureCreated();

            return context;
        }
    }
}
