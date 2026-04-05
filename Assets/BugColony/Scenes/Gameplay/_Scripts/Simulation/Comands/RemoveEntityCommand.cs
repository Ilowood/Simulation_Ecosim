using System.Collections.Generic;
using UnityEngine;

namespace BugColony
{
    public class RemoveEntityCommand : ISimulationCommand
    {
        private readonly Entity _entity;

        public RemoveEntityCommand(Entity entity)
        {
            _entity = entity;
        }

        public void Execute(Simulation simulation)
        {
            simulation.RemoveEntityImmediate(_entity);
        }
    }
}
