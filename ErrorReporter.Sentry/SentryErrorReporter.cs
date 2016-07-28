using System;
using ErrorReporter.Core;
using SharpRaven;
using SharpRaven.Data;
using System.Collections.Generic;
using System.Linq;

namespace ErrorReporter.Sentry
{
    public class SentryErrorReporter : IErrorReporter
    {
        private readonly RavenClient ravenClient;
        private readonly IEnumerable<KeyValuePair<string, object>> extraInformation;

        public SentryErrorReporter(string dsn, string release, IEnumerable<KeyValuePair<String, object>> extraInformation)
        {
            this.extraInformation = extraInformation ?? new KeyValuePair<string, object>[] { };
            this.ravenClient = new RavenClient(dsn);

            if (!string.IsNullOrWhiteSpace(release))
            {
                this.ravenClient.Release = release;
            }
        }

        public static IErrorReporter Connect(string dsn, string release, IEnumerable<KeyValuePair<String, object>> extraInformation = null)
        {
            if (string.IsNullOrWhiteSpace(dsn))
            {
                return new NopErrorReporter();
            }

            return new SentryErrorReporter(dsn, release, extraInformation);
        }        

        public void Capture(Exception e, IEnumerable<KeyValuePair<String, object>> extraInformation = null)
        {
            extraInformation = extraInformation ?? new KeyValuePair<string, object>[] { };
            var r = new SentryEvent(e);

            r.Message = e.Message;
            r.Extra = this.extraInformation.Union(extraInformation).ToDictionary(k => k.Key, k => k.Value);

            this.ravenClient.Capture(r);
        }

        public void Capture(string message, Level level, IEnumerable<KeyValuePair<String, object>> extraInformation)
        {
            var l = GetLevel(level);

            var m = new SentryMessage(message);
            var extra = this.extraInformation.Union(extraInformation).ToDictionary(k => k.Key, k => k.Value);

            this.ravenClient.CaptureMessage(m, l, extra: extra);
        }

        private static ErrorLevel GetLevel(Level level)
        {            
            switch (level)
            {
                case Level.Error: return ErrorLevel.Error;                    
                case Level.Info: return ErrorLevel.Info;                    
                default: throw new Exception("Unexpected reporting level in SentryErrorReporter.Capture : " + level);
            }
        }

        public void Capture(Exception e)
        {
            this.Capture(e, null);
        }

        public void Capture(string message, Level level)
        {
            this.Capture(message, level, null);
        }
    }
}
