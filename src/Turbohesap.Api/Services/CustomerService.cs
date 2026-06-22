using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Turbohesap.Api.Common;
using Turbohesap.Api.Entities;
using Turbohesap.Api.Features.Customers;
using Turbohesap.Api.Persistence;
using Turbohesap.Shared.Common;
using Turbohesap.Shared.Contracts.Customers;
using Turbohesap.Shared.Services;
using Wolverine;

namespace Turbohesap.Api.Services;

/// <summary>
/// Müşteri iş mantığı. Yazma işlemleri transactional çalışır; okuma işlemleri AsNoTracking
/// kullanır ve Mapster ile doğrudan DTO'ya projekte edilir (req 8, 10). Olaylar Wolverine
/// veriyolu üzerinden yayınlanır (req 6).
/// </summary>
public sealed class CustomerService(
    AppDbContext db,
    IMapper mapper,
    IMessageBus bus,
    ICurrentUser currentUser) : ICustomerService
{
    public async Task<PagedResult<CustomerDto>> GetListAsync(GetCustomersQuery query, CancellationToken cancellationToken = default)
    {
        var queryable = db.Customers.AsNoTracking();

        if (query.OnlyActive is true)
        {
            queryable = queryable.Where(c => c.IsActive);
        }

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var term = $"%{query.Search.Trim()}%";
            queryable = queryable.Where(c =>
                EF.Functions.ILike(c.Name, term) ||
                EF.Functions.ILike(c.Code, term) ||
                (c.Email != null && EF.Functions.ILike(c.Email, term)));
        }

        queryable = ApplySort(queryable, query);

        return await queryable
            .ProjectToType<CustomerDto>()
            .ToPagedResultAsync(query, cancellationToken);
    }

    public async Task<CustomerDto?> GetByIdAsync(GetCustomerByIdQuery query, CancellationToken cancellationToken = default)
    {
        return await db.Customers
            .AsNoTracking()
            .Where(c => c.Id == query.Id)
            .ProjectToType<CustomerDto>()
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<CustomerDto> CreateAsync(CreateCustomerCommand command, CancellationToken cancellationToken = default)
    {
        var codeExists = await db.Customers.AnyAsync(c => c.Code == command.Code, cancellationToken);
        if (codeExists)
        {
            throw new ConflictException($"'{command.Code}' kodlu bir müşteri zaten mevcut.");
        }

        await using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);

        var entity = mapper.Map<Customer>(command);
        entity.CreatedAtUtc = DateTime.UtcNow;
        entity.CreatedBy = currentUser.UserName ?? currentUser.UserId;

        db.Customers.Add(entity);
        await db.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);

        await bus.PublishAsync(new CustomerCreatedEvent(entity.Id, entity.Code, entity.Name));

        return mapper.Map<CustomerDto>(entity);
    }

    public async Task<CustomerDto> UpdateAsync(UpdateCustomerCommand command, CancellationToken cancellationToken = default)
    {
        var entity = await db.Customers.FirstOrDefaultAsync(c => c.Id == command.Id, cancellationToken)
            ?? throw new NotFoundException("Güncellenecek müşteri bulunamadı.");

        var codeTaken = await db.Customers
            .AnyAsync(c => c.Code == command.Code && c.Id != command.Id, cancellationToken);
        if (codeTaken)
        {
            throw new ConflictException($"'{command.Code}' kodu başka bir müşteri tarafından kullanılıyor.");
        }

        await using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);

        mapper.Map(command, entity);
        entity.UpdatedAtUtc = DateTime.UtcNow;
        entity.UpdatedBy = currentUser.UserName ?? currentUser.UserId;

        await db.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);

        await bus.PublishAsync(new CustomerUpdatedEvent(entity.Id, entity.Code, entity.Name));

        return mapper.Map<CustomerDto>(entity);
    }

    public async Task<bool> DeleteAsync(DeleteCustomerCommand command, CancellationToken cancellationToken = default)
    {
        var entity = await db.Customers.FirstOrDefaultAsync(c => c.Id == command.Id, cancellationToken)
            ?? throw new NotFoundException("Silinecek müşteri bulunamadı.");

        await using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);

        db.Customers.Remove(entity);
        await db.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);

        await bus.PublishAsync(new CustomerDeletedEvent(entity.Id, entity.Code));

        return true;
    }

    private static IQueryable<Customer> ApplySort(IQueryable<Customer> queryable, GetCustomersQuery query)
    {
        var descending = query.SortDirection == SortDirection.Descending;

        return query.SortBy?.Trim().ToLowerInvariant() switch
        {
            "name" => descending ? queryable.OrderByDescending(c => c.Name) : queryable.OrderBy(c => c.Name),
            "code" => descending ? queryable.OrderByDescending(c => c.Code) : queryable.OrderBy(c => c.Code),
            "isactive" => descending ? queryable.OrderByDescending(c => c.IsActive) : queryable.OrderBy(c => c.IsActive),
            "createdatutc" => descending ? queryable.OrderByDescending(c => c.CreatedAtUtc) : queryable.OrderBy(c => c.CreatedAtUtc),
            _ => queryable.OrderByDescending(c => c.CreatedAtUtc)
        };
    }
}
