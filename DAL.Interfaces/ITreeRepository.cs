using Models;

namespace DAL.Interfaces
{
    public interface ITreeRepository
    {
        Task<Tree> AddTreeAsync(Tree tree);

        Task<ICollection<Tree>> GetTreesAsync();

        Task<Tree> GetTreeAsync(string name);
    }
}
