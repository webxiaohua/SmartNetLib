using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Xml.Linq;

/// <summary>
/// @Author:webxiaohua
/// @QQ:1584004295
/// @Date:2016-02-18
/// @Desc:XML扩展函数类
/// </summary>
public static class XmlExtends
{
    /// <summary>
    /// XElement的定义中查询包含name的XObject枚举
    /// </summary>
    /// <param name="source"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static IEnumerable<XObject> FindName(this XElement source, string name)
    {
        if (source.Attributes().Any())
        {
            foreach (XAttribute att in source.Attributes())
            {
                string attname = att.Name.ToString();
                if (attname.Contains(name))
                    yield return att;
            }
        }
        if (source.Elements().Any())
        {
            foreach (XElement child in source.Elements())
                foreach (XObject o in child.FindName(name))
                    yield return o;
        }
        else
        {
            string contents = source.Name.ToString();
            if (contents.Contains(name))
                yield return source;
        }
    }
    /// <summary>
    /// XElement的值中查询包含value的XObject枚举
    /// </summary>
    /// <param name="source"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static IEnumerable<XObject> FindValue(this XElement source, string value)
    {
        if (source.Attributes().Any())
        {
            foreach (XAttribute att in source.Attributes())
            {
                string Contents = (string)att;
                if (Contents.Contains(value))
                    yield return att;
            }
        }
        if (source.Elements().Any())
        {
            foreach (XElement child in source.Elements())
            {
                foreach (XObject o in child.FindValue(value))
                    yield return o;
            }
        }
        else
        {
            string Contents = (string)source;
            if (Contents.Contains(value))
                yield return source;

        }
    }

    /// <summary>
    /// 转xml
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    public static string Xml(this XElement element)
    {
        StringBuilder buffer = new StringBuilder(1000);
        XmlTextWriter writer = new XmlTextWriter(new StringWriter(buffer));
        element.WriteTo(writer);
        return buffer.ToString();
    }

    /// <summary>
    /// 转xml
    /// </summary>
    /// <param name="doc"></param>
    /// <returns></returns>       
    public static string ToXml(this XmlDocument doc)
    {
        StringBuilder buffer = new StringBuilder(1000);
        XmlTextWriter writer = new XmlTextWriter(new StringWriter(buffer));
        doc.WriteContentTo(writer);
        return buffer.ToString();
    }
}

