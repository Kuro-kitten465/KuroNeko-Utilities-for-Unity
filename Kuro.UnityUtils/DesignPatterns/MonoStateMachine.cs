using UnityEngine;

namespace Kuro.UnityUtils.DesignPatterns
{
    public abstract class MonoStateMachine<TOwner> : MonoBehaviour where TOwner : MonoStateMachine<TOwner>
    {
        public StateMachine<TOwner> StateMachine { get; private set; }

        protected virtual void Awake()
        {
            StateMachine = new(this as TOwner);
        }

        protected virtual void Update()
        {
            StateMachine.Update(Time.deltaTime);
        }

        protected virtual void FixedUpdate()
        {
            StateMachine.FixedUpdate(Time.fixedDeltaTime);
        }

        // ------------------------------
        // Lifecycle Controls
        // ------------------------------
        public void StartStateMachine() => StateMachine.Start();
        public void StopStateMachine() => StateMachine.Stop();
        public void PauseStateMachine() => StateMachine.Pause();
        public void ResumeStateMachine() => StateMachine.Resume();
    }

    public abstract class MonoEnumeratorStateMachine<TOwner> : MonoBehaviour where TOwner : MonoEnumeratorStateMachine<TOwner>
    {
        public EnumeratorStateMachine<TOwner> StateMachine { get; private set; }

        private Coroutine _currentCoroutine;

        protected virtual void Awake()
        {
            StateMachine = new(this as TOwner);
        }

        // ------------------------------
        // Lifecycle Controls
        // ------------------------------
        public void StartStateMachine()
        {
            StateMachine.Start();
            _currentCoroutine = StartCoroutine(StateMachine.Run());
        }

        public void StopStateMachine()
        {
            StopCoroutine(_currentCoroutine);
            StateMachine.Stop();
        }
    }
}
