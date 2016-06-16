using System;
using ErrorReporter.Core;
using SharpRaven;
using SharpRaven.Data;


namespace ErrorReporter.Sentry
{
    public class SentryErrorReporter : IErrorReporter
    {
        private readonly RavenClient ravenClient;

        public SentryErrorReporter(string dsn, string release)
        {
            this.ravenClient = new RavenClient(dsn);

            if (!string.IsNullOrWhiteSpace(release))
            {
                this.ravenClient.Release = release;
            }
        }

        public static IErrorReporter Connect(string dsn, string release)
        {
            if (string.IsNullOrWhiteSpace(dsn))
            {
                return new NopErrorReporter();
            }

            return new SentryErrorReporter(dsn, release);
        }        

        public void Capture(Exception e)
        {
            var r = new SentryEvent(e);

            r.Message = e.Message;

            this.ravenClient.Capture(r);
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
