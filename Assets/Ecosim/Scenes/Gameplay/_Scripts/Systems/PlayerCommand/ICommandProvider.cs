using UnityEngine;

namespace Ecosim
{
    public interface ICommandProvider
    {
        int Priority { get; }
        
        bool CanExecute(Entity targetEntity, RaycastHit hit);
        void Create(Entity targetEntity, RaycastHit hit);
    }
}
