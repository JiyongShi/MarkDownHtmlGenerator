using System;
using System.Collections.Generic;
using System.IO;

namespace MarkDownHtmlGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = new CommandLineOptions();
            string[] currentMarkdownFile = { string.Empty }; // Used for passing current file to Url qualify.

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
                                Console.WriteLine("{0} have include in Search pattern list,ignore", psf);
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

                var markdown = new MarkdownFix()
                {
                    ExtraMode = true,
                    SafeMode = false,
                    QualifyUrl = (href) => options.ResoveLinkedMarkdown ? QualifyUrl(href, currentMarkdownFile[0], fileNameList) : href
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
                        currentMarkdownFile[0] = file; // Used to qualify Urls

                        string markdownContent = markdownReader.ReadToEnd();
                        string htmlContent = markdown.Transform(markdownContent);

                        using (StreamWriter sw = File.CreateText(htmlFile))
                        {
                            // html header
                            sw.WriteLine("<!Doctype html><html xmlns=\"http://www.w3.org/1999/xhtml\">");
                            sw.WriteLine("<head>");
                            sw.WriteLine("<meta http-equiv=Content-Type content=text/html;charset=utf-8\">");

                            if (!string.IsNullOrEmpty(options.CssFile))
                            {
                                sw.WriteLine(" <link href=\"" + options.CssFile + "\" media=\"all\" rel=\"stylesheet\" type=\"text/css\" />");
                            }

                            sw.WriteLine("</head>");
                            sw.WriteLine("<body>");
                            sw.Write(htmlContent);
                            // html end
                            sw.WriteLine("</body>");
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

        private static string QualifyUrl(string href, string currentFile, IList<string> files)
        {
            if (href.StartsWith("/") || href.StartsWith("#") || href.Contains("://") || href.StartsWith("mailto:"))
                return href;

            var hrefPath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(currentFile), href));
            if (files.Contains(hrefPath))
            {
                return Path.ChangeExtension(href, ".html");
            }

            return href;
        }
    }
}
