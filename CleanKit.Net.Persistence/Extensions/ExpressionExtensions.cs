using System.Linq.Expressions;
using System.Reflection;

namespace CleanKit.Net.Persistence.Extensions;

public static class ExpressionExtensions
{
    public static string? GetMemberName(this Expression memberSelector)
    {
        var currentExpression = memberSelector;

        while (true)
            switch (currentExpression.NodeType)
            {
                case ExpressionType.Parameter:
                    return ((ParameterExpression)currentExpression).Name;
                case ExpressionType.MemberAccess:
                    return ((MemberExpression)currentExpression).Member.Name;
                case ExpressionType.Call:
                    return ((MethodCallExpression)currentExpression).Method.Name;
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                    currentExpression = ((UnaryExpression)currentExpression).Operand;
                    break;
                case ExpressionType.Invoke:
                    currentExpression = ((InvocationExpression)currentExpression).Expression;
                    break;
                case ExpressionType.ArrayLength:
                    return "Length";
                default:
                    throw new Exception("not a proper member selector");
            }
    }

    public static Type? GetMemberType(this Expression memberSelector)
    {
        var currentExpression = memberSelector;

        while (true)
            switch (currentExpression.NodeType)
            {
                case ExpressionType.Parameter:
                    return ((ParameterExpression)currentExpression).Type;
                case ExpressionType.MemberAccess:
                    return ((MemberExpression)currentExpression).Member.ExtractType();
                case ExpressionType.Call:
                    return ((MethodCallExpression)currentExpression).Method.ReturnType;
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                    currentExpression = ((UnaryExpression)currentExpression).Operand;
                    break;
                case ExpressionType.Invoke:
                    currentExpression = ((InvocationExpression)currentExpression).Expression;
                    break;
                case ExpressionType.ArrayLength:
                    return typeof(int);
                default:
                    throw new Exception("not a proper member selector");
            }
    }

    public static Type? ExtractType(this MemberInfo member)
    {
        switch (member.MemberType)
        {
            case MemberTypes.Event:
                return ((EventInfo)member).EventHandlerType;
            case MemberTypes.Field:
                return ((FieldInfo)member).FieldType;
            case MemberTypes.Method:
                return ((MethodInfo)member).ReturnType;
            case MemberTypes.Property:
                return ((PropertyInfo)member).PropertyType;
            default:
                throw new ArgumentException(
                    "Input MemberInfo must be if type EventInfo, FieldInfo, MethodInfo, or PropertyInfo"
                );
        }
    }
}
