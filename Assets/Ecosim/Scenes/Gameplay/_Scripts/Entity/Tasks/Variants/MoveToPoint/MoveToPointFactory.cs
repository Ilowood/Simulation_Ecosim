namespace Ecosim
{
    public class MoveToPointFactory : ITaskFactory
    {
        public TaskVariants Variant => TaskVariants.Move;

        public IEntityTask Create(Entity owner, ITaskParams parameters)
        {
            if (parameters is MoveToPointParams p)
                return Create(owner, p);
            
            throw new System.ArgumentException("Invalid params");
        }

        public IEntityTask Create(Entity owner, MoveToPointParams parameters)
        {
            return new MoveToPointTask(owner, parameters.Destination);
        }
    }
}
