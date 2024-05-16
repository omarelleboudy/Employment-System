using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Core;
using Serilog.Sinks.MariaDB;
using Serilog.Sinks.MariaDB.Extensions;
using Serilog.Sinks.MSSqlServer;
using System.Collections.ObjectModel;
using System.Data;

namespace Infrastructure.Logging.Core
{

    public class InfrastructureLogger
    {
        public static Logger CreateSerilogLogger(IConfigurationRoot configuration)
        {
           var colOptions = GetColumnOptions();

            Logger logger = SetLoggerConfigurations(configuration, colOptions);
            return logger;
        }

        private static Logger SetLoggerConfigurations(IConfigurationRoot configuration, ColumnOptions colOptions)
        {
            var connection = configuration["Serilog:LoggingDBConnection"];
            var tableName = configuration["Serilog:TableName"];
            var schema = configuration["Serilog:DatabaseSchema"];

            return new LoggerConfiguration()
            .WriteTo.MSSqlServer(connectionString: connection,
            sinkOptions: new MSSqlServerSinkOptions
            {
                
                TableName = tableName,
                SchemaName = schema,
                AutoCreateSqlTable = true,                
            },
            appConfiguration: configuration,
            columnOptions: colOptions)
            .CreateLogger();
        }

        private static ColumnOptions GetColumnOptions()
        {
            var colOptions = new ColumnOptions();
            colOptions.Store.Remove(StandardColumn.Properties);
            colOptions.Store.Remove(StandardColumn.MessageTemplate);
            colOptions.Store.Remove(StandardColumn.LogEvent);
            colOptions.Store.Remove(StandardColumn.Exception);
            colOptions.AdditionalColumns = new Collection<SqlColumn>
            {
            new SqlColumn
                {
                    ColumnName = "CorrelationId",
                    PropertyName = "CorrelationId",
                    DataType = SqlDbType.NVarChar,
                    DataLength = 128
                }};
            return colOptions;
        }
    }
}