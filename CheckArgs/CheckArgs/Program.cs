using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Reflection.Metadata;
using System.Text;

namespace CheckArgs
{
    internal class Program
    {
        private static void Main()
        {
            #region CheckArgProg

            #region args v3

            var currentDir = Directory.GetCurrentDirectory();

            input:
            Console.Write($"> ");
            var input = Console.ReadLine();

            var arguments = ParseCommandLine(input); // Parse arguments
            var commandName = arguments[0]; // Command name
            if (arguments.Count > 0) arguments.RemoveAt(0); // Leave only the arguments

            #region pwd command

            if (commandName.TrimEnd(' ').ToLower() is "pwd")
            {
                if (arguments.Count > 0 && (arguments[0].ToLower() is "-h" or "--help"))
                {
                    SnowHelp("pwd", "This command display current working directory", "-h --help");
                    goto input;
                }

                if (arguments.Count is 0)
                {
                    Console.WriteLine(Directory.GetCurrentDirectory());
                    goto input;
                }

            }


            #endregion

            #region mkdir command

            if (commandName.Trim(' ').ToLower() is "mkdir")
            {
                if (arguments.Count > 0 && (arguments[0].ToLower() is "-h" or "--help"))
                {
                    SnowHelp("mkdir [DIR]", "This command create empty folder", "-h, --help)");
                    goto input;
                }

                if (arguments.Count > 0 && (arguments[0].ToLower() is not "-h" or "--help"))
                {
                    if (Directory.Exists(arguments[0]))
                    {
                        Console.WriteLine("That path exists already.");
                        goto input;
                    }

                    Directory.CreateDirectory(arguments[0]);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Directory created!");
                    Console.ResetColor();
                    goto input;
                }

                Console.WriteLine($"Use \"{commandName} -h\" to display all available actions");
                goto input;
            }

            #endregion

            #region cd command

            if (commandName.TrimEnd(' ').ToLower() is "cd")
            {
                if (arguments.Count > 0 && (arguments[0] is "-h" or "--help"))
                {
                    SnowHelp("cd [DIR]", "This command display current working directory", "-h --help");
                    goto input;
                }

                if (arguments.Count > 0 && (arguments[0] is ".."))
                {
                    currentDir = currentDir + "\\";
                    var pos = currentDir.LastIndexOf("\\", StringComparison.Ordinal);
                    currentDir = currentDir.Remove(pos, currentDir.Length - pos);
                    Directory.SetCurrentDirectory(currentDir);
                }

                if (arguments.Count > 0 && (arguments[0] is not "-h" or "--help" or ".."))
                {
                    if (!Directory.Exists($@"{currentDir}\{arguments[0]}"))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Directory is not exist");
                        Console.ResetColor();

                        goto input;
                    }

                    if (Directory.Exists($@"{currentDir}\{arguments[0]}"))
                    {
                        currentDir = $@"{currentDir}\{arguments[0]}";
                        Directory.SetCurrentDirectory(currentDir);

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Directory changed");
                        Console.ResetColor();

                        goto input;
                    }
                }

                if (arguments.Count is 0)
                {
                    currentDir = @"D:\Github\UmbrellaOS-cosmos rewrite\CheckArgs\CheckArgs\bin\Debug\net5.0\"; // 0:\
                    Directory.SetCurrentDirectory(currentDir);

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Directory changed to home directory");
                    Console.ResetColor();

                    goto input;
                }
            }

            #endregion

            #region ls command

