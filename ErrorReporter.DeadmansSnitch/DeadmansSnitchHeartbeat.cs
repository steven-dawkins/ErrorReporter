using System;
using System.Net.Http;
using System.Reactive.Subjects;
using System.Reactive.Linq;
using ErrorReporter.Core;

namespace ErrorReporter.DeadmansSnitch
{
    public class DeadmansSnitchHeartbeat : IHeartbeat
    {
        private readonly string url;
        private readonly Subject<bool> beatSubject;
        private readonly IErrorReporter errorReporter;

        public static IHeartbeat Connect(string url, IErrorReporter errorReporter)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return new NopHeartbeat();
            }

            return new DeadmansSnitchHeartbeat(url, errorReporter);
        }

        public DeadmansSnitchHeartbeat(string url, IErrorReporter errorReporter)
        {
            this.url = url;
            this.errorReporter = errorReporter;
            this.beatSubject = new Subject<bool>();
            this.beatSubject
                .Sample(TimeSpan.FromMinutes(1))
                .Subscribe(evt => OnThrottledBeat(evt));
        }

        public void OnThrottledBeat(bool @event)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var message = string.Format("Machine: {0}", Environment.MachineName);
                    var responseString = client.GetStringAsync(url+"?m=" + message).Result;
                }
            }
            catch (Exception e)
            {
                this.errorReporter.Capture(e);
            }
        }

        public void Beat()
        {
            beatSubject.OnNext(true);
        }
    }
}
