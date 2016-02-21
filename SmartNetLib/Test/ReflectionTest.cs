using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;

namespace Test
{
    public class ReflectionTest
    {
        public static void Test()
        {
            int count = 1000000;
            OrderInfo order = new OrderInfo() { OrderID = 22 };
            PropertyInfo propertyInfo = order.GetType().GetProperty("OrderID");
            Console.Write("反射调用：");
            Stopwatch watch1 = Stopwatch.StartNew();
            for (int i = 0; i < count; i++)
            {
                GetPropertyValue(order, propertyInfo);
            }
            watch1.Stop();
            Console.WriteLine(watch1.Elapsed.ToString());
            Console.Write("普通调用：");
            Stopwatch watch2 = Stopwatch.StartNew();
            for (int i = 0; i < count; i++)
            {
                order.OrderID = 11;
            }
            watch2.Stop();
            Console.WriteLine(watch2.Elapsed.ToString());
            Console.Write("Emit调用：");
            Action<object, object> setter1 = CreatePropertySetter_Emit(propertyInfo);
            Stopwatch watch3 = Stopwatch.StartNew();
            for (int i = 0; i < count; i++)
            {
                setter1(order, 11);
            }
            watch3.Stop();
            Console.WriteLine(watch3.Elapsed.ToString());
            Console.Write("委托调用：");
            Action<OrderInfo, int> setter2 = (Action<OrderInfo, int>)Delegate.CreateDelegate(typeof(Action<OrderInfo, int>), null, propertyInfo.GetSetMethod());
            Stopwatch watch4 = Stopwatch.StartNew();
            for (int i = 0; i < count; i++)
            {
                setter2(order, 11);
            }
            watch4.Stop();
            Console.WriteLine(watch4.Elapsed.ToString());
            Console.Write("泛型调用：");
            SetterHelper<OrderInfo, int> setter3 = new SetterHelper<OrderInfo, int>(propertyInfo);
            Stopwatch watch5 = Stopwatch.StartNew();
            for (int i = 0; i < count; i++)
            {
                setter3.SetValue(order, 11);
            }
            watch5.Stop();
            Console.WriteLine(watch5.Elapsed.ToString());
            Console.Write("通用接口调用：");
            ISetValue setter4 = SetterHelper<int, int>.CreatePropertySetterHelper(propertyInfo);
            Stopwatch watch6 = Stopwatch.StartNew();
            for (int i = 0; i < count; i++)
            {
                setter4.Set(order, 11);
            }
            watch5.Stop();
            Console.WriteLine(watch6.Elapsed.ToString());
        }

        private static void GetPropertyValue(object o, PropertyInfo propertyInfo)
        {
            //foreach (var item in o.GetType().GetProperties())
            //{
            propertyInfo.SetValue(o, 11, null);
            //}
        }

        /// <summary>
        /// 生成Set属性委托
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        public static Action<object, object> CreatePropertySetter_Emit(PropertyInfo propertyInfo)
        {
            MethodInfo setMethod = propertyInfo.GetSetMethod();
            DynamicMethod dm = new DynamicMethod("PropertySetter", null, new Type[] { typeof(object), typeof(object) }, propertyInfo.DeclaringType, true);
            ILGenerator il = dm.GetILGenerator();
            if (!setMethod.IsStatic)
            {
                il.Emit(OpCodes.Ldarg_0);
            }
            il.Emit(OpCodes.Ldarg_1);
            if (propertyInfo.PropertyType.IsValueType)
            {
                il.Emit(OpCodes.Unbox_Any, propertyInfo.PropertyType);
            }
            else
            {
                il.Emit(OpCodes.Castclass, propertyInfo.PropertyType);
            }
            if (!setMethod.IsStatic && !propertyInfo.DeclaringType.IsValueType)
            {
                il.EmitCall(OpCodes.Callvirt, setMethod, null);
            }
            else
            {
                il.EmitCall(OpCodes.Call, setMethod, null);
            }
            il.Emit(OpCodes.Ret);
            return (Action<object, object>)dm.CreateDelegate(typeof(Action<object, object>));

        }

    }


    public class SetterHelper<TType, TValue> : ISetValue
    {
        private Action<TType, TValue> _setter;
        public SetterHelper(PropertyInfo propertyInfo)
        {
            MethodInfo m = propertyInfo.GetSetMethod(true);
            _setter = (Action<TType, TValue>)Delegate.CreateDelegate(typeof(Action<TType, TValue>), null, m);
        }
        public void SetValue(TType target, TValue val)
        {
            _setter(target, val);
        }
        void ISetValue.Set(object target, object val)
        {
            _setter((TType)target, (TValue)val);
        }

        public static ISetValue CreatePropertySetterHelper(PropertyInfo propertyInfo)
        {
            MethodInfo mi = propertyInfo.GetSetMethod(true);
            Type instanceType = typeof(SetterHelper<,>).MakeGenericType(propertyInfo.DeclaringType, propertyInfo.PropertyType);
            return (ISetValue)Activator.CreateInstance(instanceType, propertyInfo);
        }
    }
}
