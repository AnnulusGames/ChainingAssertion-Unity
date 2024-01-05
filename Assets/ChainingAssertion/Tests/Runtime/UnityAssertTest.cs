using NUnit.Framework;
using UnityEngine;

namespace ChainingAssertion.Tests.Runtime
{
    public class UnityAssertTest
    {
        [Test]
        public void Test_Position()
        {
            var obj = new GameObject();
            obj.transform.position = Vector3.one;
            obj.transform.position.IsApproximately(Vector3.one);
            Object.Destroy(obj);
        }

        [Test]
        public void Test_IsActive()
        {
            var obj = new GameObject();
            obj.SetActive(false);
            obj.activeSelf.IsFalse();
            Object.Destroy(obj);
        }
    }
}