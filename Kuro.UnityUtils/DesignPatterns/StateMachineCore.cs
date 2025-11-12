using System;
using System.Collections.Generic;

namespace Kuro.UnityUtils
{
    public interface IState<TOwner>
    {
        void OnEnter(TOwner owner);
        void OnUpdate(TOwner owner, float deltaTime);
        void OnFixedUpdate(TOwner owner, float fixedDeltaTime);
        void OnExit(TOwner owner);
    }
    public class StateMachineCore<TOwner>
    {
        public TOwner Owner { get; private set; }
        public IState<TOwner> CurrentState { get; private set; }

        private Dictionary<Type, IState<TOwner>> _states = new();

        public StateMachineCore(TOwner owner)
        {
            Owner = owner;
            CurrentState = null;
        }

        public StateMachineCore<TOwner> AddState(IState<TOwner> state)
        {
            _states[state.GetType()] = state;
            return this;
        }

        public StateMachineCore<TOwner> RemoveState<TState>() where TState : IState<TOwner>
        {
            _states.Remove(typeof(TState));
            return this;
        }

        public void ChangeState<TState>() where TState : IState<TOwner>
        {
            ChangeState(typeof(TState) as IState<TOwner>);
        }
        
        public void ChangeState(IState<TOwner> newState)
        {
            CurrentState?.OnExit(Owner);
            CurrentState = newState;
            CurrentState.OnEnter(Owner);
        }

        public void Update(float deltaTime)
        {
            CurrentState?.OnUpdate(Owner, deltaTime);
        }

        public void FixedUpdate(float fixedDeltaTime)
        {
            CurrentState?.OnFixedUpdate(Owner, fixedDeltaTime);
        }
    }
}
