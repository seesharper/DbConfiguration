using System;
using System.Data;

namespace DbConfiguration
{
    /// <summary>
    /// A set of extension methods to configure provider-specific implementations
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
            configureConnection(dbConnection.GetInner<TConnection>());
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
            configureCommand(dbCommand.GetInner<TCommand>());
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
            configureDataReader(dataReader.GetInner<TDataReader>());
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
            configureTransaction(dbTransaction.GetInner<TTransaction>());
            return dbTransaction;
        }

        /// <summary>
        /// Gets the underlying/inner connection represented by <paramref name="dbConnection"/>.
        /// </summary>
        /// <typeparam name="TConnection">The type of the underlying/inner connection.</typeparam>
        /// <param name="dbConnection">The <see cref="IDbConnection"/> from which to get the underlying/inner connection.</param>
        /// <returns>The underlying/inner connection represented by <paramref name="dbConnection"/>.</returns>
        public static TConnection GetInner<TConnection>(this IDbConnection dbConnection) where TConnection : IDbConnection
            => DbConfigurationOptions.GetInnerConnection<TConnection>(dbConnection);

        /// <summary>
        /// Gets the underlying/inner command represented by <paramref name="dbCommand"/>.
        /// </summary>
        /// <typeparam name="TCommand">The type of the underlying/inner command.</typeparam>
        /// <param name="dbCommand">The <see cref="IDbCommand"/> from which to get the underlying/inner command.</param>
        /// <returns>The underlying/inner command represented by <paramref name="dbCommand"/>.</returns>
        public static TCommand GetInner<TCommand>(this IDbCommand dbCommand) where TCommand : IDbCommand
            => DbConfigurationOptions.GetInnerCommand<TCommand>(dbCommand);

        /// <summary>
        /// Gets the underlying/inner datareader represented by <paramref name="dataReader"/>.
        /// </summary>
        /// <typeparam name="TDataReader">The type of the underlying/inner datareader.</typeparam>
        /// <param name="dataReader">The <see cref="IDataReader"/> from which to get the underlying/inner datareader.</param>
        /// <returns>The underlying/inner datareader represented by <paramref name="dataReader"/>.</returns>
        public static TDataReader GetInner<TDataReader>(this IDataReader dataReader) where TDataReader : IDataReader
            => DbConfigurationOptions.GetInnerDataReader<TDataReader>(dataReader);

        /// <summary>
        /// Gets the underlying/inner transaction represented by <paramref name="dbTransaction"/>.
        /// </summary>
        /// <typeparam name="TTransaction">The type of the underlying/inner transaction.</typeparam>
        /// <param name="dbTransaction">The <see cref="IDbTransaction"/> from which to get the underlying/inner transaction.</param>
        /// <returns>The underlying/inner connection represented by <paramref name="dbTransaction"/>.</returns>
        public static TTransaction GetInner<TTransaction>(this IDbTransaction dbTransaction) where TTransaction : IDbTransaction
            => DbConfigurationOptions.GetInnerDbTransaction<TTransaction>(dbTransaction);
    }
}