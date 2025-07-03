using System;
using System.Linq;
using System.Collections.Generic;

namespace Kuro.UnityUtils.DesignPatterns
{
    #region State Interfaces
    public interface IStateEnter<TOwner>
    {
        void OnEnter(TOwner owner);
    }

    public interface IStateExit<TOwner>
    {
        void OnExit(TOwner owner);
    }

    public interface IStateUpdate<TOwner>
    {
        void OnUpdate(TOwner owner);
    }

    public interface IStateFixedUpdate<TOwner>
    {
        void OnFixedUpdate(TOwner owner);
    }

    public interface IState<TOwner> :
        IStateEnter<TOwner>,
        IStateExit<TOwner>,
        IStateUpdate<TOwner>,
        IStateFixedUpdate<TOwner>
    { }

    public readonly struct StateCache<TOwner>
    {
        public readonly object StateObject;
        public readonly IStateEnter<TOwner> Enter;
        public readonly IStateExit<TOwner> Exit;
        public readonly IStateUpdate<TOwner> Update;
        public readonly IStateFixedUpdate<TOwner> FixedUpdate;

        public StateCache(object stateObject)
        {
            StateObject = stateObject ?? throw new ArgumentNullException(nameof(stateObject));
            Enter = stateObject as IStateEnter<TOwner>;
            Exit = stateObject as IStateExit<TOwner>;
            Update = stateObject as IStateUpdate<TOwner>;
            FixedUpdate = stateObject as IStateFixedUpdate<TOwner>;
        }

        public void OnEnter(TOwner owner) => Enter?.OnEnter(owner);
        public void OnExit(TOwner owner) => Exit?.OnExit(owner);
        public void OnUpdate(TOwner owner) => Update?.OnUpdate(owner);
        public void OnFixedUpdate(TOwner owner) => FixedUpdate?.OnFixedUpdate(owner);
    }
    #endregion

    #region StateMachine Core
    public class StateMachine<TOwner>
    {
        public delegate void StateChangedHandler(object previousState, object newState);
        public event StateChangedHandler OnStateChanged;

        public object CurrentState => _currentState.StateObject;

        private readonly TOwner _owner;
        private StateCache<TOwner> _currentState;
        private readonly Dictionary<Type, StateCache<TOwner>> _states = new();

        public StateMachine(TOwner owner) => _owner = owner ?? throw new ArgumentNullException(nameof(owner));

        public StateMachine<TOwner> Add<TState>(TState state) where TState : new()
        {
            if (!IsValidState(typeof(TState)))
                throw new Exception($"State {typeof(TState)} does not implement required interfaces.");

            if (_states.ContainsKey(typeof(TState)))
                throw new Exception($"State {typeof(TState)} already exists in FSM.");

            var stateCache = new StateCache<TOwner>(state);
            _states.Add(typeof(TState), stateCache);

            return this;
        }
        public virtual void ChangeState<TState>(TState state = default)
        {
            if (!IsValidState(typeof(TState)))
                throw new Exception($"State {typeof(TState)} does not implement required interfaces.");

            if (!_states.TryGetValue(typeof(TState), out var newState))
                throw new Exception($"State {typeof(TState)} not found in FSM.");

            if (_currentState.StateObject != null)
                _currentState.OnExit(_owner);

            var previousState = _currentState.StateObject;
            _currentState = newState;
            _currentState.OnEnter(_owner);

            OnStateChanged?.Invoke(previousState, _currentState.StateObject);
        }

        public void DynamicStay() => _currentState.OnUpdate(_owner);
        public void FixedStay() => _currentState.OnFixedUpdate(_owner);

        private bool IsValidState(Type stateType) =>
            stateType.GetInterfaces().Any(i =>
                i == typeof(IStateEnter<TOwner>) ||
                i == typeof(IStateExit<TOwner>) ||
                i == typeof(IStateUpdate<TOwner>) ||
                i == typeof(IStateFixedUpdate<TOwner>) ||
                i == typeof(IState<TOwner>));
    }
    #endregion
}
