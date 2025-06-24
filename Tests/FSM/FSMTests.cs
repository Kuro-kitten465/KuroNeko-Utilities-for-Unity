using UnityEngine;
using Kuro.UnityUtils.DesignPatterns;

namespace Kuro.UnityUtils
{
    public class FSMTests : MonoStateMachine<FSMTests>
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;

        [Space(10)]
        [SerializeField] private GreenState _greenState;
        [SerializeField] private IdleState _idleState;

        private RedState _redState;
        private YellowState _yellowState;

        public SpriteRenderer SpriteRenderer
        {
            get => _spriteRenderer;
            set => _spriteRenderer = value;
        }

        protected override void SetupStates(StateMachine<FSMTests> fsm)
        {
            _redState = new RedState();
            _yellowState = new YellowState(Color.yellow);

            fsm.AddState(_greenState);
            fsm.AddState(_redState);
            fsm.AddState(_yellowState);
            fsm.AddState(_idleState);

            fsm.ChangeState(_idleState);
        }

        private void OnGUI()
        {
            if (GUILayout.Button("Change to Green State"))
            {
                _fsm.ChangeState(_greenState);
            }

            if (GUILayout.Button("Change to Idle State"))
            {
                _fsm.ChangeState(_idleState);
            }

            if (GUILayout.Button("Change to Red State"))
            {
                _fsm.ChangeState(_redState);
            }

            if (GUILayout.Button("Change to Yellow State"))
            {
                _fsm.ChangeState(_yellowState);
            }
        }
    }
}
