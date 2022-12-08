using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace ContextMenuRename
{
    internal class Program
    {
        enum MODULE
        {
            None,
            RandomGUID,
            PrependRandomGUID,
            AppendRandomGUID,
        }

        static void Main(string[] args)
        {
            // Error: not enough arguments
            if (args.Length < 2)
                System.Environment.Exit(1);

            // Parse: module
            var module = MODULE.RandomGUID;
            try
            {
                module = (MODULE)short.Parse(args[0]);
            }
            catch (System.IndexOutOfRangeException)
            {
                System.Environment.Exit(1);
            }

            // Parse: file(s) name(s)
            string[] fileNames = new string[args.Length - 1];
            for (var i = 1; i < args.Length; i++)
                fileNames[i - 1] = args[i];

            switch (module)
            {
                case MODULE.None:
                    break;
                case MODULE.RandomGUID:
                    // Error: too many files
                    if (fileNames.Length > 1)
                        System.Environment.Exit(1);

                    RenameFileRandomGUID(fileNames.First(), GenerateGUID());
                    break;
                case MODULE.PrependRandomGUID:
                    // Error: too many files
                    if (fileNames.Length > 1)
                        System.Environment.Exit(1);

                    RenameFilePrependRandomGUID(fileNames.First(), GenerateGUID());
                    break;
                case MODULE.AppendRandomGUID:
                    // Error: too many files
                    if (fileNames.Length > 1)
                        System.Environment.Exit(1);

                    RenameFileAppendRandomGUID(fileNames.First(), GenerateGUID());
                    break;
            }

            // Force to close
            System.Environment.Exit(0);
        }

        static bool RenameFileRandomGUID(in string sourceFile, in string GUID)
        {
            if (!File.Exists(sourceFile))
                return false;

            File.Move(
                sourceFile,
                Path.Combine(
                    Path.GetDirectoryName(sourceFile),
                    GUID + Path.GetExtension(sourceFile)
                )
            );

            return true;
        }

        static bool RenameFilePrependRandomGUID(in string sourceFile, in string GUID)
        {
            if (!File.Exists(sourceFile))
                return false;

            File.Move(
                sourceFile,
                Path.Combine(
                    Path.GetDirectoryName(sourceFile),
                    GUID + Path.GetFileName(sourceFile)
                )
            );

            return true;
        }

        static bool RenameFileAppendRandomGUID(in string sourceFile, in string GUID)
        {
            if (!File.Exists(sourceFile))
                return false;

            File.Move(
                sourceFile,
                Path.Combine(
                    Path.GetDirectoryName(sourceFile),
                    Path.GetFileNameWithoutExtension(sourceFile) + GUID + Path.GetExtension(sourceFile)
                )
            );

            return true;
        }

        static string GenerateGUID()
        {
            return System.Guid.NewGuid().ToString().ToUpper();
        }
    }
}
