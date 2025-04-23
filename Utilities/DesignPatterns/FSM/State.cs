using System;
using System.Collections;
using System.Threading.Tasks;

namespace Kuro.UnityUtils.DesignPatterns
{
    /*/// <summary>
    /// Generic State interface that defines the structure of a state
    /// </summary>
    /// <typeparam name="T">Type of the entity/context the state operates on</typeparam>
    public interface IState<T>
    {
        void Enter(T entity);
        void Update(T entity);
        void FixedUpdate(T entity);
        void Exit(T entity);
    }

    /// <summary>
    /// Delegate types for different types of state actions
    /// </summary>
    public delegate void StateAction<T>(T entity);
    public delegate IEnumerator StateCoroutine<T>(T entity);
    public delegate Task StateAsync<T>(T entity);

    /// <summary>
    /// State transition event arguments
    /// </summary>
    public class StateChangeEventArgs<T> : EventArgs
    {
        public IState<T> PreviousState { get; private set; }
        public IState<T> NewState { get; private set; }
        public T Entity { get; private set; }

        public StateChangeEventArgs(IState<T> previousState, IState<T> newState, T entity)
        {
            PreviousState = previousState;
            NewState = newState;
            Entity = entity;
        }
    }

    /// <summary>
    /// A flexible implementation of a state that supports normal, async, and coroutine functions
    /// </summary>
    public class State<T> : IState<T>
    {
        private readonly string _name;

        // Normal action delegates
        private StateAction<T> _onEnter;
        private StateAction<T> _onUpdate;
        private StateAction<T> _onFixedUpdate;
        private StateAction<T> _onExit;

        // Coroutine delegates
        private StateCoroutine<T> _onEnterCoroutine;
        private StateCoroutine<T> _onUpdateCoroutine;
        private StateCoroutine<T> _onExitCoroutine;

        // Async delegates
        private StateAsync<T> _onEnterAsync;
        private StateAsync<T> _onUpdateAsync;
        private StateAsync<T> _onExitAsync;

        // Event handlers
        public event EventHandler<StateChangeEventArgs<T>> OnStateEnter;
        public event EventHandler<StateChangeEventArgs<T>> OnStateExit;

        public string Name => _name;

        public State(string name = null)
            => _name = name ?? GetType().Name;

        #region Normal Action Setters
        public State<T> SetEnterAction(StateAction<T> action)
        {
            _onEnter = action;
            return this;
        }

        public State<T> SetUpdateAction(StateAction<T> action)
        {
            _onUpdate = action;
            return this;
        }

        public State<T> SetFixedUpdateAction(StateAction<T> action)
        {
            _onFixedUpdate = action;
            return this;
        }

        public State<T> SetExitAction(StateAction<T> action)
        {
            _onExit = action;
            return this;
        }
        #endregion

        #region Coroutine Setters
        public State<T> SetEnterCoroutine(StateCoroutine<T> coroutine)
        {
            _onEnterCoroutine = coroutine;
            return this;
        }

        public State<T> SetUpdateCoroutine(StateCoroutine<T> coroutine)
        {
            _onUpdateCoroutine = coroutine;
            return this;
        }

        public State<T> SetExitCoroutine(StateCoroutine<T> coroutine)
        {
            _onExitCoroutine = coroutine;
            return this;
        }
        #endregion

        #region Async Setters
        public State<T> SetEnterAsync(StateAsync<T> asyncAction)
        {
            _onEnterAsync = asyncAction;
            return this;
        }

        public State<T> SetUpdateAsync(StateAsync<T> asyncAction)
        {
            _onUpdateAsync = asyncAction;
            return this;
        }

        public State<T> SetExitAsync(StateAsync<T> asyncAction)
        {
            _onExitAsync = asyncAction;
            return this;
        }
        #endregion

        #region Interface Implementation
        public void Enter(T entity)
        {
            _onEnter?.Invoke(entity);
            OnStateEnter?.Invoke(this, new StateChangeEventArgs<T>(null, this, entity));
        }

        public void Update(T entity)
            => _onUpdate?.Invoke(entity);

        public void FixedUpdate(T entity)
            => _onFixedUpdate?.Invoke(entity);

        public void Exit(T entity)
        {
            _onExit?.Invoke(entity);
            OnStateExit?.Invoke(this, new StateChangeEventArgs<T>(this, null, entity));
        }
        #endregion

        #region Coroutine Getters
        public IEnumerator GetEnterCoroutine(T entity)
            => _onEnterCoroutine?.Invoke(entity);

        public IEnumerator GetUpdateCoroutine(T entity)
            => _onUpdateCoroutine?.Invoke(entity);

        public IEnumerator GetExitCoroutine(T entity)
            => _onExitCoroutine?.Invoke(entity);
        #endregion

        #region Async Getters
        public Task GetEnterAsync(T entity)
            => _onEnterAsync?.Invoke(entity);

        public Task GetUpdateAsync(T entity)
            => _onUpdateAsync?.Invoke(entity);

        public Task GetExitAsync(T entity)
            => _onExitAsync?.Invoke(entity);
        #endregion

        public bool HasEnterCoroutine => _onEnterCoroutine != null;
        public bool HasUpdateCoroutine => _onUpdateCoroutine != null;
        public bool HasExitCoroutine => _onExitCoroutine != null;

        public bool HasEnterAsync => _onEnterAsync != null;
        public bool HasUpdateAsync => _onUpdateAsync != null;
        public bool HasExitAsync => _onExitAsync != null;
    }*/
}
