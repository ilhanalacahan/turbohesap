using System;
using System.Collections.Generic;

namespace Turbohesap.Shared.Common.QueryBuilder;

/// <summary>Query Builder mantıksal grup operatörü (AND / OR).</summary>
public enum QueryGroupOperator
{
    And,
    Or
}

/// <summary>Query Builder kural operatörleri.</summary>
public enum QueryRuleOperator
{
    Equals,
    NotEquals,
    Contains,
    NotContains,
    StartsWith,
    EndsWith,
    GreaterThan,
    LessThan,
    GreaterThanOrEqual,
    LessThanOrEqual,
    IsNull,
    IsNotNull
}

/// <summary>Alan veri tipleri.</summary>
public enum QueryFieldType
{
    String,
    Number,
    Boolean,
    Date
}

/// <summary>Showcase veya diğer tablolar için tanımlanabilir alan metadata'sı.</summary>
public sealed class QueryBuilderField
{
    public string Name { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public QueryFieldType Type { get; set; } = QueryFieldType.String;
}

/// <summary>Tek bir filtreleme kuralı.</summary>
public sealed class QueryRule
{
    public string Field { get; set; } = string.Empty;
    public QueryRuleOperator Operator { get; set; } = QueryRuleOperator.Equals;
    public QueryFieldType Type { get; set; } = QueryFieldType.String;
    public object? Value { get; set; }
}

/// <summary>Operatörle birleştirilmiş filtre grubu. Özyinelemeli (recursive) yapıya sahiptir.</summary>
public sealed class QueryGroup
{
    public QueryGroupOperator Operator { get; set; } = QueryGroupOperator.And;
    public List<QueryRule> Rules { get; set; } = [];
    public List<QueryGroup> Groups { get; set; } = [];
}
