using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Reflection;
using System.Linq;

/// <summary>
/// Tostring 工具类
/// </summary>
public static class ObjectExtensions
{
    public static string ToStringReflection<T>(this T @this)
    {
        var query = from prop in @this.GetType().GetProperties(
                    BindingFlags.Instance | BindingFlags.Public)
                    where prop.CanRead
                    select string.Format("{0}: {1}\n",
                    prop.Name,
                    prop.GetValue(@this, null));

        string[] fields = query.ToArray();
        StringBuilder format = new StringBuilder();

        foreach (string field in fields)
        {
            format.Append(field);
        }

        return format.ToString();
    }
}