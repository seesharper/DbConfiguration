using System;
using System.Data;

/// <summary>
/// Configuration options for how to gain access to the underlying
/// provider-specific types.
/// </summary>
public class DbConfigurationOptions
{
    private static Func<IDbConnection, IDbConnection> _configureConnectionAccessor;
    private static Func<IDbCommand, IDbCommand> _configureCommandAccessor;
    private static Func<IDataReader, IDataReader> _configureDataReaderAccessor;
    private static Func<IDbTransaction, IDbTransaction> _configureTransactionAccessor;

    /// <summary>
    /// Clears out all configuration.
    /// </summary>
    public static void Clear()
    {
        _configureConnectionAccessor = (connection) => connection;
        _configureCommandAccessor = (command) => command;
        _configureDataReaderAccessor = (dataReader) => dataReader;
        _configureTransactionAccessor = (transaction) => transaction;
    }

    static DbConfigurationOptions() => Clear();

    /// <summary>
    /// Configures how to access the underlying <see cref="IDbConnection"/>.
    /// </summary>
    /// <typeparam name="TConnection">The type of <see cref="IDbConnection"/> for which to get the underlying/inner type.</typeparam>
    /// <typeparam name="TInnerConnection">The underlying/inner <see cref="IDbConnection"/> implementing type.</typeparam>
    /// <param name="configureAccessor">A function that configures how to access the underlying <see cref="IDbConnection"/>.</param>
    public static void ConfigureConnectionAccessor<TConnection, TInnerConnection>(Func<TConnection, TInnerConnection> configureAccessor) where TConnection : IDbConnection where TInnerConnection : IDbConnection
        => _configureConnectionAccessor = (connection) => configureAccessor((TConnection)connection);

    /// <summary>
    /// Configures how to access the underlying <see cref="IDbCommand"/>.
    /// </summary>
    /// <typeparam name="TCommand">The type of <see cref="IDbCommand"/> for which to get the underlying/inner type.</typeparam>
    /// <typeparam name="TInnerCommand">The underlying/inner <see cref="IDbCommand"/> implementing type.</typeparam>
    /// <param name="configureAccessor">A function that configures how to access the underlying <see cref="IDbCommand"/>.</param>
    public static void ConfigureCommandAccessor<TCommand, TInnerCommand>(Func<TCommand, TInnerCommand> configureAccessor) where TCommand : IDbCommand where TInnerCommand : IDbCommand
        => _configureCommandAccessor = (command) => configureAccessor((TCommand)command);


    /// <summary>
    /// Configures how to access the underlying <see cref="IDataReader"/>.
    /// </summary>
    /// <typeparam name="TDataReader">The type of <see cref="IDataReader"/> for which to get the underlying/inner type.</typeparam>
    /// <typeparam name="TInnerDataReader">The underlying/inner <see cref="IDataReader"/> implementing type.</typeparam>
    /// <param name="configureAccessor">A function that configures how to access the underlying <see cref="IDataReader"/>.</param>
    public static void ConfigureDataReaderAccessor<TDataReader, TInnerDataReader>(Func<TDataReader, TInnerDataReader> configureAccessor) where TDataReader : IDataReader where TInnerDataReader : IDataReader
        => _configureDataReaderAccessor = (dataReader) => configureAccessor((TDataReader)dataReader);

    /// <summary>
    /// Configures how to access the underlying <see cref="IDbTransaction"/>.
    /// </summary>
    /// <typeparam name="TTransaction">The type of <see cref="IDbTransaction"/> for which to get the underlying/inner type.</typeparam>
    /// <typeparam name="TInnerTransaction">The underlying/inner <see cref="IDbTransaction"/> implementing type.</typeparam>
    /// <param name="configureAccessor">A function that configures how to access the underlying <see cref="IDbTransaction"/>.</param>
    public static void ConfigureTransactionAccessor<TTransaction, TInnerTransaction>(Func<TTransaction, TInnerTransaction> configureAccessor) where TTransaction : IDbTransaction where TInnerTransaction : IDbTransaction
        => _configureTransactionAccessor = (transaction) => configureAccessor((TTransaction)transaction);

    internal static TConnection GetInnerConnection<TConnection>(IDbConnection dbConnection) where TConnection : IDbConnection
    {
        var innerConnection = _configureConnectionAccessor(dbConnection);
        if (innerConnection is TConnection connection)
        {
            return connection;
        }

        throw new InvalidOperationException($"Unable to resolve {typeof(TConnection)} from {dbConnection.GetType()}. Use 'DbConfigurationOptions.ConfigureConnectionAccessor<{dbConnection.GetType().Name}, {typeof(TConnection).Name}>() to setup how to access {typeof(TConnection).Name}'");
    }

    internal static TCommand GetInnerCommand<TCommand>(IDbCommand dbCommand) where TCommand : IDbCommand
    {
        var innerCommand = _configureCommandAccessor(dbCommand);
        if (innerCommand is TCommand command)
        {
            return command;
        }
        throw new InvalidOperationException($"Unable to resolve {typeof(TCommand)} from {dbCommand.GetType()}. Use 'DbConfigurationOptions.ConfigureCommandAccessor<{dbCommand.GetType().Name}, {typeof(TCommand).Name}>() to setup how to access {typeof(TCommand).Name}'");
    }

    internal static TDataReader GetInnerDataReader<TDataReader>(IDataReader dataReader) where TDataReader : IDataReader
    {
        var innerDataReader = _configureDataReaderAccessor(dataReader);
        if (innerDataReader is TDataReader reader)
        {
            return reader;
        }
        throw new InvalidOperationException($"Unable to resolve {typeof(TDataReader)} from {dataReader.GetType()}. Use 'DbConfigurationOptions.ConfigureDataReaderAccessor<{dataReader.GetType().Name}, {typeof(TDataReader).Name}>() to setup how to access {typeof(TDataReader).Name}'");
    }

    internal static TTransaction GetInnerDbTransaction<TTransaction>(IDbTransaction dbTransaction) where TTransaction : IDbTransaction
    {
        var innerDataReader = _configureTransactionAccessor(dbTransaction);
        if (innerDataReader is TTransaction reader)
        {
            return reader;
        }
        throw new InvalidOperationException($"Unable to resolve {typeof(TTransaction)} from {dbTransaction.GetType()}. Use 'DbConfigurationOptions.ConfigureTransactionAccessor<{dbTransaction.GetType().Name}, {typeof(TTransaction).Name}>() to setup how to access {typeof(TTransaction).Name}'");
    }
}