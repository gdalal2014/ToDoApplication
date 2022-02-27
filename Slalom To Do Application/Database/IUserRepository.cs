using System;
using System.Collections.Generic;

namespace Slalom_To_Do_Application.Database
{
    public interface IUserRepository : IDisposable
    {
        void Add(UserEntity entity);
        IEnumerable<UserEntity> All();
        IEnumerable<UserEntity> Find(string userID);
        void Update(UserEntity entity);
    }
}
