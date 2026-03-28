using MicroBoss.Application.DTOs;

namespace MicroBoss.Application.Interfaces;

public interface IBoschImportService
{
    Task ImportAsync(List<SpSourceDataDto> items);
    Task ResetAsync();
}
