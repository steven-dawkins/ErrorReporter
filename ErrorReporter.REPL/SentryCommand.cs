using Replify;
using ErrorReporter.Sentry;
using ErrorReporter.Core;

namespace ErrorReporter.REPL
{
    public class SentryCommand : IReplCommand
    {
        private readonly SentryErrorReporter sentry;

        public SentryCommand()
        {
            var url = "https://8b222a8aaba8417db60255cbcdc6381b:91af66fba0f94655a930e8b9acade6e2@sentry.io/108735";

            this.sentry = new SentryErrorReporter(url);
        }

        public void Info(string message)
        {
            this.sentry.Capture(message, Level.Info);
        }
    }
}