            if (commandName.Trim(' ').ToLower() is "ls")
            {
                var dirs = Directory.GetDirectories(currentDir);

                if (arguments.Count > 0 && (arguments[0].ToLower() is "-h" or "--help"))
                {
                    SnowHelp("ls [ARG]", "This command outputs all files in the current directory", "-h, --help, -R, -al");

                    goto input;
                }

                if (arguments.Count > 0 && (arguments[0].ToLower() is "-r"))
                {
                    foreach (var dirsInCurrentDir in dirs)
                    {
                        var dirInfo = new DirectoryInfo(dirsInCurrentDir);
                        var folderName = dirInfo.Name;

                        
                        Console.WriteLine($"Folder: {folderName}");

                        var subDirectories = Directory.GetDirectories(dirsInCurrentDir);
                        foreach (var subFolder in subDirectories)
                        {
                            var subDirInfo = new DirectoryInfo(subFolder);
                            var subFolderName = subDirInfo.Name;

                            Console.WriteLine($"Sub-folder: {subFolderName}");
                        }

                        var subFiles = Directory.GetFiles(dirsInCurrentDir);
                        foreach (var subFile in subFiles)
                        {
                            var fileName = Path.GetFileName(subFile);
                            Console.WriteLine("Sub-file: " + fileName);
                        }
                    }

                    goto input;
                }

                if (arguments.Count > 0 && (arguments[0].ToLower() is "-al"))
                {
                    var allFiles = Directory.GetFiles(currentDir);
                    foreach (var oneFile in allFiles)
                    {
                        var fileName = Path.GetFileName(oneFile);
                        var fileSize = new FileInfo(oneFile).Length;
                        var FileContent = File.ReadAllText(oneFile);

                        Console.WriteLine("File name: " + fileName);
                        Console.WriteLine("File size: " + fileSize + "byte");
                        Console.WriteLine("Content: " + FileContent);
                    }

                    goto input;
                }

                if (arguments.Count is 0)
                {

                    foreach (var dirsInCurrentDir in dirs)
                    {
                        var dirInfo = new DirectoryInfo(dirsInCurrentDir);
                        var folderName = dirInfo.Name;

                        Console.WriteLine(folderName);
                    }

                    var allFiles = Directory.GetFiles(currentDir);
                    foreach (var oneFile in allFiles)
                    {
                        var fileName = Path.GetFileName(oneFile);
                        Console.WriteLine(fileName);
                    }

                    goto input;
                }
            }

            #endregion

            #region cat command

            // TODO

            #endregion

            #region echo command

            if (commandName.Trim(' ').ToLower() is "echo")
            {
                if (arguments.Count > 0 && (arguments[0].ToLower() is "-h" or "--help"))
                {
                    SnowHelp("echo [TEXT]", "This command outputs the string you specified in the argument.", "-h, --help");
                    goto input;
                }

                if (arguments.Count > 0 && (arguments[0].ToLower() is not "-h" or "--help"))
                {
                    foreach (var allText in arguments)
                    {
                        Console.Write($"{allText} ");
                    }

                    Console.WriteLine();
                    goto input;
                }

                if (arguments.Count is 0)
                {
                    Console.WriteLine($"Use \"{commandName} -h\" to display all available actions");
                    goto input;
                }
            }

            #endregion

            #region cls command

            if (commandName.Trim(' ').ToLower() is "cls" or "clear")
            {
                if (arguments.Count > 0 && (arguments[0].ToLower() is "-h" or "--help"))
                {
                    SnowHelp("cls (or clear)", "This command clear console", "-h, --help");
                    goto input;
                }

                if (arguments.Count is 0)
                {
                    Console.Clear();
                    goto input;
                }
            }

            #endregion



            #region if we are dealing with a commands that do not exist

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(
                $"Unknown command \"{commandName}\". Type \"help\" to display available commands."); // Пошёл нахуй по-русски
            Console.ResetColor();
            goto input;

            #endregion

            #endregion

            #endregion

            }


            public static void SnowHelp(string usage, string description, string options)
            {
                Console.WriteLine("  USAGE:");
                Console.WriteLine(usage);
                Console.WriteLine("  DESCRIPTION:");
                Console.WriteLine(description);
                Console.WriteLine("  OPTIONS:");
                Console.WriteLine(options);
            }

            public static List<string> ParseCommandLine(string cmdLine)
            {
                var args = new List<string>();
                if (string.IsNullOrWhiteSpace(cmdLine)) return args;

                var currentArg = new StringBuilder();
                var inQuotedArg = false;

                for (var i = 0; i < cmdLine.Length; i++)
                {
                    if (cmdLine[i] == '"')
                    {
                        if (inQuotedArg)
                        {
                            args.Add(currentArg.ToString());
                            currentArg = new StringBuilder();
                            inQuotedArg = false;
                        }
                        else
                        {
                            inQuotedArg = true;
                        }
                    }
                    else if (cmdLine[i] == ' ')
                    {
                        if (inQuotedArg)
                        {
                            currentArg.Append(cmdLine[i]);
                        }
                        else if (currentArg.Length > 0)
                        {
                            args.Add(currentArg.ToString());
                            currentArg = new StringBuilder();
                        }
                    }
                    else
                    {
                        currentArg.Append(cmdLine[i]);
                    }
                }

                if (currentArg.Length > 0) args.Add(currentArg.ToString());

                return args;
            }

        }
    }