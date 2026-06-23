using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Turbohesap.Shared.Common;
using Turbohesap.Shared.Contracts.Customers;
using Turbohesap.Web.Services;

namespace Turbohesap.Web.Pages.Customers;

/// <summary>
/// Müşteri listesi + oluştur/düzenle/sil. Form alanları değiştikçe sayfa sekmesi "kirli"
/// işaretlenir (<see cref="PageTabHandle"/>); kaydet/iptal sonrası temizlenir. Böylece sekme
/// kapatılırken kaydedilmemiş değişiklik uyarısı tetiklenir.
/// </summary>
public partial class Index
{
    [CascadingParameter] private PageTabHandle? Tab { get; set; }

    private readonly GetCustomersQuery _query = new() { PageSize = 20, SortBy = "createdAtUtc", SortDirection = SortDirection.Descending };
    private PagedResult<CustomerDto> _result = PagedResult<CustomerDto>.Empty(1, 20);
    private bool _loading = true;

    private bool _formOpen;
    private bool _saving;
    private Guid? _editId;
    private CreateCustomerCommand _form = new();
    private EditContext? _editContext;

    private bool _deleteOpen;
    private CustomerDto? _deleteTarget;

    protected override Task OnInitializedAsync() => LoadAsync();

    private async Task LoadAsync()
    {
        _loading = true;
        try
        {
            _result = await Customers.GetListAsync(_query);
        }
        catch (ApiException ex)
        {
            Toasts.Error(ex.Message, "Liste alınamadı");
        }
        finally
        {
            _loading = false;
        }
    }

    private async Task OnSearch(ChangeEventArgs e)
    {
        _query.Search = e.Value?.ToString();
        _query.Page = 1;
        await LoadAsync();
    }

    private async Task OnOnlyActiveChanged(ChangeEventArgs e)
    {
        _query.OnlyActive = (bool?)e.Value == true ? true : null;
        _query.Page = 1;
        await LoadAsync();
    }

    private async Task Sort(string field)
    {
        if (_query.SortBy == field)
        {
            _query.SortDirection = _query.SortDirection == SortDirection.Ascending ? SortDirection.Descending : SortDirection.Ascending;
        }
        else
        {
            _query.SortBy = field;
            _query.SortDirection = SortDirection.Ascending;
        }
        await LoadAsync();
    }

    private RenderFragment SortIcon(string field) => builder =>
    {
        if (_query.SortBy != field) return;
        var icon = _query.SortDirection == SortDirection.Ascending ? "fa-arrow-up-short-wide" : "fa-arrow-down-wide-short";
        builder.AddMarkupContent(0, $"<i class=\"fa-solid {icon}\" style=\"margin-left:.25rem;font-size:.7rem\"></i>");
    };

    private async Task OnPageChanged(int page)
    {
        _query.Page = page;
        await LoadAsync();
    }

    private async Task OnPageSizeChanged(int size)
    {
        _query.PageSize = size;
        _query.Page = 1;
        await LoadAsync();
    }

    private void OpenCreate()
    {
        _editId = null;
        _form = new CreateCustomerCommand { IsActive = true };
        BindForm();
        _formOpen = true;
    }

    private void OpenEdit(CustomerDto customer)
    {
        _editId = customer.Id;
        _form = new CreateCustomerCommand
        {
            Code = customer.Code,
            Name = customer.Name,
            Email = customer.Email,
            PhoneNumber = customer.PhoneNumber,
            TaxNumber = customer.TaxNumber,
            IsActive = customer.IsActive
        };
        BindForm();
        _formOpen = true;
    }

    // Yeni bir EditContext kur; alan değişiminde sekmeyi kirli işaretle.
    private void BindForm()
    {
        if (_editContext is not null)
        {
            _editContext.OnFieldChanged -= OnFieldChanged;
        }
        _editContext = new EditContext(_form);
        _editContext.OnFieldChanged += OnFieldChanged;
        Tab?.MarkClean();
    }

    private void OnFieldChanged(object? sender, FieldChangedEventArgs e) => Tab?.MarkDirty();

    private void CancelForm()
    {
        _formOpen = false;
        Tab?.MarkClean();
    }

    private async Task SaveAsync()
    {
        _saving = true;
        try
        {
            if (_editId is null)
            {
                await Customers.CreateAsync(_form);
                Toasts.Success("Müşteri oluşturuldu.");
            }
            else
            {
                await Customers.UpdateAsync(_editId.Value, new UpdateCustomerCommand
                {
                    Id = _editId.Value,
                    Code = _form.Code,
                    Name = _form.Name,
                    Email = _form.Email,
                    PhoneNumber = _form.PhoneNumber,
                    TaxNumber = _form.TaxNumber,
                    IsActive = _form.IsActive
                });
                Toasts.Success("Müşteri güncellendi.");
            }
            _formOpen = false;
            Tab?.MarkClean();
            await LoadAsync();
        }
        catch (ApiException ex)
        {
            Toasts.Error(ex.Message, "Kaydedilemedi");
        }
        finally
        {
            _saving = false;
        }
    }

    private void OpenDelete(CustomerDto customer)
    {
        _deleteTarget = customer;
        _deleteOpen = true;
    }

    private async Task ConfirmDeleteAsync()
    {
        if (_deleteTarget is null) return;
        _saving = true;
        try
        {
            await Customers.DeleteAsync(_deleteTarget.Id);
            Toasts.Success("Müşteri silindi.");
            _deleteOpen = false;
            await LoadAsync();
        }
        catch (ApiException ex)
        {
            Toasts.Error(ex.Message, "Silinemedi");
        }
        finally
        {
            _saving = false;
        }
    }
}
