using System;

namespace Ecosim
{
    [Serializable]
    public class MoveToPointSnapshot : ITaskSnapshot
    {
        public IEntityTask CreateTask(Entity root)
        {
            var navigation = root.Get<NavigationComponent>();
            return new MoveToPointTask(root, navigation.Destination);
        } 
    }
}
