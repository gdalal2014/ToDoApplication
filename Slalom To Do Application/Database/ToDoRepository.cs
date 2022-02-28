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
        protected IDbTransaction _newTransaction { get; }
        protected IDbConnection Connection { get { return _newTransaction.Connection; } }

        private IUnitOfWork _uow;

        private bool _disposed = false;
        public ToDoRepository(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.Connect();
            _newTransaction = _uow.Begin();
        }

        public IEnumerable<ToDoEntity> All()
        {
            
            return Connection.Query<ToDoEntity>(
                "SELECT list_id, user_id, item, create_date, modified_date, is_completed, date_format(completion_date, '%m/%d/%Y') AS completion_date FROM slalomtodolist.list_tbl",
                transaction: _newTransaction
            ).ToList();
        }

        public IEnumerable<ToDoEntity> Find(string user_id)
        {
           
            return Connection.Query<ToDoEntity>(
                "SELECT list_id, user_id, item, create_date, modified_date, is_completed, date_format(completion_date, '%m/%d/%Y') AS completion_date FROM slalomtodolist.list_tbl WHERE user_id = @userId",
                param: new { userId = user_id },
                transaction: _newTransaction
            ).ToList();
        }

        public void Add(ToDoEntity entity)
        {
           
            entity.list_id = Connection.ExecuteScalar<string>(
                "INSERT INTO slalomtodolist.list_tbl(user_id, item, create_date, modified_date, is_completed, completion_date) VALUES(@userId, @item, sysdate(), sysdate(), @completed, STR_TO_DATE(@completionDate,'%Y-%m-%d %H:%i:%s'));",
                
                param: new { userId = entity.user_id, item = entity.item, completed = entity.is_completed.ToUpper(), completionDate = entity.completion_date },
                transaction: _newTransaction
            );
            _uow.Commit();
        }

        public void Update(ToDoEntity entity)
        {
          
            Connection.Execute(
                "UPDATE slalomtodolist.list_tbl SET item = @item, modified_date = sysdate(), is_completed = @completed, completion_date = STR_TO_DATE(@completionDate,'%Y-%m-%d %H:%i:%s') WHERE user_id = @userId and list_id = @listId",
                param: new { Item = entity.item, completed = entity.is_completed, completionDate = entity.completion_date, userId= entity.user_id, listId = entity.list_id },
                transaction: _newTransaction
            );
            _uow.Commit();
        }

        public void UpdateCompletedEntry(ToDoEntity entity)
        {
           
            Connection.Execute(
                "UPDATE slalomtodolist.list_tbl SET modified_date = sysdate(), is_completed = 'Y' WHERE user_id = @userId and list_id = @listId",
                param: new { userId = entity.user_id, listId = entity.list_id },
                transaction: _newTransaction
            );
            _uow.Commit();
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

