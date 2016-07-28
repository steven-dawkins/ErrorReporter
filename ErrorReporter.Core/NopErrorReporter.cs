using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErrorReporter.Core
{
    public class NopErrorReporter : IErrorReporter
    {
        public void Capture(Exception e)
        {            
        }

        public void Capture(string message, Level level)
        {            
        }

        public void Capture(Exception e, IEnumerable<KeyValuePair<string, object>> extraInformation)
        {            
        }
        
        public void Capture(string message, Level level, IEnumerable<KeyValuePair<string, object>> extraInformation)
        {            
        }
    }
}
