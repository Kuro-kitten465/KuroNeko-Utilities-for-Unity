using System.Collections.Generic;

namespace KuroNeko.Utilities.DesignPattern
{
    public interface IState
    {
        void StateEnter(params dynamic[] args);
        void StateUpdate(params dynamic[] args);
        void StateExit(params dynamic[] args);
    }
    
    public class StateMachine<TKey, TState> where TState : IState
    {
        private TState _currentState;
        private TKey _currentStateKey;
        private readonly Dictionary<TKey, TState> _states = new();

        public TKey CurrentState => _currentStateKey;

        public void AddState(TKey key, TState state)
        {
            if (!_states.ContainsKey(key))
                _states.Add(key, state);
        }

        public void ChangeState(TKey key, params dynamic[] args)
        {
            if (_states.TryGetValue(key, out var state))
            {
                _currentState?.StateExit(args);
                _currentState = state;
                _currentState.StateEnter(args);
                _currentStateKey = key;
            }
        }

        public void Update(params dynamic[] args) =>
            _currentState?.StateUpdate(args);
    }
}