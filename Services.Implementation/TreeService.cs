using DAL.Interfaces;
using Models;
using Models.Exceptions;
using Services.Interfaces;

namespace Services.Implementation
{
    public class TreeService : ITreeService
    {
        private readonly ITreeRepository treeRepository;

        public TreeService(ITreeRepository treeRepository)
        {
            this.treeRepository = treeRepository;
        }

        public async Task<Tree> AddTreeAsync(string name)
        {
            var existingTrees = await treeRepository.GetTreeAsync(name);
            if (existingTrees != null)
            {
                throw new UniqueNameException();
            }

            if (string.IsNullOrEmpty(name))
            {
                throw new NameEmptyException();
            }

            var tree = new Tree() { Name = name };

            return await treeRepository.AddTreeAsync(tree);
        }

        public async Task<Tree> GetTreeAsync(string name)
        {
            var result = await treeRepository.GetTreeAsync(name);
            if (result == null)
            {
                throw new TreeNotFoundException();
            }

            return result;
        }

        public Task<ICollection<Tree>> GetTreesAsync()
        {
            return treeRepository.GetTreesAsync();
        }
    }
}
