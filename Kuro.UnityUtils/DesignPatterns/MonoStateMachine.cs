using UnityEngine;

namespace Kuro.UnityUtils.DesignPatterns
{
    public abstract class MonoStateMachine<T> : MonoBehaviour where T : MonoStateMachine<T>
    {
        protected StateMachine<T> _fsm;

        protected virtual void Awake()
        {
            _fsm = new StateMachine<T>((T)this);
            SetupStates(_fsm);
        }

        protected virtual void Update() => _fsm?.DynamicStay();
        protected virtual void FixedUpdate() => _fsm?.FixedStay();

        protected abstract void SetupStates(StateMachine<T> fsm);
    }
}
