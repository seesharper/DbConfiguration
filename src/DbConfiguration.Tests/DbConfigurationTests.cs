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
        connection.ConfigureConnection<SQLiteConnection>(connection => connection.Should().BeOfType<SQLiteConnection>());
    }

    [Fact]
    public void ShouldConfigureConnectionWithConnectionAccessor()
    {
        DbConfigurationOptions.Clear();
        DbConfigurationOptions.ConfigureConnectionAccessor<InstrumentedDbConnection, SQLiteConnection>((instrumentedDbConnection)
            => instrumentedDbConnection.InnerDbConnection as SQLiteConnection);

        var connection = GetInstrumentedConnection();
        connection.ConfigureConnection<SQLiteConnection>(connection => connection.Should().BeOfType<SQLiteConnection>());
    }

    [Fact]
    public void ShouldThrowExceptionWhenConnectionAccessorIsMissing()
    {
        DbConfigurationOptions.Clear();
        var connection = GetInstrumentedConnection();
        Action action = () => connection.ConfigureConnection<SQLiteConnection>(connection => { });
        action.Should().Throw<InvalidOperationException>().Where(e => e.Message.Contains("ConfigureConnectionAccessor"));
    }

    [Fact]
    public void ShouldConfigureCommand()
    {
        DbConfigurationOptions.Clear();
        var command = GetSQLiteConnection().CreateCommand();
        command.ConfigureCommand<SQLiteCommand>(command => command.Should().BeOfType<SQLiteCommand>());
    }

    [Fact]
    public void ShouldConfigureCommandWithCommandAccessor()
    {
        DbConfigurationOptions.Clear();
        DbConfigurationOptions.ConfigureCommandAccessor<InstrumentedDbCommand, SQLiteCommand>(
            command => (SQLiteCommand)command.InnerDbCommand);

        var command = GetInstrumentedConnection().CreateCommand();
        command.ConfigureCommand<SQLiteCommand>(command => command.Should().BeOfType<SQLiteCommand>());
    }

    [Fact]
    public void ShouldThrowExceptionWhenCommandAccessorIsMissing()
    {
        DbConfigurationOptions.Clear();
        var command = GetInstrumentedConnection().CreateCommand();
        Action action = () => command.ConfigureCommand<SQLiteCommand>(command => { });
        action.Should().Throw<InvalidOperationException>().Where(e => e.Message.Contains("ConfigureCommandAccessor"));
    }

    [Fact]
    public void ShouldConfigureDataReader()
    {
        DbConfigurationOptions.Clear();
        var command = GetSQLiteConnection().CreateCommand();
        command.CommandText = "SELECT 1";
        var reader = command.ExecuteReader();
        reader.ConfigureDataReader<SQLiteDataReader>(command => command.Should().BeOfType<SQLiteDataReader>());
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
        reader.ConfigureDataReader<SQLiteDataReader>(command => command.Should().BeOfType<SQLiteDataReader>());
    }

    [Fact]
    public void ShouldThrowExceptionWhenDataReaderAccessorIsMissing()
    {
        DbConfigurationOptions.Clear();
        var command = GetInstrumentedConnection().CreateCommand();
        command.CommandText = "SELECT 1";
        var reader = command.ExecuteReader();
        Action action = () => reader.ConfigureDataReader<SQLiteDataReader>(command => {});
        action.Should().Throw<InvalidOperationException>().Where(e => e.Message.Contains("ConfigureDataReaderAccessor"));
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