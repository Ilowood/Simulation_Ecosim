using System;
using Zenject;

namespace Ecosim
{
    public class ResourceTransferFactory : ITaskFactory
    {
        private StorageSystem _storageSystem;

        [Inject]
        public void Init(StorageSystem storageSystem)
        {
            _storageSystem = storageSystem;
        }

        public TaskVariants Variant => TaskVariants.TransferResource;
        
        public IEntityTask Create(WorldContext context, Entity owner, ITaskParams parameters) 
        {
            if (parameters is ResourceTransferParams p) 
                return Create(context, owner, p);
            
            throw new ArgumentException("Invalid params");
        }
        
        public IEntityTask Create(WorldContext context, Entity owner, ResourceTransferParams parameters)
        {
            return new ResourceTransferTask(
                _storageSystem,
                owner, 
                context.Registry.GetById(parameters.DestinationStorageId), 
                parameters.ResourceId,
                parameters.Amount,
                parameters.CurrentTime
            );
        }
    }
}
