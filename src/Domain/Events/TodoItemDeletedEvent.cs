using CleanContosoUniversity.Domain.Common;
using CleanContosoUniversity.Domain.Entities;

namespace CleanContosoUniversity.Domain.Events;

public class TodoItemDeletedEvent : BaseEvent
{
    public TodoItemDeletedEvent(TodoItem item)
    {
        Item = item;
    }

    public TodoItem Item { get; }
}