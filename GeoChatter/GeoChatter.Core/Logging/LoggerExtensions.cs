using log4net;
using log4net.Repository;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Xml;

namespace GeoChatter.Core.Logging
{
    /// <summary>
    /// Custom logger
    /// </summary>
    public class Log4NetLogger : ILogger
    {
        private readonly string _name;
        private readonly XmlElement _xmlElement;
        private readonly ILog _log;
        private ILoggerRepository _loggerRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="xmlElement"></param>
        public Log4NetLogger(string name, XmlElement xmlElement)
        {
            _name = name;
            _xmlElement = xmlElement;
            _loggerRepository = LogManager.CreateRepository(
                Assembly.GetEntryAssembly(), typeof(log4net.Repository.Hierarchy.Hierarchy));
            _log = LogManager.GetLogger(_loggerRepository.Name, name);
            log4net.Config.XmlConfigurator.Configure(_loggerRepository, xmlElement);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <param name="state"></param>
        /// <returns></returns>
        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        /// <summary>
        /// Check if given level is enabled
        /// </summary>
        /// <param name="logLevel"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public bool IsEnabled(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Critical:
                    return _log.IsFatalEnabled;
                case LogLevel.Debug:
                case LogLevel.Trace:
                    return _log.IsDebugEnabled;
                case LogLevel.Error:
                    return _log.IsErrorEnabled;
                case LogLevel.Information:
                    return _log.IsInfoEnabled;
                case LogLevel.Warning:
                    return _log.IsWarnEnabled;
                default:
                    throw new ArgumentOutOfRangeException(nameof(logLevel));
            }
        }

        /// <summary>
        /// Log as given <typeparamref name="TState"/>
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <param name="logLevel"></param>
        /// <param name="eventId"></param>
        /// <param name="state"></param>
        /// <param name="exception"></param>
        /// <param name="formatter"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state,
            Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            if (formatter == null)
            {
                throw new ArgumentNullException(nameof(formatter));
            }

            string message = formatter(state, exception);

            if (!string.IsNullOrEmpty(message) || exception != null)
            {
                switch (logLevel)
                {
                    case LogLevel.Critical:
                        _log.Fatal(message);
                        break;
                    case LogLevel.Debug:
                    case LogLevel.Trace:
                        _log.Debug(message);
                        break;
                    case LogLevel.Error:
                        _log.Error(message);
                        break;
                    case LogLevel.Information:
                        _log.Info(message);
                        break;
                    case LogLevel.Warning:
                        _log.Warn(message);
                        break;
                    default:
                        _log.Warn($"Encountered unknown log level {logLevel}, writing out as Info.");
                        _log.Info(message, exception);
                        break;
                }
            }
        }
    }

    /// <summary>
    /// Logger provider
    /// </summary>
    public sealed class Log4NetProvider : ILoggerProvider
    {
        private readonly string _log4NetConfigFile;
        private readonly ConcurrentDictionary<string, Log4NetLogger> _loggers = new();
        private bool disposedValue;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="log4NetConfigFile"></param>
        public Log4NetProvider(string log4NetConfigFile)
        {
            _log4NetConfigFile = log4NetConfigFile;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        public ILogger CreateLogger(string categoryName)
        {
            return _loggers.GetOrAdd(categoryName, CreateLoggerImplementation);
        }

        private Log4NetLogger CreateLoggerImplementation(string name)
        {
            return new Log4NetLogger(name, Parselog4NetConfigFile(_log4NetConfigFile));
        }

        private static XmlElement Parselog4NetConfigFile(string filename)
        {
            XmlDocument log4netConfig = new();
            using FileStream fs = File.OpenRead(filename);
            log4netConfig.Load(fs);
            return log4netConfig["log4net"];
        }

        private void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _loggers?.Clear();
                }

                disposedValue = true;
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
    /// <summary>
    /// Extensions
    /// </summary>
    public static class Log4netExtensions
    {
        /// <summary>
        /// Add provider
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="log4NetConfigFile"></param>
        /// <returns></returns>
        public static ILoggerFactory AddLog4Net([NotNull] this ILoggerFactory factory, string log4NetConfigFile)
        {
            using (Log4NetProvider provider = new(log4NetConfigFile))
            {
                factory.AddProvider(provider);
            }
            return factory;
        }
        /// <summary>
        /// See <see cref="AddLog4Net(ILoggerFactory, string)"/>
        /// </summary>
        /// <param name="factory"></param>
        /// <returns></returns>
        public static ILoggerFactory AddLog4Net(this ILoggerFactory factory)
        {
            return AddLog4Net(factory, "log4net.xml");
        }

        /// <summary>
        /// <see cref="AddLog4Net(ILoggerFactory, string)"/>
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="log4NetConfigFile"></param>
        /// <returns></returns>
        public static ILoggingBuilder AddLog4Net(this ILoggingBuilder factory, string log4NetConfigFile)
        {
            using (Log4NetProvider provider = new(log4NetConfigFile))
            {
                factory.AddProvider(provider);
            }
            return factory;
        }

        /// <summary>
        /// See <see cref="AddLog4Net(ILoggerFactory, string)"/>
        /// </summary>
        /// <param name="factory"></param>
        /// <returns></returns>
        public static ILoggingBuilder AddLog4Net(this ILoggingBuilder factory)
        {
            return AddLog4Net(factory, "log4net.xml");
        }
    }
}
