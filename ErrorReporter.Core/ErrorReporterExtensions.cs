using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErrorReporter.Core
{
    public static class ErrorReporterExtensions
    {
        public static void Capture(this IErrorReporter reporter, Exception e)
        {
            reporter.Capture(e, new KeyValuePair<string, object>[] { });
        }

        public static void Capture(this IErrorReporter reporter, string message, Level level)
        {
            reporter.Capture(message, level, new KeyValuePair <string, object>[] { });
        }
    }
}
