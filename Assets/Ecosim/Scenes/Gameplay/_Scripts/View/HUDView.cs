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
        [SerializeField] private Button _save;

        private Dictionary<EntityType, Action> _deathHandlers;

        public void Init(WorldState state)
        {
            _speed.onClick.AddListener(() => state.ToggleSpeed());
            _pause.onClick.AddListener(() => state.PauseState());
            _save.onClick.AddListener(() => state.SaveWorld());

            _deathHandlers = new Dictionary<EntityType, Action>
            {
                // { EntityType.Unit, AddDeadWorkers },
                // { EntityType.Unit, AddDeadPredators },
                // { EntityType.Warehouse, AddEatenFood }
            };
        }

        public void Close(World simulation)
        {
            simulation.OnEntityRemoved -= EntityRemoved;
            base.Close();
        }

        public void Open(World simulation)
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
