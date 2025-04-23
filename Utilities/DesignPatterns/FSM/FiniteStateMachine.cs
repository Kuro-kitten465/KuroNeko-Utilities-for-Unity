using System;
using System.Collections.Generic;
using static UnityEngine.Debug;

namespace Kuro.UnityUtils.DesignPatterns
{ /*
    /// <summary>
    /// Core Finite State Machine implementation (non-MonoBehaviour)
    /// </summary>
    public class FiniteStateMachine<T>
    {
        private readonly Dictionary<Type, IState<T>> _states = new();
        private readonly Dictionary<string, IState<T>> _statesByName = new();
        
        private IState<T> _currentState;
        private readonly T _entity;

        // Transition events
        public event EventHandler<StateChangeEventArgs<T>> OnStateChanged;

        public FiniteStateMachine(T entity)
            => _entity = entity;

        /// <summary>
        /// Add a state to the FiniteStateMachine
        /// </summary>
        public FiniteStateMachine<T> AddState(IState<T> state)
        {
            _states[state.GetType()] = state;
            
            // If it's our custom State implementation with a name
            if (state is State<T> namedState)
                _statesByName[namedState.Name] = state;
            
            return this;
        }

        /// <summary>
        /// Create and add a new state with the given name
        /// </summary>
        public State<T> CreateState(string stateName)
        {
            var state = new State<T>(stateName);
            AddState(state);
            return state;
        }

        /// <summary>
        /// Set the initial state of the FiniteStateMachine
        /// </summary>
        public FiniteStateMachine<T> SetInitialState<TState>() where TState : IState<T>
        {
            var stateType = typeof(TState);
            if (_states.TryGetValue(stateType, out IState<T> state))
            {
                _currentState = state;
                _currentState.Enter(_entity);
            }
            else
            {
                LogError($"State {stateType.Name} not found in the FiniteStateMachine");
            }
            return this;
        }

        /// <summary>
        /// Set the initial state by name
        /// </summary>
        public FiniteStateMachine<T> SetInitialState(string stateName)
        {
            if (_statesByName.TryGetValue(stateName, out IState<T> state))
            {
                _currentState = state;
                _currentState.Enter(_entity);
            }
            else
            {
                LogError($"State {stateName} not found in the FiniteStateMachine");
            }
            return this;
        }

        /// <summary>
        /// Transition to a new state by type
        /// </summary>
        public void ChangeState<TState>() where TState : IState<T>
        {
            Type stateType = typeof(TState);

            if (_states.TryGetValue(stateType, out IState<T> state))
                TransitionToState(state);
            else
                LogError($"State {stateType.Name} not found in the FiniteStateMachine");
        }

        /// <summary>
        /// Transition to a new state by name
        /// </summary>
        public void ChangeState(string stateName)
        {
            if (_statesByName.TryGetValue(stateName, out IState<T> state))
                TransitionToState(state);
            else
                LogError($"State {stateName} not found in the FiniteStateMachine");
        }

        protected void TransitionToState(IState<T> newState)
        {
            IState<T> previousState = _currentState;
            
            _currentState?.Exit(_entity);

            _currentState = newState;
            
            OnStateChanged?.Invoke(this, new StateChangeEventArgs<T>(previousState, newState, _entity));
            
            _currentState.Enter(_entity);
        }

        /// <summary>
        /// Update the current state
        /// </summary>
        public void Update()
            => _currentState?.Update(_entity);

        /// <summary>
        /// Fixed update for the current state
        /// </summary>
        public void FixedUpdate()
            => _currentState?.FixedUpdate(_entity);

        /// <summary>
        /// Get the current state
        /// </summary>
        public IState<T> GetCurrentState() 
            => _currentState;

        /// <summary>
        /// Check if a state exists in the FiniteStateMachine
        /// </summary>
        public bool HasState<TState>() where TState : IState<T>
            => _states.ContainsKey(typeof(TState));

        /// <summary>
        /// Check if a state exists in the FiniteStateMachine by name
        /// </summary>
        public bool HasState(string stateName)
            => _statesByName.ContainsKey(stateName);

        /// <summary>
        /// Get a state by type
        /// </summary>
        public TState GetState<TState>() where TState : IState<T>
        {
            if (_states.TryGetValue(typeof(TState), out IState<T> state))
                return (TState)state;

            return default;
        }

        /// <summary>
        /// Get a state by name
        /// </summary>
        public IState<T> GetState(string stateName)
        {
            if (_statesByName.TryGetValue(stateName, out IState<T> state))
                return state;

            return null;
        }
    }*/
}