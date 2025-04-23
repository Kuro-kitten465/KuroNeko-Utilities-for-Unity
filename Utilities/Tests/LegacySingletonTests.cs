using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Kuro.UnityUtils.DesignPatterns;
using System;
using NUnit.Framework.Internal;

namespace Kuro.UnityUtils.Tests
{
    public class TestSingleton : LegacySingleton<TestSingleton>
    {
        public int Value { get; private set; } = 69;

        protected override void OnInstanceCreated()
        {
            Value = 69;
        }

        protected override void OnInstanceDestroy()
        {
            Value = 0;
        }
    }

    public class LegacySingletonTests
    {
        [SetUp]
        public void Setup()
        {
            if (TestSingleton.IsInitialized)
            {
                TestSingleton.DestroyInstance();
            }
        }

        [Test]
        public void Instance_Is_Created()
        {
            Assert.Throws<InvalidOperationException>(() => _ = TestSingleton.Instance);
            var instance = TestSingleton.CreateInstance();
            Assert.IsNotNull(instance);
            Assert.IsTrue(TestSingleton.IsInitialized);
            Assert.AreEqual(69, instance.Value);
        }

        [Test]
        public void Instance_Is_Singleton()
        {
            TestSingleton.CreateInstance();
            var instance1 = TestSingleton.Instance;
            var instance2 = TestSingleton.Instance;
            Assert.AreSame(instance1, instance2);
        }

        [Test]
        public void Instance_Is_Destroyed()
        {
            TestSingleton.DestroyInstance();
            Assert.IsFalse(TestSingleton.IsInitialized);
            Assert.Throws<InvalidOperationException>(() => _ = TestSingleton.Instance.Value);
        }

        [Test]
        public void Instance_Cannot_Be_Initialized_Twice()
        {
            var instance1 = TestSingleton.CreateInstance();
            //Assert.Throws<Exception>(() => new TestSingleton());
            Assert.AreSame(instance1, TestSingleton.Instance);
        }

        [Test]
        public void Instance_Can_Be_Destroyed_And_Recreated()
        {
            TestSingleton.DestroyInstance();
            Assert.IsFalse(TestSingleton.IsInitialized);

            var newInstance = TestSingleton.CreateInstance();
            Assert.IsNotNull(newInstance);
            Assert.IsTrue(TestSingleton.IsInitialized);
            Assert.AreEqual(69, newInstance.Value);
        }
    }
}
