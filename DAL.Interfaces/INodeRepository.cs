using Models;

namespace DAL.Interfaces
{
    public interface INodeRepository
    {
        Task<ICollection<Node>> GetNodesByTreeNameAsync(string treeName);

        Task<Node> AddNodeAsync(Node node);

        Task DeleteNodeAsync(Node node);

        Task RenameNodeAsync(Node node);
    }
}
