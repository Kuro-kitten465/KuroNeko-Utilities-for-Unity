using Kuro.UnityUtils.DesignPatterns;
using UnityEngine;

namespace Kuro.UnityUtils
{
    public struct YellowState : IState<FSMTests>
    {
        private Color _color;

        public YellowState(Color color)
        {
            _color = color;
        }

        public readonly void OnEnter(FSMTests owner)
        {
            Debug.Log("Entering Yellow State");
            owner.SpriteRenderer.color = _color;
        }
        
        public readonly void OnUpdate(FSMTests owner)
        {
            
        }

        public readonly void OnFixedUpdate(FSMTests owner)
        {
            
        }

        public readonly void OnExit(FSMTests owner)
        {
            Debug.Log("Exiting Yellow State");
        }
    }
}
