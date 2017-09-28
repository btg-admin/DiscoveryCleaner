using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DiscoveryRenamer
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] helpOptions = {"?", "-?", "/?", "\\?", "help"};
            string startingDirectory = "";
            int changeCounter = 0;
            if (args.Length == 0 || args.Length > 1 || 
                (args.Length == 1 && helpOptions.Contains(args[0].ToLower())))
            {
                // they didn't supply a proper directory to clean or they asked for help
                ShowHelp();
            }
            else
            {
                // check to see if parmeter is a directory and is found on system
                startingDirectory = args[0];
                if (Directory.Exists(startingDirectory))
                {
                    // give warning and get user okay before continuing.
                    string[] goAnswers = { "y", "yes" };
                    Console.WriteLine("WARNING: This will permanently rename files and directories!\n");
                    Console.Write("   CONTINUE? (y/n): ");
                    string response = Console.ReadLine();

                    if (goAnswers.Contains(response.ToLower()))
                    {
                        // let 'er rip!
                        ExamineDirectory(startingDirectory, ref changeCounter);
                        Console.WriteLine("\n COMPLETED - {0} directories/files renamed.", changeCounter);
                    }
                    else
                    {
                        Console.WriteLine("*** CANCELLED ***");
                    }
                }
                else
                {
                    Console.WriteLine(" ERROR: initial directory ({0}) not found.", startingDirectory);
                }
            }
        }

        static void ExamineDirectory(string directoryPath, ref int changeCounter)
        {
            string[] files = Directory.GetFiles(directoryPath);
            string[] directories = Directory.GetDirectories(directoryPath);

            // Check all files
            foreach (var file in files)
            {
                string filename = Path.GetFileName(file);
                if (!NameIsOkay(FilenameMinusExtension(filename)))
                {
                    string newFileName = NewFileName(filename, directoryPath);
                    File.Move(file, directoryPath + "\\" + newFileName);
                    changeCounter++;
                }
            }

            // check all sub-directories and recurse
            foreach (var directory in directories)
            {
                string directoryName = directory.Remove(0, directoryPath.Length + 1);
                string directoryToTraverse = directory;
                if (!NameIsOkay(directoryName))
                {
                    string newDirectoryName = NewDirectoryName(directoryName, directoryPath);
                    Directory.Move(directory, directoryPath + "\\" + newDirectoryName);
                    directoryToTraverse = directoryPath + "\\" + newDirectoryName;
                    changeCounter++;
                }
                ExamineDirectory(directoryToTraverse, ref changeCounter);
            }
        }

        static string NewFileName(string itemname, string currentDirectory)
        {
            bool firsttime = true;
            string newFileName = "";
            int filenumber = 0;

            while (firsttime || File.Exists(currentDirectory + "\\" + newFileName))
            {
                string filename = NewName(itemname.Remove(itemname.LastIndexOf('.')));
                string extension = itemname.Remove(0, itemname.LastIndexOf('.'));
                if (firsttime)
                {
                    newFileName = filename + extension;
                }
                else
                {
                    filenumber++;
                    newFileName = filename + "-" + filenumber + extension;
                }
                firsttime = false;
            }

            return newFileName;
        }

        static string NewDirectoryName(string itemname, string currentDirectory)
        {
            bool firsttime = true;
            string newDirectoryName = "";
            int directoryNumber = 0;

            while (firsttime || Directory.Exists(currentDirectory + "\\" + newDirectoryName))
            {
                if (firsttime)
                {
                    newDirectoryName = NewName(itemname);
                }
                else
                {
                    directoryNumber++;
                    newDirectoryName = NewName(itemname) + "-" + directoryNumber;
                }
                firsttime = false;
            }

            return newDirectoryName;
        }

        static string NewName(string itemname)
        {
            // removes all illegal characters from itemname
            // if that results in an empty string, then "cleaned" is returned
            string pattern = @"[^a-zA-Z\d-_]";
            Regex r = new Regex(pattern);
            string output = r.Replace(itemname, "");
            if (string.IsNullOrEmpty(output))
            {
                output = "cleaned";
            }

            return output;
        }

        static bool NameIsOkay(string itemname)
        {
            string pattern = @"[^a-zA-Z\d-_]";
            Regex r = new Regex(pattern);
            if (r.Match(itemname).Success)
            {
                return false;
            }
            return true;
        }

        static string FilenameMinusExtension(string file)
        {
            return file.Remove(file.LastIndexOf('.'));
        }

        static void ShowHelp()
        {
            Console.WriteLine(" ====================================================================");
            Console.WriteLine(" DirectoryRenamer");
            Console.WriteLine(" ====================================================================");
            Console.WriteLine(" This utility will rename directories and all files contained");
            Console.WriteLine(" so that they only consist of alphanumeric characters, underscore,");
            Console.WriteLine(" or dash.  If this would result in a duplicate name, digits will");
            Console.WriteLine(" be added to keep the item unique.  The starting directory will NOT");
            Console.WriteLine(" be altered.\n");
            Console.WriteLine(" USAGE: DirectoryRenamer <starting directory>\n");
            Console.WriteLine(" The starting directory must be the full path including drive letter.");
            Console.WriteLine(" If the starting directory path contains spaces, it must be enclosed");
            Console.WriteLine(" by double quotation marks.");
            Console.WriteLine(" ====================================================================");
        }
    }
}
