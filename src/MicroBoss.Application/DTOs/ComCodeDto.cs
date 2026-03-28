namespace MicroBoss.Application.DTOs;

public record ComCodeDto(string GroupId, string CodeId, string CodeName, int? SortIndex);
public record CreateComCodeDto(string GroupId, string CodeId, string CodeName, int? SortIndex);
public record UpdateComCodeDto(string CodeName, int? SortIndex);
