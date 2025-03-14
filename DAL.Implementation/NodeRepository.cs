using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using Models;

namespace DAL.Implementation
{
    public class NodeRepository : INodeRepository
    {
        private readonly DataDbContext context;

        public NodeRepository(DataDbContext context)
        {
            this.context = context;
        }

        public async Task<Node> AddNodeAsync(Node node)
        {
            context.Nodes.Add(node);
            await context.SaveChangesAsync();
            return node;
        }

        public async Task DeleteNodeAsync(Node node)
        {
            context.Nodes.Remove(node);
            await context.SaveChangesAsync();
        }

        public async Task RenameNodeAsync(Node node)
        {
            context.Nodes.Entry(node).CurrentValues.SetValues(node);
            await context.SaveChangesAsync();
        }

        public async Task<ICollection<Node>> GetNodesByTreeNameAsync(string treeName)
        {
            return await context.Nodes.Include(x => x.Nodes).Where(x => x.TreeName == treeName).ToListAsync();
        }
    }
}
