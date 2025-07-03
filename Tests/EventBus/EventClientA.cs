using Kuro.UnityUtils.DesignPatterns;
using UnityEngine;

namespace Kuro.UnityUtils
{
    public class EventClientA : MonoBehaviour
    {
        private void OnGUI()
        {
            if (GUI.Button(new Rect(10, 50, 150, 30), "Heal by 10"))
            {
                EventBus<OnHealthChanged>.Raise(new OnHealthChanged(10));
            }
        }
    }
}
