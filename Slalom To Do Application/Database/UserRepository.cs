using Dapper;
using Slalom_To_Do_Application.Database;
using System;
using System.Collections.Generic;
using MySql.Data;
using System.Linq;
using System.Data;

namespace Slalom_To_Do_Application
{
    public class UserRepository: IUserRepository
    {
        protected IDbTransaction _newTransaction { get; } 
        protected IDbConnection Connection { get { return _newTransaction.Connection; } }
        
        private IUnitOfWork _uow;

        private bool _disposed  = false;
        public UserRepository(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.Connect();
            _newTransaction = _uow.Begin();
        }

        public IEnumerable<UserEntity> All()
        {
            
            return Connection.Query<UserEntity>(
                "SELECT * FROM slalomtodolist.users_tbl",
                transaction: _newTransaction
            ).ToList();
        }

        public void Add(UserEntity entity)
        {
          
            entity.user_id = Connection.ExecuteScalar<string>(
                "INSERT INTO slalomtodolist.users_tbl(user_first_name, user_last_name, create_date, modified_date) VALUES(@FirstName, @LastName, sysdate(), sysdate());",
                param: new { FirstName = entity.user_first_name, LastName =  entity.user_last_name },
                transaction: _newTransaction
            );
            _uow.Commit();
        }

        public void Update(UserEntity entity)
        {
          
            Connection.Execute(
                "UPDATE slalomtodolist.users_tbl SET user_first_name = @FirstName, user_last_name = @LastName, modified_date = sysdate() WHERE user_Id = @UserId",
                param: new { FirstName = entity.user_first_name,LastName = entity.user_last_name,  UserId = entity.user_id},
                transaction: _newTransaction
            );
            _uow.Commit();
        }


        public IEnumerable<UserEntity> Find(string userID)
        {
           
            return Connection.Query<UserEntity>(
                "SELECT * FROM slalomtodolist.users_tbl WHERE user_id = @userID",
                param: new { userID = userID },
                transaction: _newTransaction
            );
        }

        public void Dispose()
        {
            dispose(true);
            GC.SuppressFinalize(this);
        }

        private void dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_newTransaction != null)
                    {
                        _newTransaction.Dispose();
                    }
                    if (Connection != null)
                    {
                        Connection.Dispose();
                    }
                }
                _disposed = true;
            }
        }
    }
}

