using System;
using System.Collections.Generic;

namespace Kuro.UnityUtils.DesignPatterns
{
    /// <summary>
    /// Base class for all states in the state machine.
    /// </summary>
    /// <typeparam name="T">The type of the state machine owner.</typeparam>
    public abstract class State<T>
    {
        public virtual void OnEnter(T owner) { }
        public virtual void OnExit(T owner) { }
        public virtual void OnDynamicStay(T owner) { }
        public virtual void OnFixedStay(T owner) { }
    }

    /// <summary>
    /// A generic finite state machine implementation that manages state transitions and updates.
    /// </summary>
    /// <typeparam name="T">The type of the state machine owner.</typeparam>
    public class StateMachine<T>
    {
        /// <summary>
        /// Delegate for state change event handling.
        /// </summary>
        /// <param name="previousState">The state being exited.</param>
        /// <param name="newState">The state being entered.</param>
        public delegate void StateChangedHandler(State<T> previousState, State<T> newState);

        /// <summary>
        /// Event triggered when state changes occur.
        /// </summary>
        public event StateChangedHandler OnStateChanged;

        /// <summary>
        /// Gets the current active state.
        /// </summary>
        public State<T> CurrentState => _currentState;

        /// <summary>
        /// Gets a read-only view of all registered states.
        /// </summary>
        public IReadOnlyDictionary<string, State<T>> States => _states;

        private readonly T _owner;
        private State<T> _currentState;
        private readonly Dictionary<string, State<T>> _states = new();

        /// <summary>
        /// Initializes a new instance of the StateMachine.
        /// </summary>
        /// <param name="owner">The owner/context of this state machine.</param>
        /// <exception cref="ArgumentNullException">Thrown when owner is null.</exception>
        public StateMachine(T owner)
            => _owner = owner ?? throw new ArgumentNullException(nameof(owner));

        /// <summary>
        /// Adds a new state to the state machine.
        /// </summary>
        /// <typeparam name="TState">The type of state to add.</typeparam>
        /// <param name="state">The state instance to add.</param>
        /// <exception cref="ArgumentNullException">Thrown when state is null.</exception>
        public void AddState<TState>(TState state) where TState : State<T>
            => _states[typeof(TState).Name] = state ?? throw new ArgumentNullException(nameof(state));

        /// <summary>
        /// Changes the current state to the specified state type.
        /// </summary>
        /// <typeparam name="TState">The type of state to change to.</typeparam>
        /// <exception cref="Exception">Thrown when the specified state type is not found in the state machine.</exception>
        public virtual void ChangeState<TState>() where TState : State<T>
        {
            _currentState?.OnExit(_owner);

            if (_states.TryGetValue(typeof(TState).Name, out var newState))
            {
                var previousState = _currentState;
                _currentState = newState;
                _currentState?.OnEnter(_owner);

                OnStateChanged?.Invoke(previousState, _currentState);
            }
            else
            {
                throw new Exception($"State {typeof(TState)} not found in FSM.");
            }
        }

        /// <summary>
        /// Executes the dynamic update logic of the current state.
        /// </summary>
        public void DynamicStay()
            => _currentState?.OnDynamicStay(_owner);

        /// <summary>
        /// Executes the fixed update logic of the current state.
        /// </summary>
        public void FixedStay()
            => _currentState?.OnFixedStay(_owner);
    }
}
