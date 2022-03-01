using System;
using System.Collections.Generic;
using Slalom_To_Do_Application.Entities;

namespace Slalom_To_Do_Application.Repository
{
    public interface IUserRepository 
    {
        void Add(UserEntity entity);
        IEnumerable<UserEntity> All();
        IEnumerable<UserEntity> Find(string userID);
        void Update(UserEntity entity);
    }
}
