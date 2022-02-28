using System;
using System.Collections.Generic;
using Slalom_To_Do_Application.Entities;

namespace Slalom_To_Do_Application.Repository
{
    public interface IToDoRepository : IDisposable
    {
            void Add(ToDoEntity entity);
            IEnumerable<ToDoEntity> All();
            IEnumerable<ToDoEntity> Find(string user_id);
            void Update(ToDoEntity entity);
           void UpdateCompletedEntry(ToDoEntity entity);
        }
    }


