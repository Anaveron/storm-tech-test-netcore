using System.Collections.Generic;
using System.ComponentModel;
using Todo.Models.TodoItems;

namespace Todo.Models.TodoLists
{
    public class TodoListDetailViewmodel
    {
        public int TodoListId { get; }
        public string Title { get; }
        public ICollection<TodoItemSummaryViewmodel> Items { get; }
        [DisplayName("Hide done items")]
        public bool DoneItemsHidden { get; }
        public bool DescendingSortOrder { get; }

        public TodoListDetailViewmodel(int todoListId, string title, ICollection<TodoItemSummaryViewmodel> items, bool descendingSortOrder, bool doneItemsHidden)
        {
            Items = items;
            TodoListId = todoListId;
            Title = title;
            DescendingSortOrder = descendingSortOrder;
            DoneItemsHidden = doneItemsHidden;
        }
    }
}