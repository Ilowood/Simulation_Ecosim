using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BugColony
{
    public class HUDView : Screen
    {
        [Header("TMP")]
        [SerializeField] private TMP_Text _counterDeadWorkers;
        [SerializeField] private TMP_Text _counterDeadPredators;

        [Header("Buttons")]
        [SerializeField] private Button _speed;
        [SerializeField] private Button _pause;

        private Dictionary<EntityType, Action> _deathHandlers;

        public void Init(GameLoopState state, Simulation simulation)
        {
            _speed.onClick.AddListener(() => state.ToggleSpeed());
            _pause.onClick.AddListener(() => state.PauseState());

            _deathHandlers = new Dictionary<EntityType, Action>
            {
                { EntityType.Worker, AddDeadWorkers },
                { EntityType.Predator, AddDeadPredators }
            };

            simulation.OnEntityRemoved += EntityRemoved;
        }

        public void Deinit(Simulation simulation)
        {
            _speed.onClick.RemoveAllListeners();
            _pause.onClick.RemoveAllListeners();
            
            simulation.OnEntityRemoved -= EntityRemoved;
        }

        private void EntityRemoved(Entity entity)
        {
            if (_deathHandlers.TryGetValue(entity.Type, out var handler))
            {

                handler.Invoke();
            }
        }

        private void AddDeadWorkers()
        {
            var count = Convert.ToInt32(_counterDeadWorkers.text) + 1;
            _counterDeadWorkers.text = $"{count}";
        }

        private void AddDeadPredators()
        {
            var count = Convert.ToInt32(_counterDeadPredators.text) + 1;
            _counterDeadPredators.text = $"{count}";
        }
    }
}
