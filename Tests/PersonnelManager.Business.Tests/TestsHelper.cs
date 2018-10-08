using System;
using System.Linq;
using System.Linq.Expressions;

namespace PersonnelManager.Business.Tests
{
    static class TestsHelper
    {
        public static bool HasAttribute<T, TAttribute>(Expression<Func<T, object>> expression)
            where TAttribute : Attribute
        {
            try
            {
                var memberExpression = (MemberExpression)expression.Body;
                return memberExpression
                    .Member
                    .GetCustomAttributes(typeof(TAttribute), false).Length > 0;
            }
            catch
            {
                return false;
            }
        }
        public static TAttribute GetAttribute<T, TAttribute>(Expression<Func<T, object>> expression)
            where TAttribute : Attribute
        {
            try
            {
                var memberExpression = (MemberExpression)expression.Body;
                return memberExpression
                    .Member
                    .GetCustomAttributes(typeof(TAttribute), false).FirstOrDefault() as TAttribute;
            }
            catch
            {
                return null;
            }
        }
    }
}
