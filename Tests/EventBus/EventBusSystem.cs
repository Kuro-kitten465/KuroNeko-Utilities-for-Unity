using UnityEngine;
using Kuro.UnityUtils.DesignPatterns;
using UnityEngine.UI;

namespace Kuro.UnityUtils
{
    public class EventBusSystem : MonoBehaviour
    {
        [SerializeField] private Slider healthSlider;

        private void OnEnable()
        {
            EventBus<OnHealthChanged>.Register(UpdateSlider);

            Debug.Log("EventBusSystem enabled and registered for OnHealthChanged events.");
            Debug.Log(EventBus<OnHealthChanged>.HasListeners);
        }

        private void OnDisable()
        {
            EventBus<OnHealthChanged>.Unregister(UpdateSlider);

            Debug.Log("EventBusSystem disabled and unregistered from OnHealthChanged events.");
            Debug.Log(!EventBus<OnHealthChanged>.HasListeners);
        }

        private void UpdateSlider(OnHealthChanged e)
        {
            var currentHealth = healthSlider.value;
            currentHealth += e.Health;
            healthSlider.value = Mathf.Clamp(currentHealth, healthSlider.minValue, healthSlider.maxValue);
        }
    }

    public class OnHealthChanged : IEvent
    {
        public int Health { get; private set; }

        public OnHealthChanged(int health)
        {
            Health = health;
        }
    }
}
