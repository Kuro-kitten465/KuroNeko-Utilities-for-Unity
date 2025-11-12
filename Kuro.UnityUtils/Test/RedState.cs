using UnityEngine;
using Kuro.UnityUtils.DesignPatterns;
using System.Collections;

namespace Kuro.UnityUtils
{
    public class RedState : MonoBehaviour, IEnumeratorState<SMOwner>
    {
        [SerializeField] private Color _color = Color.red;

        public void OnEnter(SMOwner owner)
        {
            owner.OBJMat.color = _color;

            Debug.Log($"{this} Entering...");
        }

        public void OnExit(SMOwner owner)
        {
            
        }

        public IEnumerator Run(SMOwner owner)
        {
            yield return new WaitForSeconds(3f);
            owner.StateMachine.ChangeState<YellowState>();
        }
    }

    public class YellowState : IEnumeratorState<SMOwner>
    {
        private Color _color = Color.yellow;

        public void OnEnter(SMOwner owner)
        {
            owner.OBJMat.color = _color;

            Debug.Log($"{this} Entering...");
        }

        public void OnExit(SMOwner owner)
        {
            
        }

        public IEnumerator Run(SMOwner owner)
        {
            yield return new WaitForSeconds(3f);

            if (owner.StateMachine.PreviousState is GreenState)
            {
                owner.StateMachine.ChangeState<RedState>();
            }
            else
            {
                owner.StateMachine.ChangeState<GreenState>();
            }
        }
    }

    public struct GreenState : IEnumeratorState<SMOwner>
    {
        private Color _color;

        public void OnEnter(SMOwner owner)
        {
            _color = Color.green;
            owner.OBJMat.color = _color;

            Debug.Log($"{this} Entering...");
        }

        public void OnExit(SMOwner owner)
        {

        }

        public IEnumerator Run(SMOwner owner)
        {
            yield return new WaitForSeconds(3f);
            owner.StateMachine.ChangeState<YellowState>();
        }
    }
}
