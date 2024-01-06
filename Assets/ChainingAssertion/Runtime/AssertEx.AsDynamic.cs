using System;
using System.Dynamic;
using System.Linq;
using System.Reflection;

// NOTE: dynamic type does not work with IL2CPP. It is recommended not to use it in Runtime tests.

namespace ChainingAssertion
{
    public static partial class AssertEx
    {
        public static dynamic AsDynamic<T>(this T target)
        {
            return new DynamicAccessor<T>(target);
        }
    }

    internal interface IDynamicAccessor
    {
        object Target { get; }
    }

    internal sealed class DynamicAccessor<T> : DynamicObject, IDynamicAccessor
    {
        readonly T target;
        public object Target => target;

        public DynamicAccessor(T target)
        {
            this.target = target;
        }

        public override bool TrySetIndex(SetIndexBinder binder, object[] indexes, object value)
        {
            try
            {
                typeof(T).InvokeMember("Item", ReflectionHelper.TransparentFlags | BindingFlags.SetProperty, null, target, indexes.Concat(new[] { value }).ToArray());
                return true;
            }
            catch (MissingMethodException)
            { 
                throw new ArgumentException($"indexer not found : Type '{typeof(T).Name}'");
            };
        }

        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            try
            {
                result = typeof(T).InvokeMember("Item", ReflectionHelper.TransparentFlags | BindingFlags.GetProperty, null, target, indexes);
                return true;
            }
            catch (MissingMethodException)
            { 
                throw new ArgumentException($"indexer not found : Type '{typeof(T).Name}'");
            };
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            for (int i = 0; i < args.Length; i++)
            {
                var arg = args[i];
                if (arg is IDynamicAccessor accessor) args[i] = accessor.Target;
            }

            var typeArgs = ReflectionHelper.GetGenericMethodArguments(binder);
            result = ReflectionHelper.InvokeMatchedMethod(typeof(T), target, binder.Name, args, typeArgs);

            return true;
        }

        public override bool TryConvert(ConvertBinder binder, out object result)
        {
            result = binder.Type.IsInstanceOfType(target) ? target : Convert.ChangeType(target, binder.Type);
            return true;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            ReflectionHelper.SetValue(target, binder.Name, value);
            return true;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = ReflectionHelper.GetValue(target, binder.Name);
            return true;
        }

        public override string ToString()
        {
            return target.ToString();
        }
    }
}