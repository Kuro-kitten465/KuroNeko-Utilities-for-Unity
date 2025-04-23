using System;
using System.Collections.Generic;
using static UnityEngine.Debug;

namespace Kuro.UnityUtils.DesignPatterns
{/*
    /// <summary>
    /// Factory for creating states dynamically
    /// </summary>
    public class StateFactory<T>
    {
        private readonly Dictionary<string, Func<State<T>>> _stateFactories = new();
        
        /// <summary>
        /// Register a state factory function
        /// </summary>
        public StateFactory<T> RegisterState(string stateName, Func<State<T>> factoryFunc)
        {
            _stateFactories[stateName] = factoryFunc;
            return this;
        }
        
        /// <summary>
        /// Create a state by name
        /// </summary>
        public State<T> CreateState(string stateName)
        {
            if (_stateFactories.TryGetValue(stateName, out Func<State<T>> factory))
                return factory();
            
            LogError($"No factory registered for state: {stateName}");
            return null;
        }
    }*/
}