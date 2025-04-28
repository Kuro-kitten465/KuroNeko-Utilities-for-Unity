using System;
using Kuro.UnityUtils.DesignPatterns;
using UnityEngine;

namespace Kuro.UnityUtils.Tests
{
    public class EventBusClient : MonoBehaviour
    {
        void OnGUI()
        {
            if (GUI.Button(new Rect(10, 10, 100, 30), "Test Event"))
            {
                EventBus.Publish(EventBusTest.TestEnum.Test1, this, "Sorasaki Hina");
                EventBus.Publish(EventBusTest.TestEnum.Test2, this, "Konno Yuuki");
                EventBus.Publish(EventBusTest.TestEnum.Test3, this, "Kuro_kitten");
                Debug.Log(EventBus.Publish<int>(EventBusTest.TestEnum.Test4, this, 500));
                EventBus.Publish(EventBusTest.TestEnum.Test4, this, "Nani");

                Debug.Log(EventBus.GetEventType(EventBusTest.TestEnum.Test1));
                Debug.Log(EventBus.GetEventType(EventBusTest.TestEnum.Test2));
                Debug.Log(EventBus.GetEventType(EventBusTest.TestEnum.Test3));
                Debug.Log(EventBus.GetEventType(EventBusTest.TestEnum.Test4));
            }
        }
    }
}
