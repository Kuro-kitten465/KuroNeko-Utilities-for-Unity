using System.Collections.Generic;

namespace Kuro.UnityUtils.DesignPatterns
{
    /*/// <summary>
    /// Hierarchical State that can have parent states
    /// </summary>
    public class HierarchicalState<T> : State<T>
    {
        private HierarchicalState<T> _parentState;
        private readonly List<HierarchicalState<T>> _subStates = new();
        
        public HierarchicalState<T> ParentState => _parentState;
        public IReadOnlyList<HierarchicalState<T>> SubStates => _subStates;
        
        public HierarchicalState(string name = null) : base(name) { }
        
        /// <summary>
        /// Add a substate to this state
        /// </summary>
        public HierarchicalState<T> AddSubState(HierarchicalState<T> subState)
        {
            subState._parentState = this;
            _subStates.Add(subState);
            return this;
        }
        
        /// <summary>
        /// Create and add a new substate
        /// </summary>
        public HierarchicalState<T> CreateSubState(string name)
        {
            var subState = new HierarchicalState<T>(name);
            AddSubState(subState);
            return subState;
        }
        
        /// <summary>
        /// Override to propagate updates to parent states
        /// </summary>
        public new void Update(T entity)
        {
            base.Update(entity);
            _parentState?.Update(entity);
        }
        
        /// <summary>
        /// Override to propagate fixed updates to parent states
        /// </summary>
        public new void FixedUpdate(T entity)
        {
            base.FixedUpdate(entity);
            _parentState?.FixedUpdate(entity);
        }
    }
    
    /// <summary>
    /// Hierarchical Finite State Machine that supports state hierarchy
    /// </summary>
    public class HFiniteStateMachine<T> : FiniteStateMachine<T>
    {
        public HFiniteStateMachine(T entity) : base(entity) { }
        
        /// <summary>
        /// Create a new hierarchical state
        /// </summary>
        public new HierarchicalState<T> CreateState(string stateName)
        {
            var state = new HierarchicalState<T>(stateName);
            AddState(state);
            return state;
        }
    }
    
    /// <summary>
    /// MonoBehaviour wrapper for HFiniteStateMachine
    /// </summary>
    public class MonoHFiniteStateMachine<T> : MonoFiniteStateMachine<T> where T : class
    {
        private HFiniteStateMachine<T> _hFiniteStateMachine;
        
        public new void Initialize(T entity)
        {
            base.Initialize(entity);
            _hFiniteStateMachine = new HFiniteStateMachine<T>(entity);
        }
        
        /// <summary>
        /// Create a new hierarchical state
        /// </summary>
        public new HierarchicalState<T> CreateState(string stateName)
            => _hFiniteStateMachine.CreateState(stateName);
    }*/
}