using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Turbohesap.Shared.Common;
using Turbohesap.Shared.Contracts.Customers;
using Turbohesap.Shared.Security;
using Wolverine;

namespace Turbohesap.Api.Controllers.V1;

/// <summary>
/// Müşteri (cari) uç noktaları. Tüm liste sorguları sayfalıdır (req 13). Her uç noktanın
/// erişebilen rolleri Scalar dokümantasyonunda otomatik görüntülenir (req 11, 12).
/// </summary>
[ApiVersion(ApiVersions.V1)]
[Authorize]
public sealed class CustomersController(IMessageBus bus) : ApiControllerBase(bus)
{
    /// <summary>Müşterileri sayfalı, aranabilir ve sıralanabilir biçimde listeler.</summary>
    [HttpGet]
    [Authorize(Roles = $"{Roles.Administrator},{Roles.Manager},{Roles.Accountant}")]
    [ProducesResponseType<PagedResult<CustomerDto>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResult<CustomerDto>>> GetList(
        [FromQuery] GetCustomersQuery query, CancellationToken cancellationToken)
        => Ok(await Bus.InvokeAsync<PagedResult<CustomerDto>>(query, cancellationToken));

    /// <summary>Kimliğe göre tek müşteri getirir.</summary>
    [HttpGet("{id:guid}")]
    [Authorize(Roles = $"{Roles.Administrator},{Roles.Manager},{Roles.Accountant}")]
    [ProducesResponseType<CustomerDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetail>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CustomerDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var customer = await Bus.InvokeAsync<CustomerDto?>(new GetCustomerByIdQuery(id), cancellationToken);
        return customer is null ? NotFound(new ProblemDetail("Müşteri bulunamadı.")) : Ok(customer);
    }

    /// <summary>Yeni müşteri oluşturur.</summary>
    [HttpPost]
    [Authorize(Roles = $"{Roles.Administrator},{Roles.Manager}")]
    [ProducesResponseType<CustomerDto>(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetail>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetail>(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<CustomerDto>> Create(
        [FromBody] CreateCustomerCommand command, CancellationToken cancellationToken)
    {
        var created = await Bus.InvokeAsync<CustomerDto>(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = created.Id, version = ApiVersions.V1 }, created);
    }

    /// <summary>Var olan müşteriyi günceller.</summary>
    [HttpPut("{id:guid}")]
    [Authorize(Roles = $"{Roles.Administrator},{Roles.Manager}")]
    [ProducesResponseType<CustomerDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetail>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CustomerDto>> Update(
        Guid id, [FromBody] UpdateCustomerCommand command, CancellationToken cancellationToken)
    {
        command.Id = id;
        return Ok(await Bus.InvokeAsync<CustomerDto>(command, cancellationToken));
    }

    /// <summary>Müşteriyi siler.</summary>
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = Roles.Administrator)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetail>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await Bus.InvokeAsync<bool>(new DeleteCustomerCommand(id), cancellationToken);
        return NoContent();
    }
}
