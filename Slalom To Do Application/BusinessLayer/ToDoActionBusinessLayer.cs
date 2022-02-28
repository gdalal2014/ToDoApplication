using Slalom_To_Do_Application.Entities;
using Slalom_To_Do_Application.Repository;
using System.Collections.Generic;

namespace Slalom_To_Do_Application.BusinessLayer
{
    public class ToDoActionBusinessLayer : IToDoActionBusinessLayer
    {
        private IToDoRepository _todoRepository;

        public ToDoActionBusinessLayer (IToDoRepository todoRepository)
        {
            _todoRepository = todoRepository;
        }

        public void completeToDoItem(ToDoEntity _toDoEntity)
        {
                _todoRepository.UpdateCompletedEntry(_toDoEntity);
        }

        public void createNewToDoItem(ToDoEntity _toDoEntity)
        {
            _todoRepository.Add(_toDoEntity);
        }

        public IEnumerable<ToDoEntity> retriveToDoItem(string _userId)
        {
            return _todoRepository.Find(_userId);
        }

        public void updateExistingToDoItem(ToDoEntity _toDoEntity)
        {
            _todoRepository.Update(_toDoEntity);
        }
    }
}
