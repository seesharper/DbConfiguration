using System;
using System.Data;

namespace DbConfiguration
{
    public static class ConfigurationExtensions
    {
        public static IDbConnection Configure<TConnection>(this IDbConnection dbConnection, Action<TConnection> configureConnection) where TConnection : IDbConnection
        {
            configureConnection(DbConfigurationOptions.GetInnerConnection<TConnection>(dbConnection));
            return dbConnection;
        }

        public static IDbCommand Configure<TCommand>(this IDbCommand dbCommand, Action<TCommand> configureCommand) where TCommand : IDbCommand
        {
            configureCommand(DbConfigurationOptions.GetInnerCommand<TCommand>(dbCommand));
            return dbCommand;
        }

        public static IDataReader Configure<TDataReader>(this IDataReader dataReader, Action<TDataReader> configureDataReader) where TDataReader : IDataReader
        {
            configureDataReader(DbConfigurationOptions.GetInnerDataReader<TDataReader>(dataReader));
            return dataReader;
        }

        public static IDbTransaction Configure<TDbTransaction>(this IDbTransaction dbTransaction, Action<TDbTransaction> configureDataReader) where TDbTransaction : IDbTransaction
        {
            configureDataReader(DbConfigurationOptions.GetInnerDbTransaction<TDbTransaction>(dbTransaction));
            return dbTransaction;
        }
    }
}