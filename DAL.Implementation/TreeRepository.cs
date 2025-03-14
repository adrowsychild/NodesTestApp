using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using Models;

namespace DAL.Implementation
{
    public class TreeRepository : ITreeRepository
    {
        private readonly DataDbContext context;

        public TreeRepository(DataDbContext context)
        {
            this.context = context;
        }

        public async Task<Tree> AddTreeAsync(Tree tree)
        {
            context.Trees.Add(tree);
            await context.SaveChangesAsync();
            return tree;
        }

        public async Task<ICollection<Tree>> GetTreesAsync()
        {
            var result = await GetTreesRawAsync();
            return result.Select(BuildTreeCopyRecursive).ToList();
        }

        public async Task<ICollection<Tree>> GetTreesRawAsync()
        {
            return await context.Trees.Include(t => t.Nodes).ToListAsync();
        }

        public async Task<Tree> GetTreeAsync(string name)
        {
            var result = await GetTreeRawAsync(name);
            return BuildTreeCopyRecursive(result);
        }

        public async Task<Tree> GetTreeRawAsync(string name)
        {
            return await context.Trees.Include(t => t.Nodes).FirstOrDefaultAsync(x => x.Name == name);
        }

        private Tree BuildTreeCopyRecursive(Tree tree)
        {
            if (tree == null)
            {
                return null;
            }

            return new Tree()
            {
                Id = tree.Id,
                Name = tree.Name,
                Nodes = BuildTree(tree.Nodes)
            };
        }

        private List<Node> BuildTree(List<Node> nodes, int? parentId = null)
        {
            return nodes.Where(n => n.ParentId == parentId)
                .Select(n => new Node
                {
                    Id = n.Id,
                    Name = n.Name,
                    ParentId = n.ParentId,
                    TreeId = n.TreeId,
                    TreeName = n.TreeName,
                    Nodes = BuildTree(nodes, n.Id)
                })
                .ToList();
        }        
    }
}
