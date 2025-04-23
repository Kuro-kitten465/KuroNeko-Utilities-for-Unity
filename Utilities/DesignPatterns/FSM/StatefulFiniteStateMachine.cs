using System.Collections.Generic;
using UnityEngine;

namespace Kuro.UnityUtils.DesignPatterns
{
    /*/// <summary>
    /// FiniteStateMachine extension that keeps track of state history
    /// </summary>
    public class StatefulFiniteStateMachine<T> : FiniteStateMachine<T>
    {
        private readonly Stack<IState<T>> _previousStates = new();
        private readonly int _maxHistorySize;
        
        public StatefulFiniteStateMachine(T entity, int maxHistorySize = 10) : base(entity)
        {
            _maxHistorySize = maxHistorySize;
            OnStateChanged += TrackStateHistory;
        }
        
        private void TrackStateHistory(object sender, StateChangeEventArgs<T> e)
        {
            if (e.PreviousState != null)
            {
                _previousStates.Push(e.PreviousState);
                
                // Keep history size bounded
                if (_previousStates.Count > _maxHistorySize)
                {
                    var tempStack = new Stack<IState<T>>();
                    
                    // Copy all but the oldest state
                    for (int i = 0; i < _maxHistorySize; i++)
                    {
                        if (_previousStates.Count > 0)
                        {
                            tempStack.Push(_previousStates.Pop());
                        }
                    }
                    
                    // Skip the oldest
                    if (_previousStates.Count > 0)
                    {
                        _previousStates.Pop();
                    }
                    
                    // Put everything back
                    while (tempStack.Count > 0)
                    {
                        _previousStates.Push(tempStack.Pop());
                    }
                }
            }
        }
        
        /// <summary>
        /// Return to the previous state
        /// </summary>
        public bool GoToPreviousState()
        {
            if (_previousStates.Count > 0)
            {
                IState<T> prevState = _previousStates.Pop();
                TransitionToState(prevState);
                return true;
            }
            return false;
        }
        
        /// <summary>
        /// Get the state history
        /// </summary>
        public IEnumerable<IState<T>> GetStateHistory()
            => _previousStates;
    }
    
    /// <summary>
    /// MonoBehaviour wrapper for StatefulFiniteStateMachine
    /// </summary>
    public class MonoStatefulFiniteStateMachine<T> : MonoBehaviour where T : class
    {
        private StatefulFiniteStateMachine<T> _FiniteStateMachine;
        private T _entity;
        
        public void Initialize(T entity, int historySize = 10)
        {
            _entity = entity;
            _FiniteStateMachine = new StatefulFiniteStateMachine<T>(entity, historySize);
        }
        
        /// <summary>
        /// Return to the previous state
        /// </summary>
        public bool GoToPreviousState()
            => _FiniteStateMachine.GoToPreviousState();
        
        /// <summary>
        /// Get the state history
        /// </summary>
        public IEnumerable<IState<T>> GetStateHistory()
            => _FiniteStateMachine.GetStateHistory();
    }*/
}