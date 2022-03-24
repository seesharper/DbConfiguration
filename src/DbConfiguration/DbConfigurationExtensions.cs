using System;
using System.Data;
using System.Data.Common;

namespace DbConfiguration
{
    public static class ConfigurationExtensions
    {
        public static IDbConnection Configure<TConnection>(this IDbConnection dbConnection, Action<TConnection> configureConnection) where TConnection : DbConnection
        {
            configureConnection(DbConfigurationOptions.GetInnerConnection<TConnection>(dbConnection));
            return dbConnection;
        }
    }

}