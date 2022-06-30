using Microsoft.Extensions.Logging;

namespace WorkloadCalculator
{
    public class ConsoleLogger<TState> : ILogger
    {
        public void Log<TState>(TState state, Func<TState, Exception?, string> formatter)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(formatter(state, null));
            Console.ResetColor();
        }

        public void Error<TState>(TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(formatter(state, exception));
            Console.ResetColor();
        }

        public void Warn<TState>(TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(formatter(state, exception));
            Console.ResetColor();
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            switch (logLevel)
            {
                case LogLevel.Debug:
                case LogLevel.Information:
                case LogLevel.Trace:
                    Log(state, formatter);
                    break;

                case LogLevel.Error:
                    Error(state, exception, formatter);
                    break;

                case LogLevel.Warning:
                    Warn(state, exception, formatter);
                    break;
            }
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            throw new NotImplementedException();
        }
    }
}
