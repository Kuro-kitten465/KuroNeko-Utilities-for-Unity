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
    public abstract class NestedState<T> : IState<T>, INestedState<T>
    {
        protected StateMachine<T> _subFSM;

        public void SetSubFSM(StateMachine<T> subFSM) => _subFSM = subFSM;
        public StateMachine<T> GetSubFSM() => _subFSM;

        public void OnEnter(T owner) => _subFSM?.ChangeStateToFirst();
        public void OnExit(T owner) { }
        public void OnDynamicStay(T owner) => _subFSM?.DynamicStay();
        public void OnFixedStay(T owner) => _subFSM?.FixedStay();
    }

    public static class StateMachineExtensions
    {
        #region Parent State
        public static void ChangeStateToFirst<T>(this StateMachine<T> fsm)
        {
            if (fsm.GetFirstParentState() != null)
                fsm.ChangeState<IState<T>>();
        }

        public static Type GetFirstParentState<T>(this StateMachine<T> fsm)
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

    /* public class TransitionStateMachine<TOwner> : StateMachine<TOwner>
    {
        private readonly Dictionary<Type, List<Type>> _transitions = new();

        public TransitionStateMachine(TOwner owner) : base(owner) { }

        public void AddTransition<TFrom, TTo>()
            where TFrom : IState<TOwner>
            where TTo : IState<TOwner>
        {
            var from = typeof(TFrom);
            var to = typeof(TTo);

            if (!_transitions.ContainsKey(from))
                _transitions[from] = new List<Type>();

            _transitions[from].Add(to);
        }

        public override void ChangeState<TState>()
        {
            var from = CurrentState?.GetType();
            var to = typeof(TState);

            if (from != null && _transitions.TryGetValue(from, out var validTransitions))
            {
                if (!validTransitions.Contains(to))
                    throw new Exception($"Invalid transition: {from} -> {to}");
            }

            base.ChangeState<TState>();
        }
    } */
}
