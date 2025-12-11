using Agenda_Cons.Migrations;
using Agenda_Models;
using Azure.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace Agenda_Web.Services
{
    public class DbLoggerOptions
    {

        public DbLoggerOptions()
        {
        }
    }

    [ProviderAlias("Database")]
    public class DbLoggerProvider : ILoggerProvider
    {
        public readonly DbLoggerOptions Options;
        private readonly AgendaDbContext _context;

        public DbLoggerProvider(IOptions<DbLoggerOptions> _options)
        {
            Options = _options.Value; // Stores all the options.
        }

        /// <summary>
        /// Creates a new instance of the db logger.
        /// </summary>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        public ILogger CreateLogger(string categoryName)
        {
            return new DbLogger(this);
        }

        public void Dispose()
        {
        }
    }


    public class DbLogger : ILogger
    {
        private readonly DbLoggerProvider _dbLoggerProvider;   // Instance of <see cref="DbLoggerProvider"
        private readonly AgendaDbContext _context;

        static public LogLevel DefaultLogLevel = LogLevel.Warning; // Default to Warning and higher

        // Constructor
        public DbLogger([NotNull] DbLoggerProvider dbLoggerProvider)
        {
            _dbLoggerProvider = dbLoggerProvider;
            _context = new AgendaDbContext();
        }


        // Aanwezig voor interface, maar niet gebruikt
        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }


        // Moet er gelogd worden voor dit logniveau?
        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel >= DefaultLogLevel && logLevel != LogLevel.None;
        }


        // De eigenlijke logica om te loggen naar de database
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception, string> formatter)
        {
            if (IsEnabled(logLevel))
            {
                LogError error = new LogError {
                    Application = "Agenda_Web",
                    DeviceName = Globals.App.Environment.EnvironmentName,
                    ThreadId = Environment.CurrentManagedThreadId, // Get the current thread ID to use in the log file. 
                    LogLevel = logLevel.ToString(),
                    EventId = eventId == null ? 0 : eventId.Id,
                    EventName = eventId == null ? "-" : eventId.Name == null || eventId.Name == "-" ? "-" : eventId.Name,
                    Message = formatter(state, exception),
                    ExceptionMessage = exception == null ? "-" : exception.Message,
                    StackTrace = exception == null ? "-" : exception.StackTrace,
                    Source = exception == null ? "-" : exception.Source,
                    TimeStamp = DateTime.UtcNow
                };
                _context.LogErrors.Add(error);
                _context.SaveChanges();

            }
        }
    }


    // Voegt de DbLogger toe aan de logging builder; kan gebruikt worden in Program.cs
    public static class DbLoggerExtensions
    {
        public static ILoggingBuilder AddDbLogger(this ILoggingBuilder builder, Action<DbLoggerOptions> configure)
        {
            builder.Services.AddSingleton<ILoggerProvider, DbLoggerProvider>();
            builder.Services.Configure(configure);
            return builder;
        }
    }

}
