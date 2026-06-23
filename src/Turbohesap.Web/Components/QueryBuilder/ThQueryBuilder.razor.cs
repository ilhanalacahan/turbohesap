using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;
using Turbohesap.Shared.Common.QueryBuilder;
using Turbohesap.Web.Components.Base;

namespace Turbohesap.Web.Components.QueryBuilder;

/// <summary>
/// Query Builder bileşeni. Özyinelemeli (recursive) mantıksal gruplama ve operatör
/// kuralları oluşturarak dinamik filtre üretmeyi sağlar.
/// </summary>
public partial class ThQueryBuilder : TurboComponentBase
{
    [Parameter] public List<QueryBuilderField> Fields { get; set; } = [];
    [Parameter] public QueryGroup Value { get; set; } = new();
    [Parameter] public EventCallback<QueryGroup> ValueChanged { get; set; }

    private string RootClass => Cx("th-query-builder", Class);

    protected override void OnParametersSet()
    {
        if (Value == null)
        {
            Value = new QueryGroup();
        }
        
        // Eğer grupta hiç kural veya grup yoksa ve alanlar tanımlıysa, ilk kuralı otomatik ekleyelim
        if (Value.Rules.Count == 0 && Value.Groups.Count == 0 && Fields.Count > 0)
        {
            AddRule(Value);
        }
    }

    private void SetGroupOperator(QueryGroup group, QueryGroupOperator op)
    {
        group.Operator = op;
        NotifyChanged();
    }

    private void AddRule(QueryGroup group)
    {
        if (Fields.Count == 0) return;
        var firstField = Fields[0];
        
        group.Rules.Add(new QueryRule
        {
            Field = firstField.Name,
            Type = firstField.Type,
            Operator = GetOperators(firstField.Type)[0],
            Value = GetDefaultValueForType(firstField.Type)
        });
        
        NotifyChanged();
    }

    private void AddSubGroup(QueryGroup group)
    {
        var newSub = new QueryGroup();
        group.Groups.Add(newSub);
        
        // Alt gruba da başlangıç için bir kural ekle
        AddRule(newSub);
        NotifyChanged();
    }

    private void DeleteRule(QueryGroup group, QueryRule rule)
    {
        group.Rules.Remove(rule);
        NotifyChanged();
    }

    private void DeleteGroup(QueryGroup parentGroup, QueryGroup group)
    {
        parentGroup.Groups.Remove(group);
        NotifyChanged();
    }

    private void OnRuleFieldChange(QueryRule rule, string? fieldName)
    {
        if (string.IsNullOrEmpty(fieldName)) return;
        var field = Fields.FirstOrDefault(f => f.Name == fieldName);
        if (field == null) return;

        rule.Field = field.Name;
        rule.Type = field.Type;
        rule.Operator = GetOperators(field.Type)[0];
        rule.Value = GetDefaultValueForType(field.Type);
        
        NotifyChanged();
    }

    private void OnRuleOperatorChange(QueryRule rule, string? opStr)
    {
        if (string.IsNullOrEmpty(opStr) || !Enum.TryParse<QueryRuleOperator>(opStr, out var op)) return;
        
        rule.Operator = op;
        NotifyChanged();
    }

    private void OnRuleValueChange(QueryRule rule, object? val)
    {
        if (rule.Type == QueryFieldType.Boolean && val is string s)
        {
            rule.Value = bool.TryParse(s, out var b) ? b : true;
        }
        else if (rule.Type == QueryFieldType.Number)
        {
            if (decimal.TryParse(val?.ToString(), out var d)) rule.Value = d;
            else rule.Value = 0;
        }
        else
        {
            rule.Value = val;
        }
        
        NotifyChanged();
    }

    private void NotifyChanged()
    {
        _ = ValueChanged.InvokeAsync(Value);
        StateHasChanged();
    }

    private List<QueryRuleOperator> GetOperators(QueryFieldType type)
    {
        return type switch
        {
            QueryFieldType.Boolean => [QueryRuleOperator.Equals, QueryRuleOperator.NotEquals],
            QueryFieldType.Number => [
                QueryRuleOperator.Equals, QueryRuleOperator.NotEquals,
                QueryRuleOperator.GreaterThan, QueryRuleOperator.LessThan,
                QueryRuleOperator.GreaterThanOrEqual, QueryRuleOperator.LessThanOrEqual,
                QueryRuleOperator.IsNull, QueryRuleOperator.IsNotNull
            ],
            QueryFieldType.Date => [
                QueryRuleOperator.Equals, QueryRuleOperator.NotEquals,
                QueryRuleOperator.GreaterThan, QueryRuleOperator.LessThan,
                QueryRuleOperator.GreaterThanOrEqual, QueryRuleOperator.LessThanOrEqual,
                QueryRuleOperator.IsNull, QueryRuleOperator.IsNotNull
            ],
            _ => [
                QueryRuleOperator.Equals, QueryRuleOperator.NotEquals,
                QueryRuleOperator.Contains, QueryRuleOperator.NotContains,
                QueryRuleOperator.StartsWith, QueryRuleOperator.EndsWith,
                QueryRuleOperator.IsNull, QueryRuleOperator.IsNotNull
            ]
        };
    }

    private string GetOperatorLabel(QueryRuleOperator op)
    {
        return op switch
        {
            QueryRuleOperator.Equals => "Eşit (=)",
            QueryRuleOperator.NotEquals => "Eşit Değil (!=)",
            QueryRuleOperator.Contains => "İçerir",
            QueryRuleOperator.NotContains => "İçermez",
            QueryRuleOperator.StartsWith => "Başlar",
            QueryRuleOperator.EndsWith => "Biter",
            QueryRuleOperator.GreaterThan => "Büyük (>)",
            QueryRuleOperator.LessThan => "Küçük (<)",
            QueryRuleOperator.GreaterThanOrEqual => "Büyük Eşit (>=)",
            QueryRuleOperator.LessThanOrEqual => "Küçük Eşit (<=)",
            QueryRuleOperator.IsNull => "Boş / Yok",
            QueryRuleOperator.IsNotNull => "Dolu / Var",
            _ => op.ToString()
        };
    }

    private object? GetDefaultValueForType(QueryFieldType type)
    {
        return type switch
        {
            QueryFieldType.Boolean => true,
            QueryFieldType.Number => 0,
            QueryFieldType.Date => DateTime.Now.ToString("yyyy-MM-dd"),
            _ => string.Empty
        };
    }

    private string FormatDate(object? value)
    {
        if (value is DateTime dt) return dt.ToString("yyyy-MM-dd");
        if (value is string s && DateTime.TryParse(s, out var dt2)) return dt2.ToString("yyyy-MM-dd");
        return DateTime.Now.ToString("yyyy-MM-dd");
    }
}
