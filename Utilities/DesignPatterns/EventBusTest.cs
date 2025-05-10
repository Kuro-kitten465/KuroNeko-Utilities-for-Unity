using System;
using Kuro.UnityUtils.DesignPatterns;
using UnityEngine;

namespace Kuro.UnityUtils.Tests
{
    public class EventBusTest : MonoBehaviour
    {
        public enum TestEnum
        {
            Test1,
            Test2,
            Test3,
            Test4
        }

        void Awake()
        {
            EventBus.Clear();
            EventBus.Subscribe(TestEnum.Test1, OnTestEvent1);
            EventBus.Subscribe(TestEnum.Test1, OnTestEvent2);
            EventBus.Subscribe(TestEnum.Test3, OnTestEvent3);
            EventBus.Subscribe(TestEnum.Test4, OnTestEvent4);
            EventBus.Subscribe(TestEnum.Test4, OnVoidTestEvent4);
        }

        void OnApplicationQuit()
        {
            EventBus.Unsubscribe(TestEnum.Test1, OnTestEvent1);
            EventBus.Unsubscribe(TestEnum.Test2, OnTestEvent2);
            EventBus.Unsubscribe(TestEnum.Test3, OnTestEvent3);
            EventBus.Unsubscribe(TestEnum.Test4, OnTestEvent4);
            EventBus.Unsubscribe(TestEnum.Test4, OnVoidTestEvent4);
        }

        public void OnTestEvent1(object e, object a)
        {
            Debug.Log(a);
        }

        public void OnTestEvent2(object e, object a)
        {
            Debug.Log(a);
        }

        public void OnTestEvent3(object e, object a)
        {
            Debug.Log(a);
        }

        public int OnTestEvent4(object e, object a)
        {
            return (int)a;
        }

        public void OnVoidTestEvent4(object e, object a)
        {
            Debug.Log("OnTestEvent4: " + a);
        }
    }
}
