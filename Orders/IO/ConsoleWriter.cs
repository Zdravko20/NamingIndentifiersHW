using System;

namespace Orders.IO
{
    class ConsoleWriter :OutputWriter
    {
        public override void Write(string line)
        {
            Console.WriteLine(line);
        }
    }
}
