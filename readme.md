# DbConfiguration

Sometimes when working with `IdbConnection`, `IDbCommand` and friends we need to drop down to the actual underlying class to gain access to provider-specific properties and methods. For instance, Oracle has a property `OracleDbCommand.BindByName` which  is commonly used to specify that command arguments are to be specified by name.  Since `BindByName` is provider-specific and not part of the `IDbCommand` interface, we need a way to easily set this property.

One possible solution could be to cast the abstraction(`IDbCommand`) to what we think is the underlying type.

```c#
((OracleConnection)dbConnection).ClientId = "SomeClientId";
```

The problem with this approach is that the code becomes a bit cumbersome and we also make the assumption that we can cast `dbCommand` to an `OracleCommand`.  One of the reasons that making assumption about the actual concrete type is dangerous is that it might change. For instance, we might choose to add some kind of profiling on top of the provider-specific types. [DbActivities](https://github.com/seesharper/DbActivities) is an example of such a profiling-library that decorates (wraps around) `DbConnection`, `DbCommand`, `DbTransaction` and `DbDataReader` to provide profiling capabilities. 

An example would be like this:

```c#
IDbConnection dbConnection = new InstrumentedDbConnection(new OracleConnection());
```

The decorator-chain would now be: `IDbConnection -> InstrumentedDbConnection -> OracleConnection` and we can no longer cast `IDbConnection` directly to `OracleCommand` which we need to be able to set the `ClientId` property.

## Setup 

**DbConfiguration** solves this by letting us specify a function to convert the actual connection which in this case is an `InstrumentedDbConnection` to the type we want to configure (`OracleConnection`).

```c#
DbConfigurationOptions.ConfigureConnectionAccessor<InstrumentedDbConnection, OracleConnection>((connection)
            => connection.InnerDbConnection as OracleConnection);
```

> **DbActivites** provides `InnerDbxxx` properties for easy access to the decorated instance.

We might argue that this is still a cast to the underlying connection, but at least we have isolated the cast to a single place in our code and we could easily modify the casting based on whether we have profiling enabled or not. 

A complete setup for the case with **Oracle** and **DbActivities** would be 

```c#
DbConfigurationOptions.ConfigureConnectionAccessor<InstrumentedDbConnection, OracleConnection>((connection)
            => connection.InnerDbConnection as OracleConnection);
DbConfigurationOptions.ConfigureCommandAccessor<InstrumentedDbCommand, OracleCommand>((command)
            => command.InnerDbCommand as OracleCommand);            
DbConfigurationOptions.ConfigureDataReaderAccessor<InstrumentedDbDataReader, OracleDataReader>((reader)
            => reader.InnerDbDataReader as OracleDataReader);            
DbConfigurationOptions.ConfigureTransactionAccessor<InstrumentedDbTransaction, OracleTransaction>((transaction)
            => reader.InnerTransaction as OracleTransaction);            
```

So now we can at any time "configure" the underlying provider-specific instance by using the `Configure<TConnection>` method.

```c#
IDbConnection dbConnection = GetConnectionFromSomeWhere();
dbConnection.Configure<OracleConnection>(c => c.ClientId = "SomeClientId");
```

This also goes for `IDbCommand`, `IDbDataReader` and `IDbTransaction`

For instance setting the `BindByName` property for the underlying `OracleCommand` is a simple as:

```c#
dbCommand.Configure<OracleCommand>(c => c.BindByName = true);
```





