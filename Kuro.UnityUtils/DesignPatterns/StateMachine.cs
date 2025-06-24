using System;
using System.Collections.Generic;

namespace Kuro.UnityUtils.DesignPatterns
{
    public interface IState<TOwner>
    {
        public void OnEnter(TOwner owner);
        public void OnExit(TOwner owner);
        public void OnUpdate(TOwner owner);
        public void OnFixedUpdate(TOwner owner);
    }

    public class StateMachine<TOwner>
    {
        public delegate void StateChangedHandler(IState<TOwner> previousState, IState<TOwner> newState);
        public event StateChangedHandler OnStateChanged;

        public IState<TOwner> CurrentState => _currentState;
        public IReadOnlyDictionary<Type, IState<TOwner>> States => _states;

        private readonly TOwner _owner;
        private IState<TOwner> _currentState;
        private readonly Dictionary<Type, IState<TOwner>> _states = new();

        public StateMachine(TOwner owner) => _owner = owner ?? throw new ArgumentNullException(nameof(owner));

        public void AddState<TState>(TState state) where TState : IState<TOwner>
            => _states[typeof(TState)] = state ?? throw new ArgumentNullException(nameof(state));

        public virtual void ChangeState<TState>(TState state = default) where TState : IState<TOwner>
        {
            _currentState?.OnExit(_owner);

            if (_states.TryGetValue(typeof(TState), out var newState))
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

        public void DynamicStay() => _currentState?.OnUpdate(_owner);
        public void FixedStay() => _currentState?.OnFixedUpdate(_owner);
    }
}
