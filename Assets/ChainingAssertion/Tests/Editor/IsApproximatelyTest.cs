using NUnit.Framework;
using UnityEngine;

namespace ChainingAssertion.Tests.Editor
{
    public class IsApproximatelyTest
    {
        const float FloatError = 0.000001f;
        
        [Test, Repeat(100)]
        public void Test_IsApproximately_Float()
        {
            var actual = Random.value;
            var expected = actual + FloatError;
            var notExpected = -1f;

            actual.IsNot(expected);
            actual.IsApproximately(expected);
            actual.IsNotApproximately(notExpected);
        }

        [Test, Repeat(100)]
        public void Test_IsApproximately_Vector2()
        {
            var actual = new Vector2(Random.value, Random.value);
            var expected = actual + Vector2.one * FloatError;
            var notExpected = -Vector2.one;

            actual.IsNot(expected);
            actual.IsApproximately(expected);
            actual.IsNotApproximately(notExpected);
        }

        [Test, Repeat(100)]
        public void Test_IsApproximately_Vector3()
        {
            var actual = new Vector3(Random.value, Random.value, Random.value);
            var expected = actual + Vector3.one * FloatError;
            var notExpected = -Vector3.one;

            actual.IsNot(expected);
            actual.IsApproximately(expected);
            actual.IsNotApproximately(notExpected);
        }

        [Test, Repeat(100)]
        public void Test_IsApproximately_Vector4()
        {
            var actual = new Vector4(Random.value, Random.value, Random.value, Random.value);
            var expected = actual + Vector4.one * FloatError;
            var notExpected = -Vector4.one;

            actual.IsNot(expected);
            actual.IsApproximately(expected);
            actual.IsNotApproximately(notExpected);
        }

        [Test, Repeat(100)]
        public void Test_IsApproximately_Color()
        {
            var actual = new Color(Random.value, Random.value, Random.value, 0.1f);
            var expected = new Color(actual.r + FloatError, actual.g, actual.b, actual.a);
            var notExpected = Color.clear;

            actual.IsNot(expected);
            actual.IsApproximately(expected);
            actual.IsNotApproximately(notExpected);
        }

        [Test, Repeat(100)]
        public void Test_IsApproximately_Quaternion()
        {
            var actual = Quaternion.Euler(Random.value, Random.value, Random.value);
            var expected = actual * Quaternion.Euler(FloatError, FloatError, FloatError);
            var notExpected = Quaternion.Euler(-1f, -1f, -1f);

            actual.IsNot(expected);
            actual.IsApproximately(expected);
            actual.IsNotApproximately(notExpected);
        }
    }
}