using Autofac;
using Slalom_To_Do_Application.Entities;
using Slalom_To_Do_Application.Repository;
using System.Collections.Generic;

namespace Slalom_To_Do_Application.BusinessLayer
{
    public class UserActionBusinessLayer : IUserActionsBusinessLayer
    {

        private IUserRepository _userRepository;
        public UserActionBusinessLayer (IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public void createNewUser(UserEntity _user)
        {
            _userRepository.Add(_user);
        }

        public IEnumerable<UserEntity> retriveAllUsers()
        {
                return _userRepository.All();
        }

        public IEnumerable<UserEntity> retriveUser(string _userId)
        {
            return _userRepository.Find(_userId);
        }

        public void updateExistingUser(UserEntity _user)
        {
            _userRepository.Update(_user);
        }
    }
}
