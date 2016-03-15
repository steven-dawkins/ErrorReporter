using ErrorReporter.Core;
using SharpRaven;
using SharpRaven.Data;
using System;

namespace ErrorReporter.Sentry
{
    public class SentryErrorReporter : IErrorReporter
    {
        private readonly RavenClient ravenClient;

        public SentryErrorReporter(string dsn)
        {
            this.ravenClient = new RavenClient(dsn);
        }

        public void Capture(Exception e)
        {
            this.ravenClient.Capture(new SentryEvent(e));
        }

        public void Capture(string message, Level level)
        {
            var r = new SentryEvent(new SentryMessage(message));
            switch (level)
            {
                case Level.Error:
                    r.Level = ErrorLevel.Error;
                    break;
                case Level.Info:
                    r.Level = ErrorLevel.Info;
                    break;
                default:
                    throw new Exception("Unexpected reporting level in SentryErrorReporter.Capture : " + level);
            }

            this.ravenClient.Capture(r);
        }
    }
}
