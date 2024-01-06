using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;

namespace ChainingAssertion
{
    internal static class ReflectionHelper
    {
        public static readonly BindingFlags TransparentFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        public static object GetValue<T>(T target, string name)
        {
            var fieldInfo = typeof(T).GetField(name, TransparentFlags);
            if (fieldInfo != null) return fieldInfo.GetValue(target);

            var propertyInfo = typeof(T).GetProperty(name, TransparentFlags);
            if (propertyInfo != null) return propertyInfo.GetValue(target, null);

            throw new ArgumentException($"\"{name}\" not found : Type '{typeof(T).Name}'");
        }

        public static void SetValue<T>(T target, string name, object value)
        {
            var fieldInfo = typeof(T).GetField(name, TransparentFlags);
            if (fieldInfo != null)
            {
                fieldInfo.SetValue(target, value);
                return;
            }

            var propertyInfo = typeof(T).GetProperty(name, TransparentFlags);
            if (propertyInfo != null)
            {
                propertyInfo.SetValue(target, value);
                return;
            }

            throw new ArgumentException($"\"{name}\" not found : Type '{typeof(T).Name}'");
        }

        public static bool IsParametersCompatible(MethodInfo method, object[] args)
        {
            var methodParams = method.GetParameters();
            if (methodParams.Length != args.Length) return false;

            for (int i = 0; i < methodParams.Length; ++i)
            {
                var parameterType = methodParams[i].ParameterType;
                ref var arg = ref args[i];

                if (arg == null && parameterType.IsValueType) return false;

                if (!parameterType.IsInstanceOfType(arg))
                {
                    if (parameterType.IsByRef)
                    {
                        var typePassedByRef = parameterType.GetElementType();
                        if (typePassedByRef.IsValueType && arg == null) return false;

                        if (arg != null)
                        {
                            var argType = arg.GetType();
                            var argByRefType = argType.MakeByRefType();
                            if (parameterType != argByRefType)
                            {
                                try
                                {
                                    arg = Convert.ChangeType(arg, typePassedByRef);
                                }
                                catch (InvalidCastException)
                                {
                                    return false;
                                }
                            }
                        }
                    }
                    else if (arg == null)
                    {
                        continue;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public static object InvokeMatchedMethod(Type type, object target, string name, object[] args, Type[] typeArgs)
        {
            MethodInfo method = null;
            var currentType = type;

            while (method == null && currentType != null)
            {
                var methods = currentType.GetMethods(TransparentFlags);

                MethodInfo candidate;
                for (int i = 0; i < methods.Length; ++i)
                {
                    candidate = methods[i];

                    if (candidate.Name == name)
                    {
                        if (typeArgs.Length > 0 && candidate.ContainsGenericParameters)
                        {
                            var candidateTypeArgs = candidate.GetGenericArguments();
                            if (candidateTypeArgs.Length == typeArgs.Length)
                            {
                                candidate = candidate.MakeGenericMethod(typeArgs);
                            }
                        }

                        if (IsParametersCompatible(candidate, args))
                        {
                            method = candidate;
                            break;
                        }
                    }
                }

                if (method == null)
                {
                    currentType = currentType.BaseType;
                }
            }

            if (method == null)
            {
                throw new MissingMethodException($"Method with name '{name}' not found on type '{type.FullName}'.");
            }

            return method.Invoke(target, args);
        }

        public static Type[] GetGenericMethodArguments(InvokeMemberBinder binder)
        {
            var csharpInvokeMemberBinderType = binder
                .GetType()
                .GetInterface("Microsoft.CSharp.RuntimeBinder.ICSharpInvokeOrInvokeMemberBinder");

            var typeArgsList = (IList<Type>)csharpInvokeMemberBinderType.GetProperty("TypeArguments").GetValue(binder, null);

            Type[] typeArgs;
            if (typeArgsList.Count == 0)
            {
                typeArgs = Array.Empty<Type>();
            }
            else
            {
                typeArgs = typeArgsList.ToArray();
            }

            return typeArgs;
        }
    }
}