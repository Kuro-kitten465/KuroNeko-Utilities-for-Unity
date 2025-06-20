using System;
using System.Collections.Generic;
using UnityEngine;

namespace Kuro.UnityUtils.DesignPatterns
{
    public abstract class ScriptableState : ScriptableObject
    {
        public virtual void OnEnter(ScriptableStateRunner runner) { }
        public virtual void OnDynamicStay(ScriptableStateRunner runner) { }
        public virtual void OnFixedStay(ScriptableStateRunner runner) { }
        public virtual void OnExit(ScriptableStateRunner runner) { }
    }

    [CreateAssetMenu(menuName = "FSM/FSM Definition")]
    public class ScriptableStateDefinition : ScriptableObject
    {
        public ScriptableState InitialState;
        public List<ScriptableState> AllStates;

        [Serializable]
        public class StateTransition
        {
            public ScriptableState from;
            public ScriptableState to;
        }

        public List<StateTransition> Transitions;

        public bool CanTransition(ScriptableState from, ScriptableState to)
        {
            foreach (var t in Transitions)
            {
                if (t.from.Equals(from) && t.to.Equals(to)) return true;
            }
            return false;
        }
    }
}
