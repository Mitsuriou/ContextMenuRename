namespace ContextMenuRename
{
    internal class Program
    {
        enum TargetModule
        {
            None,
            RandomGUID
        }

        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                System.Environment.Exit(1);
            }

            // File name
            var fileName = args[0];
            if (fileName.Length <= 0)
            {
                System.Environment.Exit(1);
            }

            // Target module
            var targetApp = TargetModule.RandomGUID;
            try
            {
                targetApp = (TargetModule)System.Int16.Parse(args[1]);
            }
            catch (System.IndexOutOfRangeException)
            {
                System.Environment.Exit(1);
            }

            switch (targetApp)
            {
                case TargetModule.None:
                    break;
                case TargetModule.RandomGUID:
                    RenameFileRandomGUID(fileName);
                    break;
            }

            // Force to close
            System.Environment.Exit(0);
        }

        static bool RenameFileRandomGUID(string fileName)
        {
            var newGUID = System.Guid.NewGuid().ToString().ToUpper();
            var parentDirectory = System.IO.Path.GetDirectoryName(fileName);
            var extension = System.IO.Path.GetExtension(fileName);

            System.IO.File.Move(fileName, System.IO.Path.Combine(parentDirectory, newGUID + extension));

            return true;
        }
    }
}
