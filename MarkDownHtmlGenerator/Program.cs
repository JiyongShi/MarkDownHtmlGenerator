using System;
using System.Collections.Generic;
using System.IO;
using MarkdownDeep;

namespace MarkDownHtmlGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = new CommandLineOptions();
            if (CommandLine.Parser.Default.ParseArguments(args, options))
            {
                if (!Directory.Exists(options.Path))
                {
                    Console.WriteLine("Path {0} NOT Exists", options.Path);
                    Environment.Exit(1);
                }
                var fileNameList = new List<string>();

                // SearchPartten Files
                if (!string.IsNullOrEmpty(options.SearchPartten))
                {
                    var searchFiles = Directory.GetFiles(options.Path, options.SearchPartten);
                    fileNameList.AddRange(searchFiles);
                }

                // Parameter Files
                if (!string.IsNullOrEmpty(options.Files))
                {
                    var paramFileList = options.Files.Split(new[] { ',', ' ' });
                    foreach (var psf in paramFileList)
                    {
                        if (!File.Exists(Path.Combine(options.Path, psf)))
                        {
                            Console.WriteLine("{0} NOT Exists. Check Files argument", psf);
                            Environment.Exit(1);
                        }

                        if (!fileNameList.Contains(psf))
                            fileNameList.Add(psf);
                        else
                            if (options.Verbose)
                                Console.WriteLine("{0} have include in Search partten list,ignore", psf);
                    }
                }

                // No SearchPattern AND No Parameter Files, Search all .md files
                if (string.IsNullOrEmpty(options.SearchPartten) && string.IsNullOrEmpty(options.Files))
                {
                    var searchFiles = Directory.GetFiles(options.Path, "*.md");
                    fileNameList.AddRange(searchFiles);
                }

                if (fileNameList.Count == 0)
                {
                    Console.WriteLine("No Files to process");
                    Environment.Exit(0);
                }

                var markdown = new Markdown
                {
                    ExtraMode = true,
                    SafeMode = false
                };

                int processCount = 0;
                foreach (var file in fileNameList)
                {
                    if (!File.Exists(file))
                    {
                        if (options.Verbose)
                            Console.WriteLine("{0} NOT Exists", file);
                        continue;
                    }

                    // Generator html
                    processCount++;
                    string htmlFile = Path.Combine(options.Output ?? options.Path,
                        Path.GetFileNameWithoutExtension(file) + ".html");
                    using (var markdownReader = new StreamReader(file))
                    {
                        string markdownContent = markdownReader.ReadToEnd();
                        string htmlContent = markdown.Transform(markdownContent);

                        using (StreamWriter sw = File.CreateText(htmlFile))
                        {
                            // html header
                            sw.WriteLine("<!Doctype html><html xmlns=http://www.w3.org/1999/xhtml>");
                            sw.WriteLine("<head><meta http-equiv=\"Content-Type\" content=\"text/html;charset=utf-8\" /></head>");
                            sw.Write(htmlContent);
                            // html end
                            sw.WriteLine("</html>");
                            sw.Close();
                        }

                        if (options.Verbose)
                            Console.WriteLine("{0}:{1}", htmlFile,
                                string.IsNullOrEmpty(htmlContent) ? 0 : htmlContent.Length);

                        markdownReader.Close();
                    }

                }
                Console.WriteLine("Generated Total:{0}", processCount);
                Environment.Exit(0);
            }

            Environment.Exit(1);
        }
    }
}
