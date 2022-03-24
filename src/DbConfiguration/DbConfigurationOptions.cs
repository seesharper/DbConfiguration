using System;
using System.Data;

public class DbConfigurationOptions
{
    private static Func<IDbConnection, IDbConnection> _configureConnectionAccessor;
    private static Func<IDbCommand, IDbCommand> _configureCommandAccessor;
    private static Func<IDataReader, IDataReader> _configureDataReaderAccessor;
    private static Func<IDbTransaction, IDbTransaction> _configureTransactionAccessor;

    public static void Clear()
    {
        _configureConnectionAccessor = (connection) => connection;
        _configureCommandAccessor = (command) => command;
        _configureDataReaderAccessor = (dataReader) => dataReader;
        _configureTransactionAccessor = (transaction) => transaction;
    }

    static DbConfigurationOptions() => Clear();

    public static void ConfigureConnectionAccessor<TConnection, TInnerConnection>(Func<TConnection, TInnerConnection> configureAccessor) where TConnection : IDbConnection where TInnerConnection : IDbConnection
        => _configureConnectionAccessor = (connection) => configureAccessor((TConnection)connection);

    public static void ConfigureCommandAccessor<TCommand, TInnerCommand>(Func<TCommand, TInnerCommand> configureAccessor) where TCommand : IDbCommand where TInnerCommand : IDbCommand
        => _configureCommandAccessor = (command) => configureAccessor((TCommand)command);

    public static void ConfigureDataReaderAccessor<TDataReader, TInnerDataReader>(Func<TDataReader, TInnerDataReader> configureAccessor) where TDataReader : IDataReader where TInnerDataReader : IDataReader
        => _configureDataReaderAccessor = (dataReader) => configureAccessor((TDataReader)dataReader);

    public static void ConfigureTransactionAccessor<TDbTransaction, TInnerDbTransaction>(Func<TDbTransaction, TInnerDbTransaction> configureAccessor) where TDbTransaction : IDbTransaction where TInnerDbTransaction : IDbTransaction
        => _configureTransactionAccessor = (transaction) => configureAccessor((TDbTransaction)transaction);

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

    internal static TDbTransaction GetInnerDbTransaction<TDbTransaction>(IDbTransaction dbTransaction) where TDbTransaction : IDbTransaction
    {
        var innerDataReader = _configureTransactionAccessor(dbTransaction);
        if (innerDataReader is TDbTransaction reader)
        {
            return reader;
        }
        throw new InvalidOperationException("Something useful here");
    }
}