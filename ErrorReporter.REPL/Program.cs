using Replify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErrorReporter.REPL
{
    class Program
    {
        static void Main(string[] args)
        {
            var repl = new ClearScriptRepl();

            repl.StartReplLoop(args);
        }
    }
}
