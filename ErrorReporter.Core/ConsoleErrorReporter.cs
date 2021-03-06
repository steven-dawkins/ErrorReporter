﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErrorReporter.Core
{
    public class ConsoleErrorReporter : IErrorReporter
    {
        public void Capture(Exception e, IEnumerable<KeyValuePair<string, object>> extraInformation)
        {
            this.Capture(e.Message, Level.Error, extraInformation);
        }
        
        public void Capture(string message, Level level, IEnumerable<KeyValuePair<string, object>> extraInformation)
        {
            System.Diagnostics.Debug.WriteLine($"{level} {message} {string.Join(",", extraInformation.Select(kv => $"{kv.Key}={kv.Value}"))}");
        }
    }
}
