using Dapper;
using Slalom_To_Do_Application.UoW;
using System;
using System.Collections.Generic;
using MySql.Data;
using System.Linq;
using System.Data;
using Autofac;
using Slalom_To_Do_Application.Entities;

namespace Slalom_To_Do_Application.Repository
{
    public class UserRepository: IUserRepository
    {
        private ILifetimeScope _autofacContainer { get;}
        protected IDbTransaction _newTransaction { get; set; }
        protected IDbConnection Connection { get { if (_newTransaction != null) { return _newTransaction.Connection; } else { return null; } } }

        private IUnitOfWork _uow;

        private bool _disposed  = false;
        public UserRepository(ILifetimeScope autofacContainer)
        {
            _autofacContainer = autofacContainer;
        }

        public IEnumerable<UserEntity> All()
        {
            using (var scope = _autofacContainer.BeginLifetimeScope())
            {
                _uow = scope.Resolve<IUnitOfWork>();
                _uow.Connect();
                _newTransaction = _uow.Begin();
                return Connection.Query<UserEntity>(
                "SELECT * FROM slalomtodolist.users_tbl",
                transaction: _newTransaction
            ).ToList();
            }
        }

        public void Add(UserEntity entity)
        {
            using (var scope = _autofacContainer.BeginLifetimeScope())
            {
                _uow = scope.Resolve<IUnitOfWork>();
                _uow.Connect();
                _newTransaction = _uow.Begin();
                entity.user_id = Connection.ExecuteScalar<string>(
                "INSERT INTO slalomtodolist.users_tbl(user_first_name, user_last_name, create_date, modified_date) VALUES(@FirstName, @LastName, sysdate(), sysdate());",
                param: new { FirstName = entity.user_first_name, LastName = entity.user_last_name },
                transaction: _newTransaction
            );
                _uow.Commit();
                _uow.Dispose();
            }
        }

        public void Update(UserEntity entity)
        {
            using (var scope = _autofacContainer.BeginLifetimeScope())
            {
                _uow = scope.Resolve<IUnitOfWork>();
                _uow.Connect();
                _newTransaction = _uow.Begin();
                Connection.Execute(
                "UPDATE slalomtodolist.users_tbl SET user_first_name = @FirstName, user_last_name = @LastName, modified_date = sysdate() WHERE user_Id = @UserId",
                param: new { FirstName = entity.user_first_name, LastName = entity.user_last_name, UserId = entity.user_id },
                transaction: _newTransaction
            );
                _uow.Commit();
                _uow.Dispose();
            }
        }


        public IEnumerable<UserEntity> Find(string userID)
        {
            using (var scope = _autofacContainer.BeginLifetimeScope())
            {
                _uow = scope.Resolve<IUnitOfWork>();
                _uow.Connect();
                _newTransaction = _uow.Begin();
                return Connection.Query<UserEntity>(
                "SELECT * FROM slalomtodolist.users_tbl WHERE user_id = @userID",
                param: new { userID = userID },
                transaction: _newTransaction
            );
                }
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

