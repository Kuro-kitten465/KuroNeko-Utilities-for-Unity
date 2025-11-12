using System;
using System.Collections;
using System.Collections.Generic;

namespace Kuro.UnityUtils.DesignPatterns
{
    public interface IState<TOwner>
    {
        void OnEnter(TOwner owner);
        void OnUpdate(TOwner owner, float deltaTime);
        void OnFixedUpdate(TOwner owner, float fixedDeltaTime);
        void OnExit(TOwner owner);
    }

    public interface IEnumeratorState<TOwner>
    {
        void OnEnter(TOwner owner);
        IEnumerator Run(TOwner owner);
        void OnExit(TOwner owner);
    }

    public class StateMachine<TOwner>
    {
        public TOwner Owner { get; private set; }
        public IState<TOwner> CurrentState { get; private set; }
        public IState<TOwner> PreviousState { get; private set; }
        public bool IsRunning { get; private set; }
        public bool IsPaused { get; private set; }

        private readonly Dictionary<Type, IState<TOwner>> _states = new();
        private IState<TOwner> _initialState;

        public StateMachine(TOwner owner)
        {
            Owner = owner;
            CurrentState = null;
            PreviousState = null;
            _initialState = null;
        }

        public void SetInitialState<TState>() where TState : IState<TOwner>
        {
            if (_states.Count <= 0) throw new NullReferenceException($"You must add state before set initial state.");
            if (!_states.TryGetValue(typeof(TState), out var state)) throw new NullReferenceException($"Couldn't found {typeof(TState)} in {this} state machine.");

            _initialState = state;
        }

        public void AddState(IState<TOwner> state)
        {
            _states[state.GetType()] = state;
        }

        public void ChangeState<TState>() where TState : IState<TOwner>
        {
            ChangeState(typeof(TState));
        }

        public void ChangeState(Type newState)
        {
            if (_initialState == null) throw new NullReferenceException("State Machine must initialize first");
            if (!_states.TryGetValue(newState, out var state)) throw new NullReferenceException($"Couldn't found {newState} in {this} state machine.");

            CurrentState?.OnExit(Owner);
            PreviousState = CurrentState;
            CurrentState = state;
            CurrentState.OnEnter(Owner);
        }

        public void Update(float deltaTime)
        {
            if (!IsRunning || IsPaused) return;
            CurrentState?.OnUpdate(Owner, deltaTime);
        }

        public void FixedUpdate(float fixedDeltaTime)
        {
            if (!IsRunning || IsPaused) return;
            CurrentState?.OnFixedUpdate(Owner, fixedDeltaTime);
        }

        // ------------------------------
        // Lifecycle Controls
        // ------------------------------

        public void Start()
        {
            if (IsRunning) return;

            IsRunning = true;
            IsPaused = false;

            if (_initialState == null) throw new NullReferenceException("State Machine must initialize first");

            CurrentState ??= _initialState;
            CurrentState?.OnEnter(Owner);
        }

        public void Stop()
        {
            if (!IsRunning) return;

            CurrentState?.OnExit(Owner);
            IsRunning = false;
            IsPaused = false;
            CurrentState = null;
            PreviousState = null;
        }

        public void Pause()
        {
            if (!IsRunning) return;
            IsPaused = true;
        }

        public void Resume()
        {
            if (!IsRunning) return;
            IsPaused = false;
        }
    }

    public class EnumeratorStateMachine<TOwner> where TOwner : MonoEnumeratorStateMachine<TOwner>
    {
        public TOwner Owner { get; private set; }
        public IEnumeratorState<TOwner> CurrentState { get; private set; }
        public IEnumeratorState<TOwner> PreviousState { get; private set; }
        public bool IsRunning { get; private set; }

        private readonly Dictionary<Type, IEnumeratorState<TOwner>> _states = new();
        private IEnumeratorState<TOwner> _initialState;

        public EnumeratorStateMachine(TOwner owner)
        {
            Owner = owner;
            CurrentState = null;
            PreviousState = null;
            _initialState = null;
        }

        public void SetInitialState<TState>() where TState : IEnumeratorState<TOwner>
        {
            if (_states.Count <= 0) throw new NullReferenceException($"You must add state before set initial state.");
            if (!_states.TryGetValue(typeof(TState), out var state)) throw new NullReferenceException($"Couldn't found {typeof(TState)} in {this} state machine.");

            _initialState = state;
        }

        public void AddState(IEnumeratorState<TOwner> state)
        {
            _states[state.GetType()] = state;
        }

        public void ChangeState<TState>() where TState : IEnumeratorState<TOwner>
        {
            ChangeState(typeof(TState));
        }

        public void ChangeState(Type newState)
        {
            if (_initialState == null) throw new NullReferenceException("State Machine must initialize first");
            if (!_states.TryGetValue(newState, out var state)) throw new NullReferenceException($"Couldn't found {newState} in {this} state machine.");

            CurrentState?.OnExit(Owner);
            PreviousState = CurrentState;
            CurrentState = state;
            CurrentState?.OnEnter(Owner);
        }

        public IEnumerator Run()
        {
            if (!IsRunning) yield return null;
            
            while (IsRunning)
            {
                yield return CurrentState?.Run(Owner);
            }
        }

        // ------------------------------
        // Lifecycle Controls
        // ------------------------------

        public void Start()
        {
            if (IsRunning) return;

            IsRunning = true;

            if (_initialState == null) throw new NullReferenceException("State Machine must initialize first");

            CurrentState ??= _initialState;
            CurrentState?.OnEnter(Owner);
        }

        public void Stop()
        {
            if (!IsRunning) return;

            CurrentState?.OnExit(Owner);
            IsRunning = false;
            CurrentState = null;
            PreviousState = null;
        }
    }
}
