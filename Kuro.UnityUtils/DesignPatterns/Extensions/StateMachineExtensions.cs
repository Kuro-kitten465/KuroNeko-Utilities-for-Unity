using System;
using System.Collections.Generic;

namespace Kuro.UnityUtils.DesignPatterns
{
    public interface INestedState<T>
    {
        void SetSubFSM(StateMachine<T> subFSM);
        StateMachine<T> GetSubFSM();
    }

    // Helper base class (optional)
    public abstract class NestedState<T> : State<T>, INestedState<T>
    {
        protected StateMachine<T> _subFSM;

        public void SetSubFSM(StateMachine<T> subFSM) => _subFSM = subFSM;
        public StateMachine<T> GetSubFSM() => _subFSM;

        public override void OnEnter(T owner) => _subFSM?.ChangeStateToFirst();
        public override void OnExit(T owner) { }
        public override void OnDynamicStay(T owner) => _subFSM?.DynamicStay();
        public override void OnFixedStay(T owner) => _subFSM?.FixedStay();
    }

    public static class StateMachineExtensions
    {
        #region Parent State
        public static void ChangeStateToFirst<T>(this StateMachine<T> fsm)
        {
            if (fsm.GetFirstParentState() != null)
                fsm.ChangeState<State<T>>();
        }

        public static string GetFirstParentState<T>(this StateMachine<T> fsm)
        {
            foreach (var kvp in fsm.States)
            {
                if (kvp.Value is NestedState<T> nestedState)
                    return kvp.Key;
            }
                
            return null;
        }
        #endregion
    }

    public class TransitionStateMachine<T> : StateMachine<T>
    {
        private readonly Dictionary<string, List<Type>> _transitions = new();

        public TransitionStateMachine(T owner) : base(owner) { }

        public void AddTransition<TFrom, TTo>()
            where TFrom : State<T>
            where TTo : State<T>
        {
            var from = typeof(TFrom);
            var to = typeof(TTo);

            if (!_transitions.ContainsKey(from.Name))
                _transitions[from.Name] = new List<Type>();

            _transitions[from.Name].Add(to);
        }

        public override void ChangeState<TState>()
        {
            var from = CurrentState?.GetType();
            var to = typeof(TState);

            if (from != null && _transitions.TryGetValue(from.Name, out var validTransitions))
            {
                if (!validTransitions.Contains(to))
                    throw new Exception($"Invalid transition: {from} -> {to}");
            }

            base.ChangeState<TState>();
        }
    }
}
