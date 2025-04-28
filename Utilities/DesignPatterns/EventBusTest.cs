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
            EventBus.ClearEvents();
            EventBus.Subscribe(TestEnum.Test1, OnTestEvent1);
            EventBus.Subscribe(TestEnum.Test2, OnTestEvent2);
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

        public void OnTestEvent1(object sender, params object[] args)
        {
            foreach (var arg in args)
            {
                Debug.Log(arg);
            }
        }

        public void OnTestEvent2(object sender, params object[] args)
        {
            foreach (var arg in args)
            {
                Debug.Log(arg);
            }
        }

        public void OnTestEvent3(object sender, params object[] args)
        {
            foreach (var arg in args)
            {
                Debug.Log(arg);
            }
        }

        public int OnTestEvent4(object sender, params object[] args)
        {
            return (int)args[0];
        }

        public void OnVoidTestEvent4(object sender, params object[] args)
        {
            Debug.Log("OnTestEvent4: " + args[0]);
        }
    }
}
