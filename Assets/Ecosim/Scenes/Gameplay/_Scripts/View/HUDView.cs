using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ecosim
{
    public class HUDView : Screen
    {
        [Header("TMP")]
        [SerializeField] private TMP_Text _counterDeadWorkers;
        [SerializeField] private TMP_Text _counterDeadPredators;
        [SerializeField] private TMP_Text _counterEatenFood;

        [Header("Buttons")]
        [SerializeField] private Button _speed;
        [SerializeField] private Button _pause;

        private Dictionary<EntityType, Action> _deathHandlers;

        public void Init(SimulationState state)
        {
            _speed.onClick.AddListener(() => state.ToggleSpeed());
            _pause.onClick.AddListener(() => state.PauseState());

            _deathHandlers = new Dictionary<EntityType, Action>
            {
                { EntityType.Worker, AddDeadWorkers },
                { EntityType.Predator, AddDeadPredators },
                { EntityType.Resource, AddEatenFood }
            };
        }

        public void Close(Simulation simulation)
        {
            simulation.OnEntityRemoved -= EntityRemoved;
            base.Close();
        }

        public void Open(Simulation simulation)
        {
            simulation.OnEntityRemoved += EntityRemoved;
            base.Open();
        }

        public void ResetView()
        {
            _counterDeadWorkers.text = $"{0}";
            _counterDeadPredators.text = $"{0}";
            _counterEatenFood.text = $"{0}";
        }

        private void EntityRemoved(EntityType entityType)
        {
            if (_deathHandlers.TryGetValue(entityType, out var handler))
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

        private void AddEatenFood()
        {
            var count = Convert.ToInt32(_counterEatenFood.text) + 1;
            _counterEatenFood.text = $"{count}";
        }
    }
}
