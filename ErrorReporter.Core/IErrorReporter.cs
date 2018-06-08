using System;
using System.Collections.Generic;

namespace ErrorReporter.Core
{
    public enum Level { Error, Info, Debug }

    public interface IErrorReporter
    {
        void Capture(Exception e, IEnumerable<KeyValuePair<String, object>> extraInformation);

        void Capture(string message, Level level, IEnumerable<KeyValuePair<String, object>> extraInformation);
    }
}
