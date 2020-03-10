using System;
using System.Collections.Generic;
using System.Linq;
using Todo.Data.Entities;
using Todo.EntityModelMappers.TodoItems;
using Todo.Models.TodoLists;

namespace Todo.EntityModelMappers.TodoLists
{
    public static class TodoListDetailViewmodelFactory
    {
        public static TodoListDetailViewmodel Create(TodoList todoList, bool descendingSortOrder, bool doneItemsHidden)
        {
            var items = todoList
                .Items
                .Where(p => !doneItemsHidden || p.IsDone == false)
                .Select(TodoItemSummaryViewmodelFactory.Create)
                .OrderByWithDirection(i => i.Importance, false)
                .ThenByWithDirection(i => i.Rank, descendingSortOrder)
                .ToList();
            return new TodoListDetailViewmodel(todoList.TodoListId, todoList.Title, items, descendingSortOrder, doneItemsHidden);
        }

        public static IOrderedEnumerable<TSource> OrderByWithDirection<TSource, TKey>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            bool descending)
        {
            return descending ? source.OrderByDescending(keySelector) : source.OrderBy(keySelector);
        }

        public static IOrderedEnumerable<TSource> ThenByWithDirection<TSource, TKey>(
            this IOrderedEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            bool descending)
        {
            return descending ? source.ThenByDescending(keySelector) : source.ThenBy(keySelector);
        }
    }
}