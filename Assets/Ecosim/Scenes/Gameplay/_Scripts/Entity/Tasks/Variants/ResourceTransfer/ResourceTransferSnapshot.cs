using System;

namespace Ecosim
{
    [Serializable]
    public class ResourceTransferSnapshot : ITaskSnapshot
    {
        public readonly long DestinationStorageId;
        public readonly long ResourceId;
        public readonly int Amount;
        public readonly float CurrentTime;

        public ResourceTransferSnapshot(long destinationStorageId, long resourceId, int amount, float currentTime)
        {
            DestinationStorageId = destinationStorageId;
            ResourceId = resourceId;
            Amount = amount;
            CurrentTime = currentTime;
        }

        public IEntityTask CreateTask(WorldContext context, Entity owner)
        {
            var p = new ResourceTransferParams { 
                DestinationStorageId = this.DestinationStorageId, 
                ResourceId = this.ResourceId,
                Amount = this.Amount,
                CurrentTime = this.CurrentTime,  
            };
            
            return context.TaskFactory.Create(TaskVariants.TransferResource, owner, p);
        } 
    }
}
