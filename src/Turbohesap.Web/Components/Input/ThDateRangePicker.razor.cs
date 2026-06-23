using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Turbohesap.Web.Components.Base;

namespace Turbohesap.Web.Components.Input;

/// <summary>
/// Preset kısayolları ve interaktif çift takvim desteği sunan estetik tarih aralığı seçici bileşeni.
/// </summary>
public partial class ThDateRangePicker : TurboComponentBase
{
    private static readonly CultureInfo TrCulture = new CultureInfo("tr-TR");

    [Parameter] public DateTime? StartDate { get; set; }
    [Parameter] public EventCallback<DateTime?> StartDateChanged { get; set; }
    [Parameter] public DateTime? EndDate { get; set; }
    [Parameter] public EventCallback<DateTime?> EndDateChanged { get; set; }

    [Parameter] public string Label { get; set; } = "";
    [Parameter] public string Placeholder { get; set; } = "Tarih Aralığı Seçiniz";
    [Parameter] public bool Disabled { get; set; } = false;
    [Parameter] public ComponentSize Size { get; set; } = ComponentSize.Md;

    private bool _isOpen = false;
    private DateTime _leftCalendarMonth = DateTime.Today;
    private DateTime _rightCalendarMonth = DateTime.Today.AddMonths(1);
    
    // Geçici seçim durumları (Uygula butonuna basılana kadar tutulur)
    private DateTime? _tempStartDate;
    private DateTime? _tempEndDate;
    private DateTime? _hoveredDate;

    protected override void OnParametersSet()
    {
        _tempStartDate = StartDate;
        _tempEndDate = EndDate;

        if (StartDate.HasValue)
        {
            _leftCalendarMonth = StartDate.Value;
            _rightCalendarMonth = StartDate.Value.AddMonths(1);
        }
        else
        {
            _leftCalendarMonth = DateTime.Today;
            _rightCalendarMonth = DateTime.Today.AddMonths(1);
        }
    }

    private string RootClass => Cx(
        "th-daterange-picker-wrapper",
        SizeClass(Size, "th-daterange-picker-wrapper"),
        Class);

    private string InputValue => (StartDate.HasValue && EndDate.HasValue)
        ? $"{StartDate.Value.ToString("dd.MM.yyyy", TrCulture)} - {EndDate.Value.ToString("dd.MM.yyyy", TrCulture)}"
        : "";

    private void ToggleDropdown()
    {
        if (Disabled) return;
        _isOpen = !_isOpen;
        if (_isOpen)
        {
            _tempStartDate = StartDate;
            _tempEndDate = EndDate;
        }
    }

    private void CloseDropdown()
    {
        _isOpen = false;
    }

    private void PrevMonth()
    {
        _leftCalendarMonth = _leftCalendarMonth.AddMonths(-1);
        _rightCalendarMonth = _rightCalendarMonth.AddMonths(-1);
    }

    private void NextMonth()
    {
        _leftCalendarMonth = _leftCalendarMonth.AddMonths(1);
        _rightCalendarMonth = _rightCalendarMonth.AddMonths(1);
    }

    private void HandleDayClick(DateTime date)
    {
        if (!_tempStartDate.HasValue || (_tempStartDate.HasValue && _tempEndDate.HasValue))
        {
            _tempStartDate = date;
            _tempEndDate = null;
        }
        else if (_tempStartDate.HasValue && !_tempEndDate.HasValue)
        {
            if (date < _tempStartDate.Value)
            {
                _tempStartDate = date;
            }
            else
            {
                _tempEndDate = date;
            }
        }
    }

    private void HandleDayHover(DateTime date)
    {
        if (_tempStartDate.HasValue && !_tempEndDate.HasValue)
        {
            _hoveredDate = date;
        }
    }

    private bool IsSelected(DateTime date)
    {
        return date == _tempStartDate || date == _tempEndDate;
    }

