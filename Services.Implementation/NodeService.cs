using DAL.Interfaces;
using Models;
using Models.Dto;
using Models.Exceptions;
using Services.Interfaces;

namespace Services.Implementation
{
    public class NodeService : INodeService
    {
        private readonly INodeRepository nodeRepository;
        private readonly ITreeRepository treeRepository;

        public NodeService(INodeRepository nodeRepository, ITreeRepository treeRepository)
        {
            this.nodeRepository = nodeRepository;
            this.treeRepository = treeRepository;
        }

        public async Task<Node> AddNodeAsync(AddNodeDto dto)
        {
            var treeId = await ValidateTreeExistsAsync(dto.TreeName);

            var existingNodes = await nodeRepository.GetNodesByTreeNameAsync(dto.TreeName);            
            await ValidateNodeName(dto.NodeName, dto.ParentNodeId, existingNodes);

            if (dto.ParentNodeId != null)
            {
                var parentNode = existingNodes.FirstOrDefault(x => x.Id == dto.ParentNodeId);
                if (parentNode == null)
                {
                    throw new NodeNotFoundException();
                }
            }

            var node = new Node()
            {
                Name = dto.NodeName,
                TreeName = dto.TreeName,
                ParentId = dto.ParentNodeId,
                TreeId = treeId
            };

            return await nodeRepository.AddNodeAsync(node);
        }

        public async Task DeleteNodeAsync(DeleteNodeDto dto)
        {
            await ValidateTreeExistsAsync(dto.TreeName);

            var existingNodes = await nodeRepository.GetNodesByTreeNameAsync(dto.TreeName);
            var targetNode = existingNodes.FirstOrDefault(x => x.Id == dto.NodeId);
            if (targetNode == null)
            {
                throw new NodeNotFoundException();
            }

            if (existingNodes.Any(x => x.ParentId == dto.NodeId))
            {
                throw new DeleteChildrenException();
            }

            await nodeRepository.DeleteNodeAsync(targetNode);
        }

        public async Task RenameNodeAsync(RenameNodeDto dto)
        {
            await ValidateTreeExistsAsync(dto.TreeName);

            var existingNodes = await nodeRepository.GetNodesByTreeNameAsync(dto.TreeName);
            var targetNode = existingNodes.FirstOrDefault(x => x.Id == dto.NodeId);
            if (targetNode == null)
            {
                throw new NodeNotFoundException();
            }
            
            await ValidateNodeName(dto.NewNodeName, targetNode.ParentId, existingNodes);

            targetNode.Name = dto.NewNodeName;
            await nodeRepository.RenameNodeAsync(targetNode);
        }

        private async Task<int> ValidateTreeExistsAsync(string treeName)
        {
            var targetTree = await treeRepository.GetTreeAsync(treeName);
            if (targetTree == null)
            {
                throw new TreeNotFoundException();
            }
            
            return targetTree.Id;
        }

        private async Task ValidateNodeName(string nodeName, int? parentNodeId, IEnumerable<Node> existingNodes)
        {
            var siblings = existingNodes.Where(x => x.ParentId == parentNodeId);
            var duplicateName = siblings.FirstOrDefault(x => x.Name == nodeName);
            if (duplicateName != null)
            {
                throw new UniqueNameException();
            }

            if (string.IsNullOrEmpty(nodeName))
            {
                throw new NameEmptyException();
            }
        }
    }
}
