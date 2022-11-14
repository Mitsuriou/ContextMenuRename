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
            RandomGUIDSameForAllFiles,
            PrependRandomGUIDSameForAllFiles,
            AppendRandomGUIDSameForAllFiles,
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
                case MODULE.RandomGUIDSameForAllFiles:
                    RenameFileRandomGUIDSameForAllFiles(fileNames);
                    break;
                case MODULE.PrependRandomGUIDSameForAllFiles:
                    RenameFilePrependRandomGUIDSameForAllFiles(fileNames);
                    break;
                case MODULE.AppendRandomGUIDSameForAllFiles:
                    RenameFileAppendRandomGUIDSameForAllFiles(fileNames);
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

        static void RenameFileRandomGUIDSameForAllFiles(in string[] sourceFiles)
        {
            var commonGUID = GenerateGUID();

            foreach (var sourceFile in sourceFiles)
            {
                RenameFileRandomGUID(sourceFile, commonGUID);
            }
        }

        static void RenameFilePrependRandomGUIDSameForAllFiles(in string[] sourceFiles)
        {
            var commonGUID = GenerateGUID();

            foreach (var sourceFile in sourceFiles)
                RenameFilePrependRandomGUID(sourceFile, commonGUID);
        }

        static void RenameFileAppendRandomGUIDSameForAllFiles(in string[] sourceFiles)
        {
            var commonGUID = GenerateGUID();
            foreach (var sourceFile in sourceFiles)
                RenameFileAppendRandomGUID(sourceFile, commonGUID);
        }

        static string GenerateGUID()
        {
            return System.Guid.NewGuid().ToString().ToUpper();
        }
    }
}
