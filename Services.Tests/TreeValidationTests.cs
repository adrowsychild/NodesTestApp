using DAL.Implementation;
using Microsoft.EntityFrameworkCore;
using Models.Exceptions;
using Models;
using Services.Implementation;
using Services.Interfaces;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Services.Tests
{
    [TestClass]
    public class TreeValidationTests
    {
        private const string treeName = "testTree";
        private const int treeId = 1;
        private ITreeService treeService;

        [TestInitialize]
        public void Initialise()
        {
            var options = new DbContextOptionsBuilder<DataDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;
            var dbContext = new DataDbContext(options);
            dbContext.Trees.Add(new Tree() { Id = treeId, Name = treeName });

            dbContext.SaveChanges();

            treeService = new TreeService(new TreeRepository(dbContext));
        }

        [TestMethod]
        public async Task GetTreeThrowTreeNotFoundException()
        {
            await Assert.ThrowsExceptionAsync<TreeNotFoundException>(() => treeService.GetTreeAsync("fake"));
        }

        [TestMethod]
        public async Task CreateThrowsEmptyNameException()
        {
            await Assert.ThrowsExceptionAsync<NameEmptyException>(() => treeService.AddTreeAsync(null));
            await Assert.ThrowsExceptionAsync<NameEmptyException>(() => treeService.AddTreeAsync(""));
        }
    }
}
