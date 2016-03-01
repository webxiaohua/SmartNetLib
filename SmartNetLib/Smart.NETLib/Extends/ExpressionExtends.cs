using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

/// <summary>
/// @Author : Robin
/// @Date:20160226
/// @Desc:Expression 表达式树扩展函数
/// </summary>
public static class ExpressionExtends
{
    /// <summary>
    /// 类似于&&操作
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static Expression AndAlso(this Expression left, Expression right)
    {
        return Expression.AndAlso(left, right);
    }

    public static Expression Call(this Expression instance, string methodName, params Expression[] arguments)
    {
        return Expression.Call(instance, instance.Type.GetMethod(methodName), arguments);
    }
}

