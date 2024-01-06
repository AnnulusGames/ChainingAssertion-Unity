using System;
using System.Linq;
using NUnit.Framework;
using Random = UnityEngine.Random;

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

        [Test]
        public void Test_Is_Enumerable()
        {
            Enumerable.Range(0, 5).Is(0, 1, 2, 3, 4);
            Enumerable.Range(0, 5).IsNot(0, 1, 2, 3, 4, 5);
        }

        [Test]
        public void Test_IsInstanceOf_String()
        {
            "foo".IsInstanceOf<string>();
            "foo".IsNotInstanceOf<int>();
        }

        [Test]
        public void Test_IsSameReferenceAs()
        {
            var a = Tuple.Create("foo");
            var b = a;
            var c = Tuple.Create("foo");
            a.IsSameReferenceAs(b);
            a.IsNotSameReferenceAs(c);
        }

        [Test]
        public void Test_IsNull()
        {
            object obj1 = null;
            obj1.IsNull();
            object obj2 = new();
            obj2.IsNotNull();
        }

        [Test]
        public void Test_IsTrue_IsFalse()
        {
            true.IsTrue();
            false.IsFalse();
        }
    }
}