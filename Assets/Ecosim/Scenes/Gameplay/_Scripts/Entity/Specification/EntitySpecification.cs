using System.Collections.Generic;
using UnityEngine;

namespace Ecosim
{
    [CreateAssetMenu(menuName = "Ecosim/Entity/Specification", fileName = "EntitySpecification")]
    public class EntitySpecification : ScriptableObject
    {
        [ReadOnly, SerializeField] private long _id; 

        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public EntityType Type { get; private set; }

        [field: Space, SerializeField] public Sprite Icon { get; private set; }

        [Space, SerializeField] public BehaviourSpecification Behaviour;
        [SerializeField] public List<Specification> Configuration;

        public long Id => _id;

#if UNITY_EDITOR
        private void Awake()
        {
            if (_id == 0)
            {
                GenerateUniqueId();
            }
        }

        public void GenerateUniqueId()
        {
            var gb = System.Guid.NewGuid().ToByteArray();
            _id = System.BitConverter.ToInt64(gb, 0);
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
    }
}
