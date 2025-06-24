using UnityEngine;
using Kuro.UnityUtils.DesignPatterns;

namespace Kuro.UnityUtils
{
    public class GreenState : MonoBehaviour, IState<FSMTests>
    {
        [SerializeField] private Color _color = Color.green;
        [SerializeField] private float _rotationSpeed = 30f;
        [SerializeField] private AudioSource _audioSource;

        public void OnEnter(FSMTests owner)
        {
            owner.SpriteRenderer.color = _color;
            Debug.Log("Entering Green State");
            _audioSource.Play();
        }

        public void OnUpdate(FSMTests owner)
        {
            owner.transform.Rotate(Vector3.forward, _rotationSpeed * Time.deltaTime);
        }

        public void OnFixedUpdate(FSMTests owner)
        {
            
        }

        public void OnExit(FSMTests owner)
        {
            _audioSource.Stop();
            Debug.Log("Exiting Green State");
        }
    }
}
