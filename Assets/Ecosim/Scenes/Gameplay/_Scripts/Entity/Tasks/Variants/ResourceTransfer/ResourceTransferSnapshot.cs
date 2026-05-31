using System;

namespace Ecosim
{
    [Serializable]
    public class ResourceTransferSnapshot : ITaskSnapshot
    {
        public ResourceTransferParams Params;

        public ResourceTransferSnapshot(long destinationStorageId, long resourceId, int amount, float currentTime)
        {
            Params = new ResourceTransferParams
            {
                DestinationStorageId = destinationStorageId,
                ResourceId = resourceId,
                Amount = amount,
                CurrentTime = currentTime
            };
        }

        public IEntityTask CreateTask(WorldContext context, Entity owner)
        {
            return context.TaskFactory.Create(TaskVariants.TransferResource, context, owner, Params);
        } 
    }
}
