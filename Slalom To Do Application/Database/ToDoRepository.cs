using Dapper;
using Slalom_To_Do_Application.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
namespace Slalom_To_Do_Application.Database
{
    public class ToDoRepository : IToDoRepository
    {
        protected IDbTransaction Transaction { get; }
        protected IDbConnection Connection { get { return Transaction.Connection; } }
        private bool _disposed = false;
        public ToDoRepository(IDbTransaction transaction)
        {
            Transaction = transaction;
        }

        public IEnumerable<ToDoEntity> All()
        {
            return Connection.Query<ToDoEntity>(
                "SELECT list_id, user_id, item, create_date, modified_date, is_completed, date_format(completion_date, '%m/%d/%Y') AS completion_date FROM slalomtodolist.list_tbl",
                transaction: Transaction
            ).ToList();
        }

        public IEnumerable<ToDoEntity> Find(string user_id)
        {
            return Connection.Query<ToDoEntity>(
                "SELECT list_id, user_id, item, create_date, modified_date, is_completed, date_format(completion_date, '%m/%d/%Y') AS completion_date FROM slalomtodolist.list_tbl WHERE user_id = @userId",
                param: new { userId = user_id },
                transaction: Transaction
            ).ToList();
        }

        public void Add(ToDoEntity entity)
        {
            entity.list_id = Connection.ExecuteScalar<string>(
                "INSERT INTO slalomtodolist.list_tbl(user_id, item, create_date, modified_date, is_completed, completion_date) VALUES(@userId, @item, sysdate(), sysdate(), @completed, STR_TO_DATE(@completionDate,'%Y-%m-%d %H:%i:%s'));",
                
                param: new { userId = entity.user_id, item = entity.item, completed = entity.is_completed.ToUpper(), completionDate = entity.completion_date },
                transaction: Transaction
            );
        }

        public void Update(ToDoEntity entity)
        {
            Connection.Execute(
                "UPDATE slalomtodolist.list_tbl SET item = @item, modified_date = sysdate(), is_completed = @completed, completion_date = STR_TO_DATE(@completionDate,'%Y-%m-%d %H:%i:%s') WHERE user_id = @userId and list_id = @listId",
                param: new { Item = entity.item, completed = entity.is_completed, completionDate = entity.completion_date, userId= entity.user_id, listId = entity.list_id },
                transaction: Transaction
            );
        }

        public void UpdateCompletedEntry(ToDoEntity entity)
        {
            Connection.Execute(
                "UPDATE slalomtodolist.list_tbl SET modified_date = sysdate(), is_completed = 'Y' WHERE user_id = @userId and list_id = @listId",
                param: new { userId = entity.user_id, listId = entity.list_id },
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

