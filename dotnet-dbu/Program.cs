using DbUp;
using System;
using System.Linq;

namespace dotnet_dbu
{
    static class Program
    {
        private const int ERROR = 2;
        private const int OK = 0;

        static int Main(string[] args)
        {
            try
            {
                var connectionString = GetArg(args, 0);
                var sqlFilesPath = GetArg(args, 1);

                var upgrader = DeployChanges.To
                    .SqlDatabase(connectionString)
                    .WithScriptsFromFileSystem(sqlFilesPath)
                    .WithTransaction()
                    .Build();

                var result = upgrader.PerformUpgrade();

                if (!result.Successful)
                    return WriteResult(ConsoleColor.Red, result.Error.ToString(), ERROR);

                return WriteResult(ConsoleColor.Green, "Success!", OK);
            }
            catch (Exception ex)
            {
                return WriteResult(ConsoleColor.Red, ex.ToString(), ERROR);
            }
        }

        static string GetArg(string[] args, int index)
        {
            if (args.Count() > index)
                return args[index];
            else
                return null;
        }

        static int WriteResult(ConsoleColor color, string message, int retCode)
        {
            Console.ForegroundColor = color;
            Console.Error.WriteLine(message);
            Console.ResetColor();

#if DEBUG
            Console.ReadKey();
#endif

            return retCode;
        }
    }
}
