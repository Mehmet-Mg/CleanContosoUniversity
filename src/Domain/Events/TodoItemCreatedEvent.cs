using CleanContosoUniversity.Domain.Common;
using CleanContosoUniversity.Domain.Entities;

namespace CleanContosoUniversity.Domain.Events;
public class TodoItemCreatedEvent : BaseEvent
{
    public TodoItemCreatedEvent(TodoItem item)
    {
        Item = item;
    }

    public TodoItem Item { get; }
}