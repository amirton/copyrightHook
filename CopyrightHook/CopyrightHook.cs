/*
 * © 2017 Amirton Chagas
 * */

using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amirton.Tools.Productivity.CopyrightHook
{
    class CopyrightHook
    {
        const char SPLIT_CHAR = ';';
        const string APPSETTING_EXTENSION = "extensions";
        const string APPSETTING_COPYRIGHT_NOTICE = "copyrightnotice";

        static readonly string[] validExtensions = ConfigurationManager.AppSettings[APPSETTING_EXTENSION].Split(';');
        static readonly string copyrightNotice = ConfigurationManager.AppSettings[APPSETTING_COPYRIGHT_NOTICE];

        static void Main(string[] args)
        {
            var wrongFiles = FindFilesWithWrongCopyrightNotice(File.ReadAllLines(args[0]));

            if (wrongFiles.Count > 0)
            {
                Fail(wrongFiles);
            }
        }

        private static void Fail(List<string> wrongFiles)
        {
            string error = "The following files don't contain the copyright notice \"" + copyrightNotice + "\":\n\n" + string.Join("\n", wrongFiles);
            Console.Error.WriteLine(error);
            Environment.Exit(1);
        }

        static List<string> FindFilesWithWrongCopyrightNotice(string[] changes)
        {
            var wrongFiles = new List<string>();

            foreach (var file in changes)
            {
                if (IsInvalid(file)) {
                    wrongFiles.Add(file);
                }
            }

            return wrongFiles;
        }

        static bool IsInvalid(string file){
            return HasCopyrighteableExtension(file) && !HasCorrectCopyrightNotice(file);
        }

        static bool HasCopyrighteableExtension(string file)
        {
            return validExtensions.Contains(Path.GetExtension(file).ToLower().Replace(".", ""));
        }

        static bool HasCorrectCopyrightNotice(string file)
        { 
            return File.ReadLines(file).Any(x => x.Contains(copyrightNotice));
        }
    }
}
