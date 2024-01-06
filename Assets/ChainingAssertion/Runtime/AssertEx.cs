using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools.Utils;
using IS = NUnit.Framework.Is;

namespace ChainingAssertion
{
    public static partial class AssertEx
    {
        public static void Is<T>(this T actual, T expected, string message = "")
        {
            Assert.AreEqual(expected, actual, message);
        }

        public static void Is<T>(this IEnumerable<T> actual, IEnumerable<T> expected, string message = "")
        {
            CollectionAssert.AreEqual(expected.ToArray(), actual.ToArray(), message);
        }

        public static void Is<T>(this IEnumerable<T> actual, params T[] expected)
        {
            Is(actual, expected.AsEnumerable());
        }

        public static void Is<T>(this T value, Func<T, bool> predicate, string message = "")
        {
            var condition = predicate.Invoke(value);
            Assert.IsTrue(condition, message);
        }

        public static void IsNot<T>(this T actual, T expected, string message = "")
        {
            Assert.AreNotEqual(expected, actual, message);
        }

        public static void IsNot<T>(this IEnumerable<T> actual, IEnumerable<T> expected, string message = "")
        {
            CollectionAssert.AreNotEqual(expected.ToArray(), actual.ToArray(), message);
        }

        public static void IsNot<T>(this IEnumerable<T> actual, params T[] expected)
        {
            IsNot(actual, expected.AsEnumerable());
        }

        public static void IsNot<T>(this T value, Func<T, bool> predicate, string message = "")
        {
            var condition = predicate.Invoke(value);
            Assert.IsFalse(condition, message);
        }

        public static void IsTrue(this bool value, string message = "")
        {
            value.Is(true, message);
        }

        public static void IsFalse(this bool value, string message = "")
        {
            value.Is(false, message);
        }

        public static TExpected IsInstanceOf<TExpected>(this object value, string message = "")
        {
            Assert.IsInstanceOf<TExpected>(value, message);
            return (TExpected)value;
        }

        public static void IsNotInstanceOf<TWrong>(this object value, string message = "")
        {
            Assert.IsNotInstanceOf<TWrong>(value, message);
        }

        public static void IsSameAs<T>(this T actual, T expected, string message = "")
        {
            Assert.AreSame(expected, actual, message);
        }

        public static void IsNotSameAs<T>(this T actual, T notExpected, string message = "")
        {
            Assert.AreNotSame(notExpected, actual, message);
        }

        public static void IsAssignableFrom<T>(this object actual, string message = "")
        {
            Assert.IsAssignableFrom<T>(actual, message);
        }

        public static void IsNotAssignableFrom<T>(this object actual, string message = "")
        {
            Assert.IsNotAssignableFrom<T>(actual, message);
        }

        public static void IsApproximately(this float actual, float expected, float error = 0.00001f, string message = "")
        {
            Assert.That(actual, IS.EqualTo(expected).Using(new FloatEqualityComparer(error)), message);
        }

        public static void IsApproximately(this Vector2 actual, Vector2 expected, float error = 0.00001f, string message = "")
        {
            Assert.That(actual, IS.EqualTo(expected).Using(new Vector2EqualityComparer(error)), message);
        }

        public static void IsApproximately(this Vector3 actual, Vector3 expected, float error = 0.00001f, string message = "")
        {
            Assert.That(actual, IS.EqualTo(expected).Using(new Vector3EqualityComparer(error)), message);
        }

        public static void IsApproximately(this Vector4 actual, Vector4 expected, float error = 0.00001f, string message = "")
        {
            Assert.That(actual, IS.EqualTo(expected).Using(new Vector4EqualityComparer(error)), message);
        }

        public static void IsApproximately(this Quaternion actual, Quaternion expected, float error = 0.00001f, string message = "")
        {
            Assert.That(actual, IS.EqualTo(expected).Using(new QuaternionEqualityComparer(error)), message);
        }

        public static void IsApproximately(this Color actual, Color expected, float error = 0.00001f, string message = "")
        {
            Assert.That(actual, IS.EqualTo(expected).Using(new ColorEqualityComparer(error)), message);
        }

        public static void IsNotApproximately(this float actual, float expected, float error = 0.00001f, string message = "")
        {
            Assert.That(actual, IS.Not.EqualTo(expected).Using(new FloatEqualityComparer(error)), message);
        }

        public static void IsNotApproximately(this Vector2 actual, Vector2 expected, float error = 0.00001f, string message = "")
        {
            Assert.That(actual, IS.Not.EqualTo(expected).Using(new Vector2EqualityComparer(error)), message);
        }

        public static void IsNotApproximately(this Vector3 actual, Vector3 expected, float error = 0.00001f, string message = "")
        {
            Assert.That(actual, IS.Not.EqualTo(expected).Using(new Vector3EqualityComparer(error)), message);
        }

        public static void IsNotApproximately(this Vector4 actual, Vector4 expected, float error = 0.00001f, string message = "")
        {
            Assert.That(actual, IS.Not.EqualTo(expected).Using(new Vector4EqualityComparer(error)), message);
        }

        public static void IsNotApproximately(this Color actual, Color expected, float error = 0.00001f, string message = "")
        {
            Assert.That(actual, IS.Not.EqualTo(expected).Using(new ColorEqualityComparer(error)), message);
        }

        public static void IsNotApproximately(this Quaternion actual, Quaternion expected, float error = 0.00001f, string message = "")
        {
            Assert.That(actual, IS.Not.EqualTo(expected).Using(new QuaternionEqualityComparer(error)), message);
        }
    }
}