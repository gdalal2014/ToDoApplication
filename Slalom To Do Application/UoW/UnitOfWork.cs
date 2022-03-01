using System;
using System.Data;
using MySql.Data;
using Slalom_To_Do_Application.Repository;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace Slalom_To_Do_Application.UoW
{
    public class UnitOfWork : IUnitOfWork
    {
        private IDbConnection newConnection;
        private IDbTransaction newTransaction;
        public bool _disposed { get; private set; } = false;

        public UnitOfWork(string connectionString)
        {

            newConnection = new MySqlConnection(connectionString);
            
        }

        public void Connect() 
        {
           newConnection.Open();
        }
        public IDbTransaction Begin()
        {
            return newTransaction = newConnection.BeginTransaction();

        }
        public void Commit()
        {
            try
            {
                newTransaction.Commit();
            }
            catch
            {
                newTransaction.Rollback();
                throw;
            }
            finally
            {
                newTransaction.Dispose();
                newTransaction = newConnection.BeginTransaction();
            }
        }
        public void Rollback()
        {
            newTransaction.Rollback();
            Dispose();
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
                    if (newTransaction != null)
                    {
                        newTransaction.Dispose();
                    }
                    if (newConnection != null)
                    {
                        newConnection.Dispose();
                    }
                }
                _disposed = true;
            }
        }

    }

}