    private bool IsInRange(DateTime date)
    {
        if (_tempStartDate.HasValue && _tempEndDate.HasValue)
        {
            return date > _tempStartDate.Value && date < _tempEndDate.Value;
        }
        
        if (_tempStartDate.HasValue && _hoveredDate.HasValue)
        {
            return date > _tempStartDate.Value && date <= _hoveredDate.Value;
        }

        return false;
    }

    private bool IsRangeStart(DateTime date)
    {
        return date == _tempStartDate;
    }

    private bool IsRangeEnd(DateTime date)
    {
        return date == _tempEndDate || (date == _hoveredDate && _tempStartDate.HasValue && !_tempEndDate.HasValue);
    }

    private async Task ApplyRange()
    {
        StartDate = _tempStartDate;
        EndDate = _tempEndDate;

        await StartDateChanged.InvokeAsync(StartDate);
        await EndDateChanged.InvokeAsync(EndDate);

        _isOpen = false;
    }

    private async Task ClearRange()
    {
        _tempStartDate = null;
        _tempEndDate = null;
        _hoveredDate = null;

        StartDate = null;
        EndDate = null;

        await StartDateChanged.InvokeAsync(null);
        await EndDateChanged.InvokeAsync(null);

        _isOpen = false;
    }

    // Preset Kısayolları
    private void SelectPreset(string rangeType)
    {
        var today = DateTime.Today;
        switch (rangeType)
        {
            case "today":
                _tempStartDate = today;
                _tempEndDate = today;
                break;
            case "yesterday":
                _tempStartDate = today.AddDays(-1);
                _tempEndDate = today.AddDays(-1);
                break;
            case "this-week":
                int diff = (7 + (today.DayOfWeek - DayOfWeek.Monday)) % 7;
                _tempStartDate = today.AddDays(-1 * diff);
                _tempEndDate = _tempStartDate.Value.AddDays(6);
                break;
            case "last-week":
                int diffLast = (7 + (today.DayOfWeek - DayOfWeek.Monday)) % 7;
                _tempStartDate = today.AddDays(-1 * diffLast - 7);
                _tempEndDate = _tempStartDate.Value.AddDays(6);
                break;
            case "this-month":
                _tempStartDate = new DateTime(today.Year, today.Month, 1);
                _tempEndDate = _tempStartDate.Value.AddMonths(1).AddDays(-1);
                break;
            case "last-month":
                var lastMonth = today.AddMonths(-1);
                _tempStartDate = new DateTime(lastMonth.Year, lastMonth.Month, 1);
                _tempEndDate = _tempStartDate.Value.AddMonths(1).AddDays(-1);
                break;
            case "last-7-days":
                _tempStartDate = today.AddDays(-7);
                _tempEndDate = today;
                break;
            case "last-30-days":
                _tempStartDate = today.AddDays(-30);
                _tempEndDate = today;
                break;
            case "this-year":
                _tempStartDate = new DateTime(today.Year, 1, 1);
                _tempEndDate = new DateTime(today.Year, 12, 31);
                break;
        }

        _leftCalendarMonth = _tempStartDate ?? today;
        _rightCalendarMonth = _leftCalendarMonth.AddMonths(1);
    }

    private IEnumerable<DateTime> GetDaysInMonth(DateTime monthDate)
    {
        var year = monthDate.Year;
        var month = monthDate.Month;
        var firstDayOfMonth = new DateTime(year, month, 1);
        
        // Haftanın ilk gününü Pazartesi yapmak için ofset bulalım
        // Pazartesi = 1, Pazar = 0 (DayOfWeek)
        int offset = ((int)firstDayOfMonth.DayOfWeek - 1 + 7) % 7;
        
        var startOfCalendar = firstDayOfMonth.AddDays(-offset);

        // Takvim ızgarasını 42 güne tamamla (6 satır x 7 sütun)
        for (int i = 0; i < 42; i++)
        {
            yield return startOfCalendar.AddDays(i);
        }
    }
}
