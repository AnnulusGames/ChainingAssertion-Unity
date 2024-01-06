# ChainingAssertion for Unity

[![license](https://img.shields.io/badge/LICENSE-MIT-green.svg)](LICENSE)

[日本語版READMEはこちら](README_JA.md)

ChainingAssertion for Unity is a method chaining based assertion library for Unity Test Framework.

In addition to `Is()` and `AsDynamic()` provided in the original [ChainingAssertion](https://github.com/neuecc/ChainingAssertion), `IsApproximately()` that performs comparisons that take floating point and `AsPrivateObject()` that supports accessing private members using reflection for testing in IL2CPP (AOT environment) are provided.

## Setup

### Requirements

* Unity 2019.2 or later
* Test Framework 1.0.0 or later

### Installation

1. Open the Package Manager from Window > Package Manager.
2. Click on the "+" button > Add package from git URL.
3. Enter the following URL:

```
https://github.com/AnnulusGames/ChainingAssertion-Unity.git?path=Assets/ChainingAssertion
```

Alternatively, open Packages/manifest.json and add the following to the dependencies block:

```json
{
    "dependencies": {
        "com.annulusgames.chaining-assertion": "https://github.com/AnnulusGames/ChainingAssertion-Unity.git?path=Assets/ChainingAssertion"
    }
}
```

## Is/IsNot

You can intuitively perform assertions using the `Is()`/`IsNot()` extension methods.

```csharp
using System;
using System.Linq;
using NUnit.Framework;
using ChainingAssertion;

public class Example
{
    [Test]
    public void Test_Is()
    {
        Math.Pow(5, 2).Is(25); // Assert.Equals
        Math.Abs(-1).IsNot(-1); // Assert.NotEquals
    }
    
    [Test]
    public void Test_Is_Lambda()
    {
        // Assert.IsTrue(x => ...);
        "ChainingAssertion".Is(s => s.StartsWith("Chaining") && s.EndsWith("Assertion"));
    }

    [Test]
    public void Test_Is_Collection()
    {
        Enumerable.Range(0, 5).Is(0, 1, 2, 3, 4); // CollectionAssert.AreEqual
        Enumerable.Range(0, 5).IsNot(0, 1, 2, 3, 4, 5); // CollectionAssert.AreNotEqual
    }
}
```

## Collection Assertions

Combining LINQ to Objects with Chaining Assertion allows expressing assertions similar to CollectionAssert.

```csharp
var array = new[] { 1, 3, 7, 8 };

array.Contains(8).IsTrue(); // CollectionAssert.Contains
array.Any().IsTrue(); // CollectionAssert.IsNotEmpty
new int[] { }.Any().IsFalse(); // CollectionAssert.IsEmpty
array.OrderBy(x => x).Is(array); // CollectionAssert.IsOrdered
```

## IsApproximately

For floating-point types like `float`, `Vector3`, `Quaternion`, etc., you can perform comparisons considering errors using `IsApproximately()`.

```csharp
transform.position.IsApproximately(Vector3.one);
transform.localEulerAngles.IsApproximately(new Vector3(0f, 0f, 90f), 0.001f);
```

## Other Assertions

```csharp
using System;
using NUnit.Framework;
using ChainingAssertion;

public class Example
{
    [Test]
    public void Test_IsNull()
    {
        object target = null;
        target.IsNull(); // Assert.IsNull
        new object().IsNotNull();  // Assert.IsNull
    }

    [Test]
    public void Test_IsSameReferenceAs()
    {
        var tuple = Tuple.Create("foo");
        tuple.IsSameReferenceAs(tuple); // Assert.AreSame
        tuple.IsNotSameReferenceAs(Tuple.Create("foo")); // Assert.AreNotSame
    }

    [Test]
    public void Test_IsInstanceOf()
    {
        "foo".IsInstanceOf<string>(); // Assert.IsInstanceOfType
        999.IsNotInstanceOf<double>(); // Assert.IsNotInstanceOfType
    }
}
```

## AsDynamic

Using `AsDynamic()` converts the target to a dynamic type, enabling dynamic access to private members.

> [!CAUTION]
> `AsDynamic()` does not work in an IL2CPP environment. Therefore, it's recommended to use `AsPrivateObject()` instead for PlayMode tests.

```csharp
public class TestClass
{
    public TestClass(string str)
    {
        _privateField = str;
    }

    private string _privateField;
    private string PrivateProperty
    {
        get => _privateField;
        set => _privateField = value;
    }
    private string PrivateMethod()
    {
        return _privateField;
    }
}

var actual = new TestClass("foo");
Assert.AreEqual(actual.AsDynamic()._privateField, "foo");
Assert.AreEqual(actual.AsDynamic().PrivateProperty, "foo");
Assert.AreEqual(actual.AsDynamic().PrivateMethod(), "foo");

actual.AsDynamic().PrivateProperty = "bar";
Assert.AreEqual(actual.AsDynamic()._privateField, "bar");
```

## AsPrivateObject

Using `AsPrivateObject()` converts the target to a `PrivateObject`, enabling access to private members using reflection. Use this instead of `AsDynamic()` when executing tests in an IL2CPP environment.

```csharp
var actual = new TestClass("foo");
Assert.AreEqual(actual.AsPrivateObject().GetField("_privateField"), "foo");
Assert.AreEqual(actual.AsPrivateObject().GetProperty("PrivateProperty"), "foo");
Assert.AreEqual(actual.AsPrivateObject().Invoke("PrivateMethod"), "foo");

actual.AsPrivateObject().SetFieldOrProperty("PrivateProperty", "bar");
Assert.AreEqual(actual.AsPrivateObject().GetFieldOrProperty("_privateField"), "bar");
```

## License

[MIT License](LICENSE)