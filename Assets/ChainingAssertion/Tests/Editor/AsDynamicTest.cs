using NUnit.Framework;

#pragma warning disable IDE0051 

namespace ChainingAssertion.Tests.Editor
{
    public class AsDynamicTest
    {
        [Test]
        public void Test_AsDynamic_PrivateField()
        {
            var instance = new TestClass(1);
            instance.AsDynamic()._foo = 10;
            instance.Foo.Is(10);
        }

        [Test]
        public void Test_AsDynamic_PrivateProperty()
        {
            var instance = new TestClass(1);
            instance.AsDynamic().FooProperty = 10;
            instance.Foo.Is(10);
        }

        [Test]
        public void Test_AsDynamic_PrivateMethod()
        {
            var instance = new TestClass(1);
            instance.AsDynamic().SetFoo(10);
            instance.Foo.Is(10);
        }

        [Test]
        public void Test_AsDynamic_PrivateIndexer()
        {
            var instance = new TestClass(1);
            instance.AsDynamic()[0] = 10;
            instance.array[0].Is(10);
        }

        sealed class TestClass
        {
            public TestClass(int foo)
            {
                _foo = foo;
            }

            public readonly int[] array = new int[16];
            private int _foo;

            public int Foo => _foo;

            private int FooProperty { get => _foo; set => _foo = value; }

            private int this[int index]
            {
                get => array[index];
                set => array[index] = value;
            }

            private void SetFoo(int value)
            {
                _foo = value;
            }
        }
    }
}

#pragma warning restore IDE0051