using Kuro.UnityUtils.DesignPatterns;
using UnityEngine;

namespace Kuro.UnityUtils
{
    public class EventClientB : MonoBehaviour
    {
        private void OnGUI()
        {
            if (GUI.Button(new Rect(10, 10, 150, 30), "Damage by 10"))
            {
                EventBus<OnHealthChanged>.Raise(new OnHealthChanged(-10));
            }
        }
    }
}
