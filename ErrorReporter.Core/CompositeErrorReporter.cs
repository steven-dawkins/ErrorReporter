using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErrorReporter.Core
{
    public class CompositeErrorReporter : IErrorReporter
    {
        private readonly IErrorReporter[] errorReporters;

        public CompositeErrorReporter(IEnumerable<IErrorReporter> errorReporters)
            : this(errorReporters.ToArray())
        {
        }

        public CompositeErrorReporter(params IErrorReporter[] errorReporters)
        {
            this.errorReporters = errorReporters;
        }


        public void Capture(Exception e)
        {
            foreach(var reporter in this.errorReporters)
            {
                reporter.Capture(e);
            }
        }

        public void Capture(Exception e, IEnumerable<KeyValuePair<string, object>> extraInformation)
        {
            foreach (var reporter in this.errorReporters)
            {
                reporter.Capture(e, extraInformation);
            }
        }

        public void Capture(string message, Level level)
        {
            foreach (var reporter in this.errorReporters)
            {
                reporter.Capture(message, level);
            }
        }

        public void Capture(string message, Level level, IEnumerable<KeyValuePair<string, object>> extraInformation)
        {
            foreach (var reporter in this.errorReporters)
            {
                reporter.Capture(message, level, extraInformation);
            }
        }
    }
}
