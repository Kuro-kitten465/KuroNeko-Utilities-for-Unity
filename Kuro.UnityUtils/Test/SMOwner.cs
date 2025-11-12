using UnityEngine;
using Kuro.UnityUtils.DesignPatterns;

namespace Kuro.UnityUtils
{
    public class SMOwner : MonoEnumeratorStateMachine<SMOwner>
    {
        [SerializeField] private RedState _redState;

        public Material OBJMat;

        private void Start()
        {
            OBJMat = GetComponent<MeshRenderer>().material;

            StateMachine.AddState(_redState);
            StateMachine.AddState(new GreenState());
            StateMachine.AddState(new YellowState());

            StateMachine.SetInitialState<GreenState>();
        }

        private void OnGUI()
        {
            if (GUILayout.Button("Start"))
            {
                StartStateMachine();
            }

            if (GUILayout.Button("Stop"))
            {
                StopStateMachine();
            }
        }
    }
}
