using Slalom_To_Do_Application.Entities;
using System.Collections.Generic;

namespace Slalom_To_Do_Application.BusinessLayer
{
    public interface IToDoActionBusinessLayer
    {
        IEnumerable<ToDoEntity> retriveToDoItem(string _userId);

        void createNewToDoItem(ToDoEntity _toDoEntity);

        void updateExistingToDoItem(ToDoEntity _toDoEntity);

        void completeToDoItem (ToDoEntity _toDoEntity);
    }
}
