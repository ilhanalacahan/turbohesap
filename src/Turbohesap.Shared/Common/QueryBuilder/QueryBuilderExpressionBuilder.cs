using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;

namespace Turbohesap.Shared.Common.QueryBuilder;

/// <summary>
/// QueryGroup yapısını dynamic LINQ ifadesine (Expression), SQL WHERE cümlesine ve
/// LINQ önizleme string'ine çeviren dönüştürücü motor.
/// </summary>
public static class QueryBuilderExpressionBuilder
{
    /// <summary>QueryGroup filtresini IQueryable sorgusuna dinamik olarak uygular.</summary>
    public static IQueryable<T> ApplyQueryGroup<T>(this IQueryable<T> query, QueryGroup group)
    {
        var expression = ToExpression<T>(group);
        return query.Where(expression);
    }

    /// <summary>QueryGroup nesnesini derlenebilir Expression ağacına dönüştürür.</summary>
    public static Expression<Func<T, bool>> ToExpression<T>(QueryGroup group)
    {
        var parameter = Expression.Parameter(typeof(T), "p");
        var body = BuildExpression<T>(parameter, group);
        return Expression.Lambda<Func<T, bool>>(body, parameter);
    }

    private static Expression BuildExpression<T>(ParameterExpression parameter, QueryGroup group)
    {
        var expressions = new List<Expression>();

        foreach (var rule in group.Rules)
        {
            var expr = BuildRuleExpression<T>(parameter, rule);
            if (expr != null) expressions.Add(expr);
        }

        foreach (var subGroup in group.Groups)
        {
            var expr = BuildExpression<T>(parameter, subGroup);
            expressions.Add(expr);
        }

        if (expressions.Count == 0)
        {
            return Expression.Constant(true);
        }

        Expression? combined = null;
        foreach (var expr in expressions)
        {
            if (combined == null)
            {
                combined = expr;
            }
            else
            {
                combined = group.Operator == QueryGroupOperator.And
                    ? Expression.AndAlso(combined, expr)
                    : Expression.OrElse(combined, expr);
            }
        }

        return combined!;
    }

    private static Expression? BuildRuleExpression<T>(ParameterExpression parameter, QueryRule rule)
    {
        if (string.IsNullOrWhiteSpace(rule.Field)) return null;

        var property = typeof(T).GetProperty(rule.Field, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
        if (property == null) return null;

        var left = Expression.Property(parameter, property);

        if (rule.Operator == QueryRuleOperator.IsNull)
        {
            return Expression.Equal(left, Expression.Constant(null, left.Type));
        }
        if (rule.Operator == QueryRuleOperator.IsNotNull)
        {
            return Expression.NotEqual(left, Expression.Constant(null, left.Type));
        }

        try
        {
            var convertedValue = ConvertValue(rule.Value, property.PropertyType);
            var right = Expression.Constant(convertedValue, property.PropertyType);

            switch (rule.Operator)
            {
                case QueryRuleOperator.Equals:
                    return Expression.Equal(left, right);
                case QueryRuleOperator.NotEquals:
                    return Expression.NotEqual(left, right);
                case QueryRuleOperator.GreaterThan:
                    return Expression.GreaterThan(left, right);
                case QueryRuleOperator.LessThan:
                    return Expression.LessThan(left, right);
                case QueryRuleOperator.GreaterThanOrEqual:
                    return Expression.GreaterThanOrEqual(left, right);
                case QueryRuleOperator.LessThanOrEqual:
                    return Expression.LessThanOrEqual(left, right);
                case QueryRuleOperator.Contains:
                    if (property.PropertyType == typeof(string))
                    {
                        var method = typeof(string).GetMethod(nameof(string.Contains), [typeof(string)]);
                        var nonNullCheck = Expression.NotEqual(left, Expression.Constant(null, typeof(string)));
                        var methodCall = Expression.Call(left, method!, right);
                        return Expression.AndAlso(nonNullCheck, methodCall);
                    }
                    break;
                case QueryRuleOperator.NotContains:
                    if (property.PropertyType == typeof(string))
                    {
                        var method = typeof(string).GetMethod(nameof(string.Contains), [typeof(string)]);
                        var nonNullCheck = Expression.NotEqual(left, Expression.Constant(null, typeof(string)));
                        var methodCall = Expression.Call(left, method!, right);
                        return Expression.AndAlso(nonNullCheck, Expression.Not(methodCall));
                    }
                    break;
                case QueryRuleOperator.StartsWith:
                    if (property.PropertyType == typeof(string))
                    {
                        var method = typeof(string).GetMethod(nameof(string.StartsWith), [typeof(string)]);
                        var nonNullCheck = Expression.NotEqual(left, Expression.Constant(null, typeof(string)));
                        var methodCall = Expression.Call(left, method!, right);
                        return Expression.AndAlso(nonNullCheck, methodCall);
                    }
                    break;
                case QueryRuleOperator.EndsWith:
                    if (property.PropertyType == typeof(string))
                    {
                        var method = typeof(string).GetMethod(nameof(string.EndsWith), [typeof(string)]);
                        var nonNullCheck = Expression.NotEqual(left, Expression.Constant(null, typeof(string)));
                        var methodCall = Expression.Call(left, method!, right);
                        return Expression.AndAlso(nonNullCheck, methodCall);
                    }
                    break;
            }
        }
        catch
        {
            // Değer çevrilemezse veya geçersizse eşleşme hatasını engellemek için kuralsız kabul et.
            return Expression.Constant(true);
        }

        return null;
    }

    private static object? ConvertValue(object? value, Type targetType)
    {
        if (value == null) return null;

        var underlyingType = Nullable.GetUnderlyingType(targetType) ?? targetType;

        if (value is JsonElement element)
        {
            switch (element.ValueKind)
            {
                case JsonValueKind.String:
                    if (underlyingType == typeof(Guid)) return Guid.Parse(element.GetString()!);
                    if (underlyingType == typeof(DateTime)) return element.GetDateTime();
                    return Convert.ChangeType(element.GetString(), underlyingType);
                case JsonValueKind.Number:
                    if (underlyingType == typeof(int)) return element.GetInt32();
                    if (underlyingType == typeof(long)) return element.GetInt64();
                    if (underlyingType == typeof(double)) return element.GetDouble();
                    if (underlyingType == typeof(decimal)) return element.GetDecimal();
                    return Convert.ChangeType(element.GetDecimal(), underlyingType);
                case JsonValueKind.True:
                    return true;
                case JsonValueKind.False:
                    return false;
                case JsonValueKind.Null:
                    return null;
            }
        }

        if (underlyingType == typeof(Guid)) return Guid.Parse(value.ToString()!);
        if (underlyingType == typeof(DateTime))
        {
            if (DateTime.TryParse(value.ToString(), out var dt)) return dt;
            return DateTime.MinValue;
        }

        if (underlyingType.IsEnum) return Enum.Parse(underlyingType, value.ToString()!);

        return Convert.ChangeType(value.ToString(), underlyingType);
    }

    /// <summary>QueryGroup nesnesini SQL WHERE cümleciğine dönüştürür.</summary>
    public static string ToSql(QueryGroup group)
    {
        var parts = new List<string>();

        foreach (var rule in group.Rules)
        {
            if (string.IsNullOrWhiteSpace(rule.Field)) continue;
            
            var sqlRule = BuildSqlRule(rule);
            if (!string.IsNullOrEmpty(sqlRule)) parts.Add(sqlRule);
        }

        foreach (var subGroup in group.Groups)
        {
            var sqlSub = ToSql(subGroup);
            if (!string.IsNullOrEmpty(sqlSub) && sqlSub != "1=1") parts.Add($"({sqlSub})");
        }

        if (parts.Count == 0) return "1=1";

        var op = group.Operator == QueryGroupOperator.And ? " AND " : " OR ";
        return string.Join(op, parts);
    }

    private static string BuildSqlRule(QueryRule rule)
    {
        var field = rule.Field.ToLowerInvariant();
        if (rule.Operator == QueryRuleOperator.IsNull) return $"{field} IS NULL";
        if (rule.Operator == QueryRuleOperator.IsNotNull) return $"{field} IS NOT NULL";

        var valStr = rule.Value?.ToString() ?? string.Empty;
        var quotedVal = rule.Type == QueryFieldType.String || rule.Type == QueryFieldType.Date
            ? $"'{valStr.Replace("'", "''")}'"
            : valStr;

        if (string.IsNullOrEmpty(quotedVal)) quotedVal = "''";

        return rule.Operator switch
        {
            QueryRuleOperator.Equals => $"{field} = {quotedVal}",
            QueryRuleOperator.NotEquals => $"{field} <> {quotedVal}",
            QueryRuleOperator.GreaterThan => $"{field} > {quotedVal}",
            QueryRuleOperator.LessThan => $"{field} < {quotedVal}",
            QueryRuleOperator.GreaterThanOrEqual => $"{field} >= {quotedVal}",
            QueryRuleOperator.LessThanOrEqual => $"{field} <= {quotedVal}",
            QueryRuleOperator.Contains => $"{field} LIKE '%{valStr.Replace("'", "''")}%'",
            QueryRuleOperator.NotContains => $"{field} NOT LIKE '%{valStr.Replace("'", "''")}%'",
            QueryRuleOperator.StartsWith => $"{field} LIKE '{valStr.Replace("'", "''")}%'",
            QueryRuleOperator.EndsWith => $"{field} LIKE '%{valStr.Replace("'", "''")}'",
            _ => string.Empty
        };
    }

    /// <summary>QueryGroup nesnesini LINQ lambda dizesine dönüştürür.</summary>
    public static string ToLinqString(QueryGroup group)
    {
        var parts = new List<string>();

        foreach (var rule in group.Rules)
        {
            if (string.IsNullOrWhiteSpace(rule.Field)) continue;
            var linqRule = BuildLinqRule(rule);
            if (!string.IsNullOrEmpty(linqRule)) parts.Add(linqRule);
        }

        foreach (var subGroup in group.Groups)
        {
            var linqSub = ToLinqString(subGroup);
            if (!string.IsNullOrEmpty(linqSub) && linqSub != "true") parts.Add($"({linqSub})");
        }

        if (parts.Count == 0) return "true";

        var op = group.Operator == QueryGroupOperator.And ? " && " : " || ";
        return string.Join(op, parts);
    }

    private static string BuildLinqRule(QueryRule rule)
    {
        var field = $"p.{char.ToUpper(rule.Field[0])}{rule.Field.Substring(1)}";
        
        if (rule.Operator == QueryRuleOperator.IsNull) return $"{field} == null";
        if (rule.Operator == QueryRuleOperator.IsNotNull) return $"{field} != null";

        var valStr = rule.Value?.ToString() ?? string.Empty;
        var quotedVal = rule.Type == QueryFieldType.String
            ? $"\"{valStr.Replace("\"", "\\\"")}\""
            : rule.Type == QueryFieldType.Date
                ? $"DateTime.Parse(\"{valStr}\")"
                : rule.Type == QueryFieldType.Boolean
                    ? valStr.ToLowerInvariant()
                    : valStr;

        if (string.IsNullOrEmpty(quotedVal)) quotedVal = "\"\"";

        return rule.Operator switch
        {
            QueryRuleOperator.Equals => $"{field} == {quotedVal}",
            QueryRuleOperator.NotEquals => $"{field} != {quotedVal}",
            QueryRuleOperator.GreaterThan => $"{field} > {quotedVal}",
            QueryRuleOperator.LessThan => $"{field} < {quotedVal}",
            QueryRuleOperator.GreaterThanOrEqual => $"{field} >= {quotedVal}",
            QueryRuleOperator.LessThanOrEqual => $"{field} <= {quotedVal}",
            QueryRuleOperator.Contains => $"{field} != null && {field}.Contains({quotedVal})",
            QueryRuleOperator.NotContains => $"{field} != null && !{field}.Contains({quotedVal})",
            QueryRuleOperator.StartsWith => $"{field} != null && {field}.StartsWith({quotedVal})",
            QueryRuleOperator.EndsWith => $"{field} != null && {field}.EndsWith({quotedVal})",
            _ => string.Empty
        };
    }
}
