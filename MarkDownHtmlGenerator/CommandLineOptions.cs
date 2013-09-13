using CommandLine;
using CommandLine.Text;

namespace MarkDownHtmlGenerator
{
    public class CommandLineOptions
    {
        [Option('p', "path", Required = true, HelpText = @"Path which is include Markdown files. You MUST set this argument. Example: -p C:\Projects\ProjA\Apis")]
        public string Path { get; set; }

        [Option('o', "output", Required = false, HelpText = @"Path which will save genarated html files.  Example: -o C:\Projects\ProjA\Apis\Html")]
        public string Output { get; set; }

        [Option('f', "files", Required = false, HelpText = @"Markdown file name list. Example: -f Index.md,Base.md,UserValidation.md")]
        public string Files { get; set; }

        [Option('s', "searchpattern", Required = false, HelpText = @"Markdown file search patter. Example: -s *.md or -s *Index*.md")]
        public string SearchPartten { get; set; }

        [Option('v', "verbose", DefaultValue = false, HelpText = @"Prints all messages to standard output. Example: -v")]
        public bool Verbose { get; set; }

        [ParserState]
        public IParserState LastParserState { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this,
              current => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}
