using System;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using DbActivities;
using FluentAssertions;
using Xunit;

namespace DbConfiguration.Tests;

public class DbConfigurationTests
{
    [Fact]
    public void ShouldConfigureConnection()
    {
        DbConfigurationOptions.Clear();
        var connection = GetSQLiteConnection();
        connection.Configure<SQLiteConnection>(connection => connection.Should().BeOfType<SQLiteConnection>());
    }

    [Fact]
    public void ShouldConfigureConnectionWithConnectionAccessor()
    {
        DbConfigurationOptions.Clear();
        DbConfigurationOptions.ConfigureConnectionAccessor<InstrumentedDbConnection, SQLiteConnection>((instrumentedDbConnection)
            => instrumentedDbConnection.InnerDbConnection as SQLiteConnection);

        var connection = GetInstrumentedConnection();
        connection.Configure<SQLiteConnection>(connection => connection.Should().BeOfType<SQLiteConnection>());
    }

    [Fact]
    public void ShouldThrowExceptionWhenConnectionAccessorIsMissing()
    {
        DbConfigurationOptions.Clear();
        var connection = GetInstrumentedConnection();
        Action action = () => connection.Configure<SQLiteConnection>(connection => { });
        action.Should().Throw<InvalidOperationException>().Where(e => e.Message.Contains("ConfigureConnectionAccessor"));
    }

    [Fact]
    public void ShouldConfigureCommand()
    {
        DbConfigurationOptions.Clear();
        var command = GetSQLiteConnection().CreateCommand();
        command.Configure<SQLiteCommand>(command => command.Should().BeOfType<SQLiteCommand>());
    }

    [Fact]
    public void ShouldConfigureCommandWithCommandAccessor()
    {
        DbConfigurationOptions.Clear();
        DbConfigurationOptions.ConfigureCommandAccessor<InstrumentedDbCommand, SQLiteCommand>(
            command => (SQLiteCommand)command.InnerDbCommand);

        var command = GetInstrumentedConnection().CreateCommand();
        command.Configure<SQLiteCommand>(command => command.Should().BeOfType<SQLiteCommand>());
    }

    [Fact]
    public void ShouldThrowExceptionWhenCommandAccessorIsMissing()
    {
        DbConfigurationOptions.Clear();
        var command = GetInstrumentedConnection().CreateCommand();
        Action action = () => command.Configure<SQLiteCommand>(command => { });
        action.Should().Throw<InvalidOperationException>().Where(e => e.Message.Contains("ConfigureCommandAccessor"));
    }

    [Fact]
    public void ShouldConfigureDataReader()
    {
        DbConfigurationOptions.Clear();
        var command = GetSQLiteConnection().CreateCommand();
        command.CommandText = "SELECT 1";
        var reader = command.ExecuteReader();
        reader.Configure<SQLiteDataReader>(command => command.Should().BeOfType<SQLiteDataReader>());
    }

    [Fact]
    public void ShouldConfigureDataReaderWithDataReaderAccessor()
    {
        DbConfigurationOptions.Clear();
        DbConfigurationOptions.ConfigureDataReaderAccessor<InstrumentedDbDataReader, SQLiteDataReader>(
            reader => (SQLiteDataReader)reader.InnerDbDataReader);
        var command = GetInstrumentedConnection().CreateCommand();
        command.CommandText = "SELECT 1";
        var reader = command.ExecuteReader();
        reader.Configure<SQLiteDataReader>(command => command.Should().BeOfType<SQLiteDataReader>());
    }

    [Fact]
    public void ShouldThrowExceptionWhenDataReaderAccessorIsMissing()
    {
        DbConfigurationOptions.Clear();
        var command = GetInstrumentedConnection().CreateCommand();
        command.CommandText = "SELECT 1";
        var reader = command.ExecuteReader();
        Action action = () => reader.Configure<SQLiteDataReader>(command => { });
        action.Should().Throw<InvalidOperationException>().Where(e => e.Message.Contains("ConfigureDataReaderAccessor"));
    }

    [Fact]
    public void ShouldConfigureTransaction()
    {
        DbConfigurationOptions.Clear();
        var transaction = GetSQLiteConnection().BeginTransaction();
        transaction.Configure<SQLiteTransaction>(command => command.Should().BeOfType<SQLiteTransaction>());
    }

    [Fact]
    public void ShouldConfigureTransactionWithTransactionAccessor()
    {
        DbConfigurationOptions.Clear();
        DbConfigurationOptions.ConfigureTransactionAccessor<InstrumentedDbTransaction, SQLiteTransaction>(transaction => (SQLiteTransaction)transaction.InnerDbTransaction);
        var transaction = GetInstrumentedConnection().BeginTransaction();
        transaction.Configure<SQLiteTransaction>(command => command.Should().BeOfType<SQLiteTransaction>());
    }

    [Fact]
    public void ShouldThrowExceptionWhenTransactionAccessorIsMissing()
    {
        DbConfigurationOptions.Clear();
        var transaction = GetInstrumentedConnection().BeginTransaction();
        Action action = () => transaction.Configure<SQLiteTransaction>(command => { });
        action.Should().Throw<InvalidOperationException>().Where(e => e.Message.Contains("ConfigureTransactionAccessor"));
    }

    private static IDbConnection GetInstrumentedConnection(InstrumentationOptions options = null)
    {
        return new InstrumentedDbConnection(GetSQLiteConnection(), options ?? new InstrumentationOptions("sqlite"));
    }

    private static DbConnection GetSQLiteConnection()
    {
        var connection = new SQLiteConnection("Data Source= :memory:; Cache = Shared");
        connection.Open();
        return connection;
    }

}