using Kuro.UnityUtils.DesignPatterns;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Kuro.UnityUtils
{
    public class RedState : IState<FSMTests>
    {
        private Color _color = Color.red;

        public void OnEnter(FSMTests owner)
        {
            Debug.Log("Entering Red State");
            owner.SpriteRenderer.color = _color;
        }
        
        public void OnUpdate(FSMTests owner)
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                Debug.Log("Space key pressed in Red State");
            }
        }

        public void OnFixedUpdate(FSMTests owner)
        {
            
        }

        public void OnExit(FSMTests owner)
        {
            Debug.Log("Exiting Red State");
        }
    }
}
