using Slalom_To_Do_Application.Database;
using System;
using System.Data;

namespace Slalom_To_Do_Application
{
    public interface IUnitOfWork : IDisposable
    {
        // IUserRepository UserRepository { get; }
        //   IToDoRepository ToDoRepository { get; }

        void Connect();
        IDbTransaction Begin();
        void Commit();
        void Rollback();
    }
}

