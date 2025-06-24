using Kuro.UnityUtils.DesignPatterns;
using UnityEngine;

namespace Kuro.UnityUtils
{
    [CreateAssetMenu(fileName = "IdleState", menuName = "Kuro/UnityUtils/FSM/IdleState")]
    public class IdleState : ScriptableObject, IState<FSMTests>
    {
        [SerializeField] private Color _idleColor = Color.white;

        public void OnEnter(FSMTests owner)
        {
            Debug.Log("Entering Idle State");
            owner.SpriteRenderer.color = _idleColor;
        }

        public void OnUpdate(FSMTests owner)
        {
            // Idle state does not perform any dynamic actions
        }

        public void OnFixedUpdate(FSMTests owner)
        {
            // Idle state does not perform any fixed actions
        }

        public void OnExit(FSMTests owner)
        {
            Debug.Log("Exiting Idle State");
        }
    }
}
