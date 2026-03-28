using MapsterMapper;
using MicroBoss.Application.Common;
using MicroBoss.Application.DTOs;
using MicroBoss.Application.Interfaces;
using MicroBoss.Domain.Entities;
using MicroBoss.Domain.Interfaces;

namespace MicroBoss.Application.Services;

public class CustomerService : ICustomerService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public CustomerService(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<PagedResult<CustomerDto>> QueryAsync(CustomerQueryDto query)
    {
        var repo = _uow.Repository<Customer>();
        var q = repo.Query();

        if (!string.IsNullOrWhiteSpace(query.Keyword))
        {
            var kw = query.Keyword.Trim();
            q = q.Where(c =>
                (c.CustomerName != null && c.CustomerName.Contains(kw)) ||
                (c.ShortName != null && c.ShortName.Contains(kw)) ||
                (c.CustomerId.Contains(kw)));
        }

        var totalCount = q.Count();

        var items = q
            .OrderBy(c => c.CustomerId)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToList();

        return new PagedResult<CustomerDto>
        {
            Items = _mapper.Map<List<CustomerDto>>(items),
            TotalCount = totalCount,
            Page = query.Page,
            PageSize = query.PageSize
        };
    }

    public async Task<CustomerDto?> GetByIdAsync(string id)
    {
        var repo = _uow.Repository<Customer>();
        var customer = await repo.GetByIdAsync(id);
        return customer == null ? null : _mapper.Map<CustomerDto>(customer);
    }

    public async Task CreateAsync(CreateCustomerDto dto)
    {
        var customer = _mapper.Map<Customer>(dto);
        customer.CreateTime = DateTime.Now;
        _uow.Repository<Customer>().Add(customer);
        await _uow.SaveChangesAsync();
    }

    public async Task UpdateAsync(string id, CreateCustomerDto dto)
    {
        var repo = _uow.Repository<Customer>();
        var customer = await repo.GetByIdAsync(id)
            ?? throw new InvalidOperationException("客戶不存在");

        customer.CustomerName = dto.CustomerName;
        customer.ShortName = dto.ShortName;
        customer.CustomerType = dto.CustomerType;
        customer.UnitNo = dto.UnitNo;
        customer.TaxType = dto.TaxType;
        customer.InvoiceTitle = dto.InvoiceTitle;
        customer.Tel1 = dto.Tel1;
        customer.Fax = dto.Fax;
        customer.MobileNo = dto.MobileNo;
        customer.Email = dto.Email;
        customer.ContactWindow = dto.ContactWindow;
        customer.DeliveryAddress = dto.DeliveryAddress;
        customer.DeliveryAddressZip = dto.DeliveryAddressZip;
        customer.PaymentMethod = dto.PaymentMethod;
        customer.DeliveryMethod = dto.DeliveryMethod;
        customer.Note = dto.Note;
        customer.LastUpdateTime = DateTime.Now;

        repo.Update(customer);
        await _uow.SaveChangesAsync();
    }

    public async Task DeleteAsync(string id)
    {
        var repo = _uow.Repository<Customer>();
        var customer = await repo.GetByIdAsync(id)
            ?? throw new InvalidOperationException("客戶不存在");
        repo.Remove(customer);
        await _uow.SaveChangesAsync();
    }
}
