using System;
using System.Reflection;

namespace ChainingAssertion
{
    public static partial class AssertEx
    {
        public static PrivateObject AsPrivateObject<T>(this T target)
        {
            return new PrivateObject(target);
        }
    }

    public sealed class PrivateObject
    {
        public PrivateObject(object target)
        {
            this.target = target;
            targetType = target.GetType();
        }

        readonly object target;
        readonly Type targetType;

        public object Target => target;
        public Type RealType => targetType;

        public object GetField(string name) => GetField(name, ReflectionHelper.TransparentFlags);
        public object GetField(string name, BindingFlags bindingFlags)
        {
            var field = targetType.GetField(name, bindingFlags) ?? throw new MissingFieldException();
            return field.GetValue(target);
        }

        public void SetField(string name, object value) => SetField(name, ReflectionHelper.TransparentFlags, value);
        public void SetField(string name, BindingFlags bindingFlags, object value)
        {
            var field = targetType.GetField(name, bindingFlags) ?? throw new MissingFieldException();
            field.SetValue(target, value);
        }

        public object GetProperty(string name) => GetProperty(name, ReflectionHelper.TransparentFlags);
        public object GetProperty(string name, BindingFlags bindingFlags)
        {
            var property = targetType.GetProperty(name, bindingFlags) ?? throw new MissingMemberException();
            return property.GetValue(target);
        }

        public void SetProperty(string name, object value) => SetProperty(name, ReflectionHelper.TransparentFlags, value);
        public void SetProperty(string name, BindingFlags bindingFlags, object value)
        {
            var property = targetType.GetProperty(name, bindingFlags) ?? throw new MissingMemberException();
            property.SetValue(target, value);
        }

        public object GetFieldOrProperty(string name) => GetFieldOrProperty(name, ReflectionHelper.TransparentFlags);
        public object GetFieldOrProperty(string name, BindingFlags bindingFlags)
        {
            var field = targetType.GetField(name, bindingFlags);
            if (field != null) return field.GetValue(target);

            var property = targetType.GetProperty(name, bindingFlags);
            if (property != null) return property.GetValue(target);

            throw new MissingMemberException();
        }

        public void SetFieldOrProperty(string name, object value) => SetFieldOrProperty(name, ReflectionHelper.TransparentFlags, value);
        public void SetFieldOrProperty(string name, BindingFlags bindingFlags, object value)
        {
            var field = targetType.GetField(name, bindingFlags);
            if (field != null)
            {
                field.SetValue(target, value);
                return;
            }

            var property = targetType.GetProperty(name, bindingFlags);
            if (property != null)
            {
                property.SetValue(target, value);
                return;
            }

            throw new MissingMemberException();
        }

        public object Invoke(string name) => Invoke(name, ReflectionHelper.TransparentFlags, null);
        public object Invoke(string name, object[] args) => Invoke(name, ReflectionHelper.TransparentFlags, args);
        public object Invoke(string name, BindingFlags bindingFlags, object[] args)
        {
            var method = targetType.GetMethod(name, bindingFlags) ?? throw new MissingMethodException();
            return method.Invoke(target, args);
        }

        public object Invoke(string name, Type[] parameterTypes, object[] args) => Invoke(name, ReflectionHelper.TransparentFlags, parameterTypes, args);
        public object Invoke(string name, BindingFlags bindingFlags, Type[] parameterTypes, object[] args)
        {
            var method = targetType.GetMethod(name, bindingFlags, Type.DefaultBinder, parameterTypes, null) ?? throw new MissingMethodException();
            return method.Invoke(target, args);
        }

        public override bool Equals(object obj)
        {
            return target.Equals(obj);
        }

        public override int GetHashCode()
        {
            return target.GetHashCode();
        }
    }
}