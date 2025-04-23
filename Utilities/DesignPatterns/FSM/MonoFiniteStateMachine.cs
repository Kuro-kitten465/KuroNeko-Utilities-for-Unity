using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kuro.UnityUtils.DesignPatterns
{
    /*#region MonoBehaviour Wrapper

    /// <summary>
    /// MonoBehaviour wrapper for the FiniteStateMachine
    /// </summary>
    public class MonoFiniteStateMachine<T> : MonoBehaviour where T : class
    {
        private FiniteStateMachine<T> _FiniteStateMachine;
        private T _entity;
        private readonly Dictionary<State<T>, Coroutine> _activeCoroutines = new();

        /// <summary>
        /// Initialize the MonoFiniteStateMachine with an entity
        /// </summary>
        public void Initialize(T entity)
        {
            _entity = entity;
            _FiniteStateMachine = new FiniteStateMachine<T>(entity);
            _FiniteStateMachine.OnStateChanged += HandleStateChanged;
        }

        protected void HandleStateChanged(object sender, StateChangeEventArgs<T> e)
        {
            // Handle coroutines for state transitions
            if (e.PreviousState is State<T> prevState)
            {
                StopStateCoroutines(prevState);
            }

            if (e.NewState is State<T> newState)
            {
                StartStateCoroutines(newState);
                StartStateAsyncTasks(newState);
            }
        }

        protected void StartStateCoroutines(State<T> state)
        {
            if (state.HasEnterCoroutine)
            {
                var routine = StartCoroutine(state.GetEnterCoroutine(_entity));
                _activeCoroutines[state] = routine;
            }

            if (state.HasUpdateCoroutine)
            {
                var routine = StartCoroutine(RunUpdateCoroutine(state));
                _activeCoroutines[state] = routine;
            }
        }

        protected IEnumerator RunUpdateCoroutine(State<T> state)
        {
            while (true)
            {
                yield return state.GetUpdateCoroutine(_entity);
            }
        }

        protected void StopStateCoroutines(State<T> state)
        {
            if (state.HasExitCoroutine)
            {
                StartCoroutine(state.GetExitCoroutine(_entity));
            }

            if (_activeCoroutines.TryGetValue(state, out Coroutine routine))
            {
                if (routine is not null)
                {
                    StopCoroutine(routine);
                }
                _activeCoroutines.Remove(state);
            }
        }

        protected async void StartStateAsyncTasks(State<T> state)
        {
            if (state.HasEnterAsync)
                await state.GetEnterAsync(_entity);

            if (state.HasUpdateAsync)
                StartCoroutine(RunUpdateAsync(state));
        }

        protected IEnumerator RunUpdateAsync(State<T> state)
        {
            while (true)
            {
                var task = state.GetUpdateAsync(_entity);
                while (!task.IsCompleted)
                {
                    yield return null;
                }
                yield return null;
            }
        }

        #region Public API

        /// <summary>
        /// Add a state to the FiniteStateMachine
        /// </summary>
        public MonoFiniteStateMachine<T> AddState(IState<T> state)
        {
            _FiniteStateMachine.AddState(state);
            return this;
        }

        /// <summary>
        /// Create and add a new state
        /// </summary>
        public State<T> CreateState(string stateName)
            => _FiniteStateMachine.CreateState(stateName);

        /// <summary>
        /// Set the initial state of the FiniteStateMachine
        /// </summary>
        public MonoFiniteStateMachine<T> SetInitialState<TState>() where TState : IState<T>
        {
            _FiniteStateMachine.SetInitialState<TState>();

            // Start coroutines for the initial state
            if (_FiniteStateMachine.GetCurrentState() is State<T> state)
            {
                StartStateCoroutines(state);
                StartStateAsyncTasks(state);
            }

            return this;
        }

        /// <summary>
        /// Set the initial state by name
        /// </summary>
        public MonoFiniteStateMachine<T> SetInitialState(string stateName)
        {
            _FiniteStateMachine.SetInitialState(stateName);

            // Start coroutines for the initial state
            if (_FiniteStateMachine.GetCurrentState() is State<T> state)
            {
                StartStateCoroutines(state);
                StartStateAsyncTasks(state);
            }

            return this;
        }

        /// <summary>
        /// Change state by type
        /// </summary>
        public void ChangeState<TState>() where TState : IState<T>
            => _FiniteStateMachine.ChangeState<TState>();

        /// <summary>
        /// Change state by name
        /// </summary>
        public void ChangeState(string stateName)
            => _FiniteStateMachine.ChangeState(stateName);

        /// <summary>
        /// Get the current state
        /// </summary>
        public IState<T> GetCurrentState()
            => _FiniteStateMachine.GetCurrentState();

        #endregion

        private void Update()
            => _FiniteStateMachine?.Update();

        private void FixedUpdate()
            => _FiniteStateMachine?.FixedUpdate();

        private void OnDestroy()
        {
            foreach (var coroutine in _activeCoroutines.Values)
            {
                if (coroutine is not null)
                    StopCoroutine(coroutine);
            }

            _activeCoroutines.Clear();
            
            // Clean up event subscriptions
            if (_FiniteStateMachine is not null)
                _FiniteStateMachine.OnStateChanged -= HandleStateChanged;
        }
    }
    #endregion*/
}