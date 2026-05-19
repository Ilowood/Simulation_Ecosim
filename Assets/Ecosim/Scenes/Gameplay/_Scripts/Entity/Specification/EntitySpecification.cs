using System.Collections.Generic;
using UnityEngine;

namespace Ecosim
{
    [CreateAssetMenu(menuName = "Ecosim/Entity/Specification", fileName = "EntitySpecification")]
    public class EntitySpecification : ScriptableObject
    {
        [ReadOnly, SerializeField] private long _specId; 

        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public EntityType Type { get; private set; }

        [Space, SerializeField] public BehaviourSpecification Behaviour;
        [SerializeField] public List<Specification> Configuration;

        public long SpecId => _specId;

#if UNITY_EDITOR
        private void Awake()
        {
            if (_specId == 0)
            {
                GenerateUniqueId();
            }
        }

        public void GenerateUniqueId()
        {
            var gb = System.Guid.NewGuid().ToByteArray();
            _specId = System.BitConverter.ToInt64(gb, 0);
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
    }
}
