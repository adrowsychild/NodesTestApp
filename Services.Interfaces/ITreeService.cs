using Models;

namespace Services.Interfaces
{
    public interface ITreeService
    {
        Task<ICollection<Tree>> GetTreesAsync();
        Task<Tree> GetTreeAsync(string name);
        Task<Tree> AddTreeAsync(string name);
    }
}
