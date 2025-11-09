using CleanContosoUniversity.Domain.Common;
using CleanContosoUniversity.Domain.Entities;

namespace CleanContosoUniversity.Domain.Events;

public class TodoItemCompletedEvent : BaseEvent
{
    public TodoItemCompletedEvent(TodoItem item)
    {
        Item = item;
    }

    public TodoItem Item { get; }
}