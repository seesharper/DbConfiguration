using System;
using System.Data;
using System.Data.Common;
using Microsoft.Data.Sqlite;
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
        connection.Configure<SqliteConnection>(connection => connection.Should().BeOfType<SqliteConnection>());
    }

    [Fact]
    public void ShouldConfigureConnectionWithConnectionAccessor()
    {
        DbConfigurationOptions.Clear();
        DbConfigurationOptions.ConfigureConnectionAccessor<InstrumentedDbConnection, SqliteConnection>((instrumentedDbConnection)
            => instrumentedDbConnection.InnerDbConnection as SqliteConnection);

        var connection = GetInstrumentedConnection();
        connection.Configure<SqliteConnection>(connection => connection.Should().BeOfType<SqliteConnection>());
    }

    [Fact]
    public void ShouldThrowExceptionWhenConnectionAccessorIsMissing()
    {
        DbConfigurationOptions.Clear();
        var connection = GetInstrumentedConnection();
        Action action = () => connection.Configure<SqliteConnection>(connection => { });
        action.Should().Throw<InvalidOperationException>().Where(e => e.Message.Contains("ConfigureConnectionAccessor"));
    }

    [Fact]
    public void ShouldConfigureCommand()
    {
        DbConfigurationOptions.Clear();
        var command = GetSQLiteConnection().CreateCommand();
        command.Configure<SqliteCommand>(command => command.Should().BeOfType<SqliteCommand>());
    }

    [Fact]
    public void ShouldConfigureCommandWithCommandAccessor()
    {
        DbConfigurationOptions.Clear();
        DbConfigurationOptions.ConfigureCommandAccessor<InstrumentedDbCommand, SqliteCommand>(
            command => (SqliteCommand)command.InnerDbCommand);

        var command = GetInstrumentedConnection().CreateCommand();
        command.Configure<SqliteCommand>(command => command.Should().BeOfType<SqliteCommand>());
    }

    [Fact]
    public void ShouldThrowExceptionWhenCommandAccessorIsMissing()
    {
        DbConfigurationOptions.Clear();
        var command = GetInstrumentedConnection().CreateCommand();
        Action action = () => command.Configure<SqliteCommand>(command => { });
        action.Should().Throw<InvalidOperationException>().Where(e => e.Message.Contains("ConfigureCommandAccessor"));
    }

    [Fact]
    public void ShouldConfigureDataReader()
    {
        DbConfigurationOptions.Clear();
        var command = GetSQLiteConnection().CreateCommand();
        command.CommandText = "SELECT 1";
        var reader = command.ExecuteReader();
        reader.Configure<SqliteDataReader>(command => command.Should().BeOfType<SqliteDataReader>());
    }

    [Fact]
    public void ShouldConfigureDataReaderWithDataReaderAccessor()
    {
        DbConfigurationOptions.Clear();
        DbConfigurationOptions.ConfigureDataReaderAccessor<InstrumentedDbDataReader, SqliteDataReader>(
            reader => (SqliteDataReader)reader.InnerDbDataReader);
        var command = GetInstrumentedConnection().CreateCommand();
        command.CommandText = "SELECT 1";
        var reader = command.ExecuteReader();
        reader.Configure<SqliteDataReader>(command => command.Should().BeOfType<SqliteDataReader>());
    }

    [Fact]
    public void ShouldThrowExceptionWhenDataReaderAccessorIsMissing()
    {
        DbConfigurationOptions.Clear();
        var command = GetInstrumentedConnection().CreateCommand();
        command.CommandText = "SELECT 1";
        var reader = command.ExecuteReader();
        Action action = () => reader.Configure<SqliteDataReader>(command => { });
        action.Should().Throw<InvalidOperationException>().Where(e => e.Message.Contains("ConfigureDataReaderAccessor"));
    }

    [Fact]
    public void ShouldConfigureTransaction()
    {
        DbConfigurationOptions.Clear();
        var transaction = GetSQLiteConnection().BeginTransaction();
        transaction.Configure<SqliteTransaction>(command => command.Should().BeOfType<SqliteTransaction>());
    }

    [Fact]
    public void ShouldConfigureTransactionWithTransactionAccessor()
    {
        DbConfigurationOptions.Clear();
        DbConfigurationOptions.ConfigureTransactionAccessor<InstrumentedDbTransaction, SqliteTransaction>(transaction => (SqliteTransaction)transaction.InnerDbTransaction);
        var transaction = GetInstrumentedConnection().BeginTransaction();
        transaction.Configure<SqliteTransaction>(command => command.Should().BeOfType<SqliteTransaction>());
    }

    [Fact]
    public void ShouldThrowExceptionWhenTransactionAccessorIsMissing()
    {
        DbConfigurationOptions.Clear();
        var transaction = GetInstrumentedConnection().BeginTransaction();
        Action action = () => transaction.Configure<SqliteTransaction>(command => { });
        action.Should().Throw<InvalidOperationException>().Where(e => e.Message.Contains("ConfigureTransactionAccessor"));
    }

    [Fact]
    public void ShouldGetInnerConnection()
    {
        DbConfigurationOptions.Clear();
        DbConfigurationOptions.ConfigureConnectionAccessor<InstrumentedDbConnection, SqliteConnection>(connection => (SqliteConnection)connection.InnerDbConnection);
        var connection = GetInstrumentedConnection();
        connection.GetInner<SqliteConnection>().Should().BeOfType<SqliteConnection>();
    }

    [Fact]
    public void ShouldGetInnerTransaction()
    {
        DbConfigurationOptions.Clear();
        DbConfigurationOptions.ConfigureTransactionAccessor<InstrumentedDbTransaction, SqliteTransaction>(transaction => (SqliteTransaction)transaction.InnerDbTransaction);
        using var transaction = GetInstrumentedConnection().BeginTransaction();
        transaction.GetInner<SqliteTransaction>().Should().BeOfType<SqliteTransaction>();
    }

    [Fact]
    public void ShouldGetInnerCommand()
    {
        DbConfigurationOptions.Clear();
        DbConfigurationOptions.ConfigureCommandAccessor<InstrumentedDbCommand, SqliteCommand>(command => (SqliteCommand)command.InnerDbCommand);
        var command = GetInstrumentedConnection().CreateCommand();
        command.GetInner<SqliteCommand>().Should().BeOfType<SqliteCommand>();
    }

    [Fact]
    public void ShouldGetInnerDataReader()
    {
        DbConfigurationOptions.Clear();
        DbConfigurationOptions.ConfigureDataReaderAccessor<InstrumentedDbDataReader, SqliteDataReader>(reader => (SqliteDataReader)reader.InnerDbDataReader);
        var command = GetInstrumentedConnection().CreateCommand();
        command.CommandText = "SELECT 1";
        var reader = command.ExecuteReader();
        reader.GetInner<SqliteDataReader>().Should().BeOfType<SqliteDataReader>();
    }

    private static IDbConnection GetInstrumentedConnection(InstrumentationOptions options = null)
    {
        return new InstrumentedDbConnection(GetSQLiteConnection(), options ?? new InstrumentationOptions("sqlite"));
    }

    private static DbConnection GetSQLiteConnection()
    {
        var connection = new SqliteConnection("Data Source= :memory:; Cache = Shared");
        connection.Open();
        return connection;
    }

}