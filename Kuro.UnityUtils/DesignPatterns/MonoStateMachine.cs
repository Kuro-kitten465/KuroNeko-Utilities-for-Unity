using UnityEngine;

namespace Kuro.UnityUtils.DesignPatterns
{
    public abstract class MonoStateMachine<TOwner> : MonoBehaviour where TOwner : MonoStateMachine<TOwner>
    {
        protected StateMachine<TOwner> _fsm;

        protected virtual void Awake()
        {
            _fsm = new StateMachine<TOwner>((TOwner)this);
            SetupStates(_fsm);
        }

        protected virtual void Update() => _fsm?.DynamicStay();
        protected virtual void FixedUpdate() => _fsm?.FixedStay();

        protected abstract void SetupStates(StateMachine<TOwner> fsm);
    }
}
