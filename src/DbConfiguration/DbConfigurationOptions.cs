using System.Data;
using System.Data.Common;

public class DbConfigurationOptions
{
    private static Func<IDbConnection, IDbConnection> _configureAccessor = (c) => c;

    public static void ConfigureInnerConnectionAccessor<TConnection, TInnerConnection>(Func<TConnection, TInnerConnection> configureAccessor) where TConnection : DbConnection where TInnerConnection : DbConnection
        => _configureAccessor = (c) => configureAccessor(c as TConnection);

    internal static TConnection GetInnerConnection<TConnection>(IDbConnection dbConnection) where TConnection : IDbConnection
    {
        var innerConnection = _configureAccessor(dbConnection);
        if (innerConnection is TConnection connection)
        {
            return connection;
        }
        throw new InvalidOperationException("Something useful here");
    }
}