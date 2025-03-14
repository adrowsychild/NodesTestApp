using Models;
using DAL.Implementation;
using Microsoft.EntityFrameworkCore;
using Services.Implementation;
using Services.Interfaces;
using Models.Exceptions;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Models.Dto;

namespace Services.Tests
{
    [TestClass]
    public sealed class NodeValidationTests
    {
        private const string treeName = "testTree";
        private const int treeId = 1;
        private INodeService nodeService;

        [TestInitialize]
        public void Initialise()
        {
            var options = new DbContextOptionsBuilder<DataDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;
            var dbContext = new DataDbContext(options);
            dbContext.Trees.Add(new Tree() { Id = treeId, Name = treeName });
            
            for (int i = 1; i < 10; i++)
            {
                if (i <= 5)
                {
                    dbContext.Nodes.Add(CreateTestNode(i));
                }
                else
                {
                    dbContext.Nodes.Add(CreateTestNode(i, i - 5));
                }
            }

            dbContext.SaveChanges();

            nodeService = new NodeService(new NodeRepository(dbContext), new TreeRepository(dbContext));
        }

        [TestMethod]
        public async Task AllThrowTreeNotFoundException()
        {
            await Assert.ThrowsExceptionAsync<TreeNotFoundException>(() => nodeService.AddNodeAsync(new AddNodeDto() { TreeName = "fake"}));
            await Assert.ThrowsExceptionAsync<TreeNotFoundException>(() => nodeService.DeleteNodeAsync(new DeleteNodeDto() { TreeName = "fake"}));
            await Assert.ThrowsExceptionAsync<TreeNotFoundException>(() => nodeService.RenameNodeAsync(new RenameNodeDto() { TreeName = "fake"}));
        }

        [TestMethod]
        public async Task CreateRenameThrowUniqueNameException()
        {
            await Assert.ThrowsExceptionAsync<UniqueNameException>(() => nodeService.AddNodeAsync(new AddNodeDto() { NodeName = "test1", TreeName = treeName }));
            await Assert.ThrowsExceptionAsync<UniqueNameException>(() => nodeService.RenameNodeAsync(new RenameNodeDto() { NodeId = 2, NewNodeName = "test1", TreeName = treeName }));
        }

        [TestMethod]
        public async Task CreateRenameThrowEmptyNameException()
        {
            await Assert.ThrowsExceptionAsync<NameEmptyException>(() => nodeService.AddNodeAsync(new AddNodeDto() { NodeName = "", TreeName = treeName }));
            await Assert.ThrowsExceptionAsync<NameEmptyException>(() => nodeService.AddNodeAsync(new AddNodeDto() { NodeName = null, TreeName = treeName }));
            await Assert.ThrowsExceptionAsync<NameEmptyException>(() => nodeService.RenameNodeAsync(new RenameNodeDto() { NodeId = 2, NewNodeName = "", TreeName = treeName }));
            await Assert.ThrowsExceptionAsync<NameEmptyException>(() => nodeService.RenameNodeAsync(new RenameNodeDto() { NodeId = 2, NewNodeName = null, TreeName = treeName }));
        }

        [TestMethod]
        public async Task CreateThrowsParentNotFoundException()
        {
            await Assert.ThrowsExceptionAsync<NodeNotFoundException>(() => nodeService.AddNodeAsync(new AddNodeDto() { NodeName = "test1", TreeName = treeName, ParentNodeId = -1 }));
        }

        [TestMethod]
        public async Task DeleteThrowsNotFoundException()
        {
            await Assert.ThrowsExceptionAsync<NodeNotFoundException>(() => nodeService.DeleteNodeAsync(new DeleteNodeDto() { NodeId = 11, TreeName = treeName }));
        }

        [TestMethod]
        public async Task DeleteThrowsDeleteChildrenException()
        {
            await Assert.ThrowsExceptionAsync<DeleteChildrenException>(() => nodeService.DeleteNodeAsync(new DeleteNodeDto() { NodeId = 1, TreeName = treeName }));
        }

        private Node CreateTestNode(int id, int? parentNodeId = null)
        {
            return new Node()
            {
                Id = id,
                Name = $"test{id}",
                TreeName = treeName,
                TreeId = treeId,
                ParentId = parentNodeId
            };
        }
    }
}
