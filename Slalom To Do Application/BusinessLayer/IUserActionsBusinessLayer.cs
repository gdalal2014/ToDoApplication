using System;
using System.Collections.Generic;
using Slalom_To_Do_Application.Entities;

namespace Slalom_To_Do_Application.BusinessLayer
{
    public interface IUserActionsBusinessLayer
    {
        IEnumerable<UserEntity> retriveAllUsers();

        IEnumerable<UserEntity> retriveUser(string _userId);

        void createNewUser(UserEntity _user);

        void updateExistingUser(UserEntity _user);

    }
}
