using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace TestRunnerRunner
{
    internal static class Program
    {
        private static readonly List<(string, string, bool)> Runners = new List<(string, string, bool)>
        {
            ("Framework45", "net45", false),
            ("Framework451", "net451", false),
            ("Framework452", "net452", false),
            ("Framework46", "net46", false),
            ("Framework461", "net461", false),
            ("Framework462", "net462", false),
            ("DotNetCore", "netcoreapp1.1", true)
        };

        private static void Main()
        {
            var masterResult = true;

            foreach (var (projectName, binFolder, useDotNet) in Runners)
            {
                Console.Write($"{projectName.PadLeft(12)}: ");
                var result = useDotNet ? DotNetRunner(projectName, binFolder) : ExeRunner(projectName, binFolder);

                Console.WriteLine(result ? " - Passed." : " - FAILED!");
                masterResult &= result;
            }

            Console.WriteLine();
            if (masterResult)
            {
                Console.WriteLine("All tests passed.");
                Environment.Exit(0);
            }

            Console.WriteLine("Tested failed.");
            Console.Read();
            Environment.Exit(1);
        }

        private static bool DotNetRunner(string projectName, string binFolder) =>
            RunProcess("dotnet.exe",
                       $@"..\TestRunner\bin\Release\{binFolder}\TestRunner.dll");

        private static bool ExeRunner(string projectName, string binFolder) =>
            RunProcess($@"..\TestRunner\bin\Release\{binFolder}\TestRunner.exe",
                       "");

        private static bool RunProcess(string fileName, string arguments)
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = arguments,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };
            var process = new Process { StartInfo = startInfo };

            process.Start();
            process.WaitForExit();
            var output = process.StandardOutput.ReadToEnd();
            Console.Write(output);
            return process.ExitCode == 0;
        }
    }
}