using CommandLine;

namespace LogIndexer.Processor.Console
{
    class Options
    {
        [Option('p', "ProcessLog", DefaultValue = 0)]
        public int ProcessLog { get; set; }

        [Option('s', "Seed")]
        public bool Seed { get; set; }

        [Option('i', "Index")]
        public bool Index { get; set; }

        [Option('m', "Models")]
        public bool Models { get; set; }
    }
}