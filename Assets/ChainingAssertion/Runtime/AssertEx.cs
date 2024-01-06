using System;
using System.Collections;
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
            if (typeof(T) != typeof(string) && typeof(IEnumerable).IsAssignableFrom(typeof(T)))
            {
                ((IEnumerable)actual).Cast<object>().Is(((IEnumerable)expected).Cast<object>(), message);
                return;
            }

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

        public static void Is<T>(this IEnumerable<T> actual, IEnumerable<T> expected, IEqualityComparer<T> comparer, string message = "")
        {
            Is(actual, expected, comparer.Equals, message);
        }

        public static void Is<T>(this IEnumerable<T> actual, IEnumerable<T> expected, Func<T, T, bool> equalityComparison, string message = "")
        {
            CollectionAssert.AreEqual(expected.ToArray(), actual.ToArray(), new ComparisonComparer<T>(equalityComparison), message);
        }

        public static void IsNot<T>(this T actual, T notExpected, string message = "")
        {
            if (typeof(T) != typeof(string) && typeof(IEnumerable).IsAssignableFrom(typeof(T)))
            {
                ((IEnumerable)actual).Cast<object>().IsNot(((IEnumerable)notExpected).Cast<object>(), message);
                return;
            }

            Assert.AreNotEqual(notExpected, actual, message);
        }

        public static void IsNot<T>(this IEnumerable<T> actual, IEnumerable<T> notExpected, string message = "")
        {
            CollectionAssert.AreNotEqual(notExpected.ToArray(), actual.ToArray(), message);
        }

        public static void IsNot<T>(this IEnumerable<T> actual, params T[] notExpected)
        {
            IsNot(actual, notExpected.AsEnumerable());
        }
        public static void IsNot<T>(this IEnumerable<T> actual, IEnumerable<T> notExpected, IEqualityComparer<T> comparer, string message = "")
        {
            IsNot(actual, notExpected, comparer.Equals, message);
        }

        public static void IsNot<T>(this IEnumerable<T> actual, IEnumerable<T> notExpected, Func<T, T, bool> equalityComparison, string message = "")
        {
            CollectionAssert.AreNotEqual(notExpected.ToArray(), actual.ToArray(), new ComparisonComparer<T>(equalityComparison), message);
        }

        public static void IsTrue(this bool value, string message = "")
        {
            value.Is(true, message);
        }

        public static void IsFalse(this bool value, string message = "")
        {
            value.Is(false, message);
        }

        public static void IsNull<T>(this T value)
        {
            Assert.IsNull(value);
        }

        public static void IsNotNull<T>(this T value)
        {
            Assert.IsNotNull(value);
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

        public static void IsSameReferenceAs<T>(this T actual, T expected, string message = "")
        {
            Assert.AreSame(expected, actual, message);
        }

        public static void IsNotSameReferenceAs<T>(this T actual, T notExpected, string message = "")
        {
            Assert.AreNotSame(notExpected, actual, message);
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

        public static void IsNotApproximately(this float actual, float notExpected, float error = 0.00001f, string message = "")
        {
            Assert.That(actual, IS.Not.EqualTo(notExpected).Using(new FloatEqualityComparer(error)), message);
        }

        public static void IsNotApproximately(this Vector2 actual, Vector2 notExpected, float error = 0.00001f, string message = "")
        {
            Assert.That(actual, IS.Not.EqualTo(notExpected).Using(new Vector2EqualityComparer(error)), message);
        }

        public static void IsNotApproximately(this Vector3 actual, Vector3 notExpected, float error = 0.00001f, string message = "")
        {
            Assert.That(actual, IS.Not.EqualTo(notExpected).Using(new Vector3EqualityComparer(error)), message);
        }

        public static void IsNotApproximately(this Vector4 actual, Vector4 notExpected, float error = 0.00001f, string message = "")
        {
            Assert.That(actual, IS.Not.EqualTo(notExpected).Using(new Vector4EqualityComparer(error)), message);
        }

        public static void IsNotApproximately(this Color actual, Color notExpected, float error = 0.00001f, string message = "")
        {
            Assert.That(actual, IS.Not.EqualTo(notExpected).Using(new ColorEqualityComparer(error)), message);
        }

        public static void IsNotApproximately(this Quaternion actual, Quaternion notExpected, float error = 0.00001f, string message = "")
        {
            Assert.That(actual, IS.Not.EqualTo(notExpected).Using(new QuaternionEqualityComparer(error)), message);
        }
    }

    internal sealed class ComparisonComparer<T> : IComparer
    {
        readonly Func<T, T, bool> comparison;

        public ComparisonComparer(Func<T, T, bool> comparison)
        {
            this.comparison = comparison;
        }

        public int Compare(object x, object y)
        {
            return (comparison != null)
                ? comparison((T)x, (T)y) ? 0 : -1
                : Equals(x, y) ? 0 : -1;
        }
    }
}