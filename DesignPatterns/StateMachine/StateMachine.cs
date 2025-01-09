using System.Collections.Generic;

namespace KuroNeko.Utilities.DesignPattern
{
    public class StateMachine<TKey, TValue> where TValue : IState
    {
        private TValue _currentState;
        private TKey _currentStateKey;
        private readonly Dictionary<TKey, TValue> _states = new();

        public TKey CurrentState => _currentStateKey;

        public void AddState(TKey key, TValue state)
        {
            if (!_states.ContainsKey(key))
                _states.Add(key, state);
        }

        public void ChangeState(TKey key, params dynamic[] args)
        {
            if (_states.TryGetValue(key, out var state))
            {
                _currentState?.StateExit();
                _currentState = state;
                _currentState.StateEnter(args);
                _currentStateKey = key;
            }
        }

        public void Update(params dynamic[] args) =>
            _currentState?.StateUpdate(args);
    }
}