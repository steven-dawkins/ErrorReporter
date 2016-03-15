using System;

namespace ErrorReporter.Core
{
    public enum Level { Error, Info }
    public interface IErrorReporter
    {
        void Capture(Exception e);
        void Capture(string message, Level level);
    }
}
