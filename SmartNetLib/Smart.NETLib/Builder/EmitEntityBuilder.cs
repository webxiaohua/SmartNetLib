using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data;
using System.Reflection.Emit;

namespace Smart.NETLib.Builder
{
    /// <summary>
    /// DataRow和IDataReader转换实体类
    /// </summary>
    /// <typeparam name="ItemType"></typeparam>
    public class EmitEntityBuilder<ItemType>
    {
        #region 不可改变的参数
        private static readonly MethodInfo getRow =
                typeof(DataRow).GetMethod("get_Item", new Type[] { typeof(int) }); //反射 获取索引器的值  set_Item  dr[0] 
        private static readonly MethodInfo isDBNullRow =
                typeof(DataRow).GetMethod("IsNull", new Type[] { typeof(int) });    // IsNull(0)

        private static readonly MethodInfo getRecord =
                typeof(IDataRecord).GetMethod("get_Item", new Type[] { typeof(int) });
        private static readonly MethodInfo isDBNullRecord =
                typeof(IDataRecord).GetMethod("IsDBNull", new Type[] { typeof(int) });
        #endregion

        // 自定义转换实体委托
        public delegate ItemType DynamicMethodDelegate<T>(T paramObjs);

        private EmitEntityBuilder() { }
        /// <summary>
        /// 创建委托
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public static DynamicMethodDelegate<DataRow> CreateHandler(DataRow row)
        {
            System.Type itemType = typeof(ItemType);
            DynamicMethod method = new DynamicMethod("DynamicCreateEntity",
                itemType,
                new Type[] { typeof(DataRow) },
                itemType, true);   //构造动态IL代码  method  方法名，返回类型，参数类型 动态方法逻辑关联的类型 是否跳过JIT可见性检查

            ILGenerator generator = method.GetILGenerator();//IL代码生成器
            LocalBuilder result = generator.DeclareLocal(itemType);//局部变量 
            generator.Emit(OpCodes.Newobj, itemType.GetConstructor(Type.EmptyTypes));
            generator.Emit(OpCodes.Stloc, result);//从堆栈顶部弹出当前值，并将其存储到指定索引出的局部变量列表中

            for (int i = 0; i < row.ItemArray.Length; i++)
            {
                PropertyInfo propertyInfo
                    = itemType.GetProperty(row.Table.Columns[i].ColumnName);
                Label endIfLabel = generator.DefineLabel(); //指令流中的标签  与 ILGenerator 类一起使用。
                if (propertyInfo != null && propertyInfo.GetSetMethod() != null)
                {
                    generator.Emit(OpCodes.Ldarg_0);//将索引为 0 的参数加载到计算堆栈上。
                    generator.Emit(OpCodes.Ldc_I4, i);//将所提供的 int32 类型的值作为 int32 推送到计算堆栈上。
                    generator.Emit(OpCodes.Callvirt, isDBNullRow);//对对象调用后期绑定方法，并且将返回值推送到计算堆栈上。
                    generator.Emit(OpCodes.Brtrue, endIfLabel);//如果 value 为 true、非空或非零，则将控制转移到目标指令。
                    generator.Emit(OpCodes.Ldloc, result);//将指定索引处的局部变量加载到计算堆栈上。
                    generator.Emit(OpCodes.Ldarg_0);//将索引为 0 的参数加载到计算堆栈上。
                    generator.Emit(OpCodes.Ldc_I4, i);//将所提供的 int32 类型的值作为 int32 推送到计算堆栈上。
                    generator.Emit(OpCodes.Callvirt, getRow);//对对象调用后期绑定方法，并且将返回值推送到计算堆栈上。
                    generator.Emit(OpCodes.Unbox_Any, propertyInfo.PropertyType);//将指令中指定类型的已装箱的表示形式转换成未装箱形式。
                    generator.Emit(OpCodes.Callvirt, propertyInfo.GetSetMethod());//对对象调用后期绑定方法，并且将返回值推送到计算堆栈上。
                    generator.MarkLabel(endIfLabel);
                }
            }
            generator.Emit(OpCodes.Ldloc, result);
            generator.Emit(OpCodes.Ret);

            DynamicMethodDelegate<DataRow> handler
                   = (DynamicMethodDelegate<DataRow>)method.CreateDelegate(typeof(DynamicMethodDelegate<DataRow>));
            return handler;
        }
        public static DynamicMethodDelegate<IDataRecord> CreateHandler(IDataRecord dataRecord)
        {
            System.Type itemType = typeof(ItemType);
            DynamicMethod method = new DynamicMethod("DynamicCreateEntity",
                    itemType,
                    new Type[] { typeof(IDataRecord) },
                    itemType, true);
            ILGenerator generator = method.GetILGenerator();
            LocalBuilder result = generator.DeclareLocal(itemType);
            generator.Emit(OpCodes.Newobj, itemType.GetConstructor(Type.EmptyTypes));
            generator.Emit(OpCodes.Stloc, result);
            for (int i = 0; i < dataRecord.FieldCount; i++)
            {
                PropertyInfo propertyInfo
                    = itemType.GetProperty(dataRecord.GetName(i));
                Label endIfLabel = generator.DefineLabel();
                if (propertyInfo != null && propertyInfo.GetSetMethod() != null)
                {
                    generator.Emit(OpCodes.Ldarg_0);
                    generator.Emit(OpCodes.Ldc_I4, i);
                    generator.Emit(OpCodes.Callvirt, isDBNullRecord);
                    generator.Emit(OpCodes.Brtrue, endIfLabel);
                    generator.Emit(OpCodes.Ldloc, result);
                    generator.Emit(OpCodes.Ldarg_0);
                    generator.Emit(OpCodes.Ldc_I4, i);
                    generator.Emit(OpCodes.Callvirt, getRecord);
                    generator.Emit(OpCodes.Unbox_Any, dataRecord.GetFieldType(i));
                    generator.Emit(OpCodes.Callvirt, propertyInfo.GetSetMethod());
                    generator.MarkLabel(endIfLabel);
                }
            }
            generator.Emit(OpCodes.Ldloc, result);
            generator.Emit(OpCodes.Ret);

            DynamicMethodDelegate<IDataRecord> handler
                   = (DynamicMethodDelegate<IDataRecord>)method.CreateDelegate(typeof(DynamicMethodDelegate<IDataRecord>));
            return handler;
        }
    }
}
