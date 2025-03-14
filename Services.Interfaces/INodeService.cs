using Models;
using Models.Dto;

namespace Services.Interfaces
{
    public interface INodeService
    {
        Task<Node> AddNodeAsync(AddNodeDto dto);
        Task DeleteNodeAsync(DeleteNodeDto dto);
        Task RenameNodeAsync(RenameNodeDto dto);
    }
}
