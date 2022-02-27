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
        protected IDbTransaction Transaction { get; }
        protected IDbConnection Connection { get { return Transaction.Connection; } }
        private bool _disposed  = false;
        public UserRepository(IDbTransaction transaction)
        {
           Transaction = transaction;
        }

        public IEnumerable<UserEntity> All()
        {
            return Connection.Query<UserEntity>(
                "SELECT * FROM slalomtodolist.users_tbl",
                transaction: Transaction
            ).ToList();
        }

        public void Add(UserEntity entity)
        {
            entity.user_id = Connection.ExecuteScalar<string>(
                "INSERT INTO slalomtodolist.users_tbl(user_first_name, user_last_name, create_date, modified_date) VALUES(@FirstName, @LastName, sysdate(), sysdate());",
                param: new { FirstName = entity.user_first_name, LastName =  entity.user_last_name },
                transaction: Transaction
            );
        }

        public void Update(UserEntity entity)
        {
            Connection.Execute(
                "UPDATE slalomtodolist.users_tbl SET user_first_name = @FirstName, user_last_name = @LastName, modified_date = sysdate() WHERE user_Id = @UserId",
                param: new { FirstName = entity.user_first_name,LastName = entity.user_last_name,  UserId = entity.user_id},
                transaction: Transaction
            );
        }


        public IEnumerable<UserEntity> Find(string userID)
        {
            return Connection.Query<UserEntity>(
                "SELECT * FROM slalomtodolist.users_tbl WHERE user_id = @userID",
                param: new { userID = userID },
                transaction: Transaction
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
                    if (Transaction != null)
                    {
                        Transaction.Dispose();
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

