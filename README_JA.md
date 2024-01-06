# ChainingAssertion for Unity

[![license](https://img.shields.io/badge/LICENSE-MIT-green.svg)](LICENSE)

[English README is here](README.md)

ChainingAssertion for UnityはUnity Test Framework向けに実装されたメソッドチェーンベースのAssertionライブラリです。

オリジナルの[ChainingAssertion](https://github.com/neuecc/ChainingAssertion)で提供される`Is()`や`AsDynamic()`に加え、`float`や`Vector3`、`Quaternion`の浮動小数点誤差を考慮した比較を行う`IsApproximately()`や、IL2CPP(AOT環境)でのテスト用にリフレクションを用いたprivateメンバーのアクセスをサポートする`AsPrivateObject()`などの機能が提供されています。

## セットアップ

### 要件

* Unity 2019.2 以上
* Test Framework 1.0.0 以上

### インストール

1. Window > Package ManagerからPackage Managerを開く
2. 「+」ボタン > Add package from git URL
3. 以下のURLを入力する

```
https://github.com/AnnulusGames/ChainingAssertion-Unity.git?path=Assets/ChainingAssertion
```

あるいはPackages/manifest.jsonを開き、dependenciesブロックに以下を追記

```json
{
    "dependencies": {
        "com.annulusgames.chaining-assertion": "https://github.com/AnnulusGames/ChainingAssertion-Unity.git?path=Assets/ChainingAssertion"
    }
}
```

## Is / IsNot

`Is()`/`IsNot()`拡張メソッドを用いて直感的にアサーションを行うことができます。

```cs
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

LINQ to ObjectsとChaining Assertionを組み合わせることでCollectionAssertと同様のアサーションを表現できます。

```cs
var array = new[] { 1, 3, 7, 8 };

array.Contains(8).IsTrue(); // CollectionAssert.Contains
array.Any().IsTrue(); // CollectionAssert.IsNotEmpty
new int[] { }.Any().IsFalse(); // CollectionAssert.IsEmpty
array.OrderBy(x => x).Is(array); // CollectionAssert.IsOrdered
```

## IsApproximately

`float`や`Vector3`、`Quaternion`などの浮動小数点型に対し、誤差を考慮した比較を`IsApproximately()`で行うことができます。

```cs
transform.position.IsApproximately(Vector3.one);
transform.localEulerAngles.IsApproximately(new Vector3(0f, 0f, 90f), 0.001f);
```

## Other Assertions

```cs
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

`AsDynamic()`を使用することで対象をdynamic型に変換し、privateメンバーに動的にアクセスすることが可能になります。

> [!WARNING]
> `AsDynamic()`はIL2CPP環境では動作しません。そのためPlayModeテストでは代わりに`AsPrivateObject()`を使用することが推奨されます。

```cs
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

`AsPrivateObject()`を使用することで対象を`PrivateObjcct`に変換し、対象のprivateメンバーにリフレクションを用いてアクセスすることが可能になります。IL2CPP環境でテストを実行する際は`AsDynamic()`の代わりにこちらを利用してください。

```cs
var actual = new TestClass("foo");
Assert.AreEqual(actual.AsPrivateObject().GetField("_privateField"), "foo");
Assert.AreEqual(actual.AsPrivateObject().GetProperty("PrivateProperty"), "foo");
Assert.AreEqual(actual.AsPrivateObject().Invoke("PrivateMethod"), "foo");

actual.AsPrivateObject().SetFieldOrProperty("PrivateProperty", "bar");
Assert.AreEqual(actual.AsPrivateObject().GetFieldOrProperty("_privateField"), "bar");
```

## ライセンス

[MIT License](LICENSE)