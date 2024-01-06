using System;
using NUnit.Framework;

#pragma warning disable IDE0051 

namespace ChainingAssertion.Tests.Editor
{
    public class AsPrivateObjectTest
    {
        [Test]
        public void Test_Private_Field()
        {
            var instance = new TestClass(2);
            instance.AsPrivateObject().GetField("_foo").Is(2);
            instance.AsPrivateObject().SetField("_foo", 10);
            instance.Foo.Is(10);
        }

        [Test]
        public void Test_Private_Property()
        {
            var instance = new TestClass(2);
            instance.AsPrivateObject().GetProperty("FooProperty").Is(2);
            instance.AsPrivateObject().SetProperty("FooProperty", 10);
            instance.Foo.Is(10);
        }

        [Test]
        public void Test_Private_FieldOrProperty()
        {
            var instance = new TestClass(2);
            instance.AsPrivateObject().GetFieldOrProperty("_foo").Is(2);
            instance.AsPrivateObject().GetFieldOrProperty("FooProperty").Is(2);

            instance.AsPrivateObject().SetFieldOrProperty("_foo", 10);
            instance.Foo.Is(10);

            instance.AsPrivateObject().SetFieldOrProperty("FooProperty", 20);
            instance.Foo.Is(20);
        }

        [Test]
        public void Test_Private_Method()
        {
            var instance = new TestClass(2);
            instance.Foo.Is(2);
            instance.AsPrivateObject().Invoke("SetFoo", new object[] { 10 });
            instance.Foo.Is(10);
        }

        sealed class TestClass
        {
            public TestClass(int foo)
            {
                _foo = foo;
            }

            private int _foo;
            public int Foo => _foo;

            private int FooProperty { get => _foo; set => _foo = value; }
            private void SetFoo(int value)
            {
                _foo = value;
            }
        }
    }
}

#pragma warning restore IDE0051