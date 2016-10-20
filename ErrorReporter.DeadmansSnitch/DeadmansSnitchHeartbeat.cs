using System;
using System.Net.Http;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using ErrorReporter.Core;
using System.Threading.Tasks;

namespace ErrorReporter.DeadmansSnitch
{
    public class DeadmansSnitchHeartbeat : IHeartbeat
    {
        private readonly string url;
        private readonly Subject<bool> beatSubject;
        private readonly IErrorReporter errorReporter;        

        public DeadmansSnitchHeartbeat(string url, IErrorReporter errorReporter)
        {
            this.url = url;
            this.errorReporter = errorReporter;
            this.beatSubject = new Subject<bool>();
            this.beatSubject
                .Window(()=> Observable.Interval(TimeSpan.FromMinutes(1)))
                .SelectMany(x => x.Take(1))
                .Subscribe(evt => this.OnThrottledBeat(evt));
        }

        public static IHeartbeat Connect(string url, IErrorReporter errorReporter)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return new NopHeartbeat();
            }

            return new DeadmansSnitchHeartbeat(url, errorReporter);
        }

        public void OnThrottledBeat(bool @event)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var message = string.Format("Machine: {0}", Environment.MachineName);
                    var responseString = client.GetStringAsync(this.url + "?m=" + message).Result;
                }
            }
            catch (Exception e)
            {
                this.errorReporter.Capture(e);
            }
        }

        public void Beat()
        {
            Task.Run(() => this.beatSubject.OnNext(true));
        }
    }
}
