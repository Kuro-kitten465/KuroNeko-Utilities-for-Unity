using System;
using UnityEngine;

namespace Kuro.UnityUtils.DesignPatterns
{
    public abstract class ScriptableStateRunner : MonoBehaviour
    {
        public delegate void StateChangedHandler(ScriptableState previousState, ScriptableState newState);
        public event StateChangedHandler OnStateChanged;

        [Header("State Definition")]
        [SerializeField] protected ScriptableStateDefinition _stateDefinition;

        private ScriptableState _currentState;
        public ScriptableState CurrentState => _currentState;

        protected virtual void Awake()
        {
            if (_stateDefinition == null) throw new ArgumentNullException(nameof(_stateDefinition));

            _currentState = _stateDefinition.InitialState;
            _currentState.OnEnter(this);
        }

        protected virtual void Update() => _currentState.OnDynamicStay(this);
        protected virtual void FixedUpdate() => _currentState.OnFixedStay(this);

        public void ChangeState(ScriptableState newState)
        {
            if (newState == null) throw new ArgumentNullException(nameof(newState));

            if (_stateDefinition.CanTransition(_currentState, newState))
            {
                _currentState.OnExit(this);
                _currentState = newState;
                _currentState.OnEnter(this);
                OnStateChanged?.Invoke(_currentState, newState);
            }
            else
            {
                Debug.LogWarning($"Cannot transition from {_currentState} to {newState}");
            }
        }
    }
}
