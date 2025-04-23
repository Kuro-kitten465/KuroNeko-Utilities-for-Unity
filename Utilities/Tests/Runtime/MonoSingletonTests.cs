using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Kuro.UnityUtils.DesignPatterns;

namespace Kuro.UnityUtils.Tests.Runtime
{
    public class MonoSingletonTests
    {
        public class TestMonoSingleton : MonoSingletonPersistent<TestMonoSingleton>
        {
            public bool WasInitialized { get; private set; }

            protected override void OnInstanceCreated()
            {
                WasInitialized = true;
            }
        }

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            if (TestMonoSingleton.IsInitialized)
            {
                MonoSingleton<TestMonoSingleton>.DestroyInstance();
                yield return null;
            }
        }

        [UnityTest]
        public IEnumerator Instance_CreatesSingletonGameObject()
        {
            var instance = TestMonoSingleton.Instance;

            yield return new WaitForSeconds(2f); // Wait for the GameObject to be created

            Assert.IsNotNull(instance);
            Assert.AreEqual(instance, TestMonoSingleton.Instance);
            Assert.IsTrue(TestMonoSingleton.IsInitialized);
            Assert.IsTrue(instance.WasInitialized);
        }

        [UnityTest]
        public IEnumerator MultipleInstances_DestroyExtra()
        {
            var first = TestMonoSingleton.Instance;
            yield return new WaitForSeconds(2f); // Wait for the GameObject to be created

            var secondGameObject = new GameObject("Another");
            var second = secondGameObject.AddComponent<TestMonoSingleton>();
            yield return new WaitForSeconds(2f); // Wait for the GameObject to be created

            // Since Awake destroys the second one, it should no longer exist
            Assert.IsTrue(first != null);
            Assert.IsTrue(TestMonoSingleton.Instance == first);
            Assert.IsTrue(second == null || second.Equals(null)); // Unity overrides == for destroyed objects
        }

        [UnityTest]
        public IEnumerator DestroyInstance_ResetsState()
        {
            var instance = TestMonoSingleton.Instance;
            yield return new WaitForSeconds(2f); // Wait for the GameObject to be created

            MonoSingleton<TestMonoSingleton>.DestroyInstance();
            yield return new WaitForSeconds(2f); // Wait for the GameObject to be created

            Assert.IsFalse(TestMonoSingleton.IsInitialized);
            Assert.That(() => GameObject.Find($"{typeof(TestMonoSingleton).Name} Instance") == null);
        }
    }

}
