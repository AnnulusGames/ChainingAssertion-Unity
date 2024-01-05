using NUnit.Framework;
using UnityEngine;

namespace ChainingAssertion.Tests.Editor
{
    public sealed class IsTest
    {
        [Test, Repeat(100)]
        public void Test_Is_Float()
        {
            var actual = Random.value;
            var expected = actual;
            var notExpected = -1f;

            actual.Is(expected);
            actual.IsNot(notExpected);
        }

        [Test, Repeat(100)]
        public void Test_Is_Int()
        {
            var actual = (int)(Random.value * 100f);
            var expected = actual;
            var notExpected = -1;

            actual.Is(expected);
            actual.IsNot(notExpected);
        }

        [Test, Repeat(100)]
        public void Test_Is_Tuple()
        {
            var actual = (Random.value, Random.value);
            var expected = actual;
            var notExpected = (-1f, -1f);

            actual.Is(expected);
            actual.IsNot(notExpected);
        }
    }
}