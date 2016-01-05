namespace Orders.IO
{
    using System;

    internal class ConsoleWriter : OutputWriter
    {
        public override void Write(string line)
        {
            Console.WriteLine(line);
        }
    }
}