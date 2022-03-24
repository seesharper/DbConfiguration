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
        DbConfigurationOptions.ConfigureConnectionAccessor<InstrumentedDbConnection, SQLiteConnection>((instrumentedDbConnection)
            => instrumentedDbConnection.InnerDbConnection as SQLiteConnection);


        var connection = GetInstrumentedConnection();
        connection.Configure<SQLiteConnection>(connection => connection.Should().BeOfType<SQLiteConnection>());
    }




    private IDbConnection GetInstrumentedConnection(InstrumentationOptions options = null)
    {
        return new InstrumentedDbConnection(GetSQLiteConnection(), options ?? new InstrumentationOptions("sqlite"));
    }

    private DbConnection GetSQLiteConnection()
    {
        var connection = new SQLiteConnection("Data Source= :memory:; Cache = Shared");
        connection.Open();
        return connection;
    }

}