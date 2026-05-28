using System;
using Zenject;

namespace Ecosim
{
    public class ResourceTransferFactory : ITaskFactory
    {
        private IEntityRegistry _registry;
        private StorageSystem _storageSystem;

        [Inject]
        public void Init(IEntityRegistry registry, StorageSystem storageSystem)
        {
            _registry = registry;
            _storageSystem = storageSystem;
        }

        public TaskVariants Variant => TaskVariants.TransferResource;
        
        public IEntityTask Create(Entity owner, ITaskParams parameters) 
        {
            if (parameters is ResourceTransferParams p) 
                return Create(owner, p);
            
            throw new ArgumentException("Invalid params");
        }
        
        public IEntityTask Create(Entity owner, ResourceTransferParams parameters)
        {
            return new ResourceTransferTask(
                _storageSystem,
                owner, 
                _registry.GetById(parameters.DestinationStorageId), 
                parameters.ResourceId,
                parameters.Amount,
                parameters.CurrentTime
            );
        }
    }
}
