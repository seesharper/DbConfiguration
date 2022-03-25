using System;
using System.Data;

namespace DbConfiguration
{
    /// <summary>
    /// A set of extension methods to configure probider-specific implementations
    /// of <see cref="IDbConnection"/>, <see cref="IDbCommand"/>, <see cref="IDataReader"/> and <see cref="IDbTransaction"/>.
    /// </summary>
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// Configures the underlying <see cref="IDbConnection"/>.
        /// </summary>
        /// <typeparam name="TConnection">The underlying <see cref="IDbConnection"/> implementation.</typeparam>
        /// <param name="dbConnection">The target <see cref="IDbConnection"/>.</param>
        /// <param name="configureConnection">The function to configure the underlying <see cref="IDbConnection"/>.</param>
        /// <returns>This <paramref name="dbConnection"/> for chaining calls</returns>
        public static IDbConnection Configure<TConnection>(this IDbConnection dbConnection, Action<TConnection> configureConnection) where TConnection : IDbConnection
        {
            configureConnection(DbConfigurationOptions.GetInnerConnection<TConnection>(dbConnection));
            return dbConnection;
        }

        /// <summary>
        /// Configures the underlying <see cref="IDbCommand"/>.
        /// </summary>
        /// <typeparam name="TCommand">The underlying <see cref="IDbCommand"/> implementation.</typeparam>
        /// <param name="dbCommand">The target <see cref="IDbCommand"/>.</param>
        /// <param name="configureCommand">The function to configure the underlying <see cref="IDbCommand"/>.</param>
        /// <returns>This <paramref name="dbCommand"/> for chaining calls</returns>
        public static IDbCommand Configure<TCommand>(this IDbCommand dbCommand, Action<TCommand> configureCommand) where TCommand : IDbCommand
        {
            configureCommand(DbConfigurationOptions.GetInnerCommand<TCommand>(dbCommand));
            return dbCommand;
        }

        /// <summary>
        /// Configures the underlying <see cref="IDataReader"/>.
        /// </summary>
        /// <typeparam name="TDataReader">The underlying <see cref="IDataReader"/> implementation.</typeparam>
        /// <param name="dataReader">The target <see cref="IDataReader"/>.</param>
        /// <param name="configureDataReader">The function to configure the underlying <see cref="IDataReader"/>.</param>
        /// <returns>This <paramref name="dataReader"/> for chaining calls</returns>
        public static IDataReader Configure<TDataReader>(this IDataReader dataReader, Action<TDataReader> configureDataReader) where TDataReader : IDataReader
        {
            configureDataReader(DbConfigurationOptions.GetInnerDataReader<TDataReader>(dataReader));
            return dataReader;
        }

        /// <summary>
        /// Configures the underlying <see cref="IDbTransaction"/>.
        /// </summary>
        /// <typeparam name="TTransaction">The underlying <see cref="IDbTransaction"/> implementation.</typeparam>
        /// <param name="dbTransaction">The target <see cref="IDbTransaction"/>.</param>
        /// <param name="configureTransaction">The function to configure the underlying <see cref="IDbTransaction"/>.</param>
        /// <returns>This <paramref name="dbTransaction"/> for chaining calls</returns>
        public static IDbTransaction Configure<TTransaction>(this IDbTransaction dbTransaction, Action<TTransaction> configureTransaction) where TTransaction : IDbTransaction
        {
            configureTransaction(DbConfigurationOptions.GetInnerDbTransaction<TTransaction>(dbTransaction));
            return dbTransaction;
        }
    }
}