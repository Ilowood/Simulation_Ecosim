using UnityEngine;

namespace Ecosim
{
    public class ResourceTransferTask : IEntityTask
    {
        private readonly Entity _owner;
        private readonly Entity _targetStorageEntity;
        private readonly StorageSystem _storageSystem;
        private readonly long _resourceSpecId;
        
        private StorageComponent _ownerStorage;
        private StorageComponent _targetStorage;

        private int _amount;
        private float _unloadDuration = 2.0f;
        private float _timer;
        
        private bool _isComplete = false;
        public bool IsComplete => _isComplete;

        public TaskVariants Variants => TaskVariants.TransferResource;

        public ResourceTransferTask(StorageSystem storageSystem, Entity owner, Entity targetStorageEntity, long resourceId, int amount, float startTime)
        {
            _storageSystem = storageSystem;
            _owner = owner;
            _targetStorageEntity = targetStorageEntity;
            _resourceSpecId = resourceId;
            _amount = amount;

            _ownerStorage = _owner.Get<StorageComponent>();
            _targetStorage = _targetStorageEntity.Get<StorageComponent>();

            _timer = startTime;
        }

        public void Puase() { }
        public void Resume() { }

        public void Start()
        {
            Debug.Log($"ResourceTransferTask!");
            Debug.Log($"owner has item: {_storageSystem.HasItem(_ownerStorage, _resourceSpecId)}");
            Debug.Log($"target has space:  {_storageSystem.CanAddAny(_targetStorage, _resourceSpecId)}");
        }

        public void Tick(float deltaTime, float scale)
        {
            _timer += deltaTime * scale;

            if (!_isComplete && _timer >= _unloadDuration && _amount >= 0)
            {
                var moved = _storageSystem.TryTransfer(_ownerStorage, _targetStorage, _resourceSpecId, 1);
                _amount -= moved;
                _timer = 0;

                Debug.Log($"кол-во древесины на складе: {_storageSystem.GetItemCount(_targetStorage, _resourceSpecId)}");

                if (_amount == 0)
                {
                    End();
                }
            }
        }

        public void End()
        {
            _isComplete = true;
        }

        public ITaskSnapshot GetSnapshot()
        {
            return new ResourceTransferSnapshot(_owner.Id, _resourceSpecId, _amount, _timer);
        }

        public void Restore(Entity root, ITaskSnapshot snapshot)
        {
            _ownerStorage = root.Get<StorageComponent>();
            _targetStorage = _targetStorageEntity.Get<StorageComponent>();
        }
    }
}
