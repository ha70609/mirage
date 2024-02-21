using Creationline.Mirage.Application.Common.Models;

namespace Creationline.Mirage.Application.TodoLists.Queries;

public class TodosVm
{
    public IReadOnlyCollection<LookupDto> PriorityLevels { get; init; } = Array.Empty<LookupDto>();

    public IReadOnlyCollection<TodoListDto> Lists { get; init; } = Array.Empty<TodoListDto>();
}
