using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DiscoveryRenamer
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] helpOptions = {"?", "-?", "/?", "\\?", "help"};
            if (args.Length == 0 || args.Length > 1 || 
                (args.Length == 1 && helpOptions.Contains(args[0].ToLower())))
            {
                // they didn't supply a proper directory to clean or they asked for help
                ShowHelp();
            }
            else
            {
                // check to see if parmeter is found on system
            }
        }

        static void ShowHelp()
        {
            Console.WriteLine(" ====================================================================");
            Console.WriteLine(" DirectoryRenamer");
            Console.WriteLine(" ====================================================================");
            Console.WriteLine(" This utility will rename directories and all files contained");
            Console.WriteLine(" so that they only consist of alphanumeric characters, underscore,");
            Console.WriteLine(" or dash.  If this would result in a duplicate name, digits will");
            Console.WriteLine(" be added to keep the item unique.\n");
            Console.WriteLine(" USAGE: DirectoryRenamer <starting directory>\n");
            Console.WriteLine(" The starting directory must be the full path including drive letter.");
            Console.WriteLine(" If the starting directory path contains spaces, it must be enclosed");
            Console.WriteLine(" by double quotation marks.");
            Console.WriteLine(" ====================================================================");
        }
    }
}
