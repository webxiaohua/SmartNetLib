using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using System.Diagnostics;

namespace Test
{
    public class OrderInfo
    {
        public int OrderID { get; set; }
    }
    public class EmitTest
    {
        public static void Test()
        {
            Console.WriteLine(System.Runtime.InteropServices.RuntimeEnvironment.GetSystemVersion());
            int count = 1000000;
            OrderInfo testObj = new OrderInfo();
            PropertyInfo propInfo = typeof(OrderInfo).GetProperty("OrderID");
            Console.Write("直接访问花费时间：    ");
            Stopwatch watch1 = Stopwatch.StartNew();
            for (int i = 0; i < count; i++)
            {
                testObj.OrderID = 123;
            }
            watch1.Stop();
            Console.WriteLine(watch1.Elapsed.ToString());
            SetValueDelegate setter2 = DynamicMethodFactory.CreatePropertySetter(propInfo);
            Console.Write("EmitSet花费时间：     ");
            Stopwatch watch2 = Stopwatch.StartNew();
            for (int i = 0; i < count; i++)
            {
                setter2(testObj, 123);
            }
            watch2.Stop();
            Console.WriteLine(watch2.Elapsed.ToString());

            Console.Write("纯反射花费时间：     ");
            Stopwatch watch3 = Stopwatch.StartNew();
            for (int i = 0; i < count; i++)
            {
                propInfo.SetValue(testObj, 123, null);
            }
            watch3.Stop();
            Console.WriteLine(watch3.Elapsed.ToString());
            Console.WriteLine("----------------------------------");
            Console.WriteLine("{0} / {1} = {2}",
            watch3.Elapsed.ToString(),
            watch1.Elapsed.ToString(),
    watch3.Elapsed.TotalMilliseconds / watch1.Elapsed.TotalMilliseconds);

            Console.WriteLine("{0} / {1} = {2}",
    watch3.Elapsed.ToString(),
    watch2.Elapsed.ToString(),
    watch3.Elapsed.TotalMilliseconds / watch2.Elapsed.TotalMilliseconds);

            Console.WriteLine("{0} / {1} = {2}",
    watch2.Elapsed.ToString(),
    watch1.Elapsed.ToString(),
    watch2.Elapsed.TotalMilliseconds / watch1.Elapsed.TotalMilliseconds);

            Console.Write("泛型委托花费时间：        ");
            SetterWrapper<OrderInfo, int> setter3 = new SetterWrapper<OrderInfo, int>(propInfo);
            Stopwatch watch4 = Stopwatch.StartNew();
            for (int i = 0; i < count; i++)
            {
                setter3.SetValue(testObj, 123);
            }
            watch4.Stop();
            Console.WriteLine(watch4.Elapsed.ToString());

            Console.Write("通用接口花费时间：        ");
            ISetValue setter4 = GetterSetterFactory.CreatePropertySetterWrapper(propInfo);
            Stopwatch watch5 = Stopwatch.StartNew();
            watch5.Start();
            for (int i = 0; i < count; i++)
            {
                setter4.Set(testObj, 123);
            }
            watch5.Stop();
            Console.WriteLine(watch5.Elapsed.ToString());
        }
    }

    public delegate void SetValueDelegate(object target, object arg);

    public static class DynamicMethodFactory
    {
        /// <summary>
        /// 创建属性设置器
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public static SetValueDelegate CreatePropertySetter(PropertyInfo property)
        {
            if (property == null)
            {
                throw new ArgumentException("property");
            }
            if (!property.CanWrite)
            {
                return null;
            }
            MethodInfo setMethod = property.GetSetMethod(true);
            DynamicMethod dm = new DynamicMethod("PropertySetter", null, new Type[] { typeof(object), typeof(object) }, property.DeclaringType, true);
            ILGenerator il = dm.GetILGenerator();
            if (!setMethod.IsStatic)
            {
                il.Emit(OpCodes.Ldarg_0);
            }
            il.Emit(OpCodes.Ldarg_1);
            EmitCastToReference(il, property.PropertyType);
            if (!setMethod.IsStatic && !property.DeclaringType.IsValueType)
            {
                il.EmitCall(OpCodes.Callvirt, setMethod, null);
            }
            else
            {
                il.EmitCall(OpCodes.Call, setMethod, null);
            }
            il.Emit(OpCodes.Ret);
            return (SetValueDelegate)dm.CreateDelegate(typeof(SetValueDelegate));
        }

        private static void EmitCastToReference(ILGenerator il, Type type)
        {
            if (type.IsValueType)
                il.Emit(OpCodes.Unbox_Any, type);
            else
                il.Emit(OpCodes.Castclass, type);
        }
    }

    public interface ISetValue
    {
        void Set(object target, object val);
    }

    public class GetterSetterFactory
    {
        public static ISetValue CreatePropertySetterWrapper(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
                throw new ArgumentNullException("propertyInfo");
            if (propertyInfo.CanWrite == false)
                throw new NotSupportedException("属性不支持写操作。");

            MethodInfo mi = propertyInfo.GetSetMethod(true);
            if (mi.GetParameters().Length > 1)
                throw new NotSupportedException("不支持构造索引器属性的委托。");
            Type instanceType = typeof(SetterWrapper<,>).MakeGenericType(propertyInfo.DeclaringType, propertyInfo.PropertyType);
            return (ISetValue)Activator.CreateInstance(instanceType, propertyInfo);
        }
    }

    public class SetterWrapper<T, V> : ISetValue
    {
        private Action<T, V> _setter;
        public SetterWrapper(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
            {
                throw new ArgumentNullException("propertyInfo");
            }
            if (!propertyInfo.CanWrite)
                throw new NotSupportedException("属性不支持写操作");
            MethodInfo m = propertyInfo.GetSetMethod(true);
            _setter = (Action<T, V>)Delegate.CreateDelegate(typeof(Action<T, V>), null, m);
        }
        public void SetValue(T target, V val)
        {
            _setter(target, val);
        }



        void ISetValue.Set(object target, object val)
        {
            _setter((T)target, (V)val);
        }

    }
}
