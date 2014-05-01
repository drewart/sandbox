using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace EmptyDirs
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1) {
                //usage
                System.Console.Out.WriteLine("no directory");
                Environment.Exit(-1);
            }

            Console.Out.WriteLine("Empty directories:");
            DirectoryInfo dir = new DirectoryInfo(args[0]);
            ListEmptyDir(dir);

        }

        static int ListEmptyDir(DirectoryInfo dir)
        {
            int fileCount = 0;
            //Console.WriteLine("checking: " + dir.FullName);
            foreach (DirectoryInfo d in dir.GetDirectories())
            { 
                fileCount += ListEmptyDir(d);                    
            }
            

            FileInfo[] files = dir.GetFiles();

            int hiddenFileCount = files.Where(f => f.Attributes.HasFlag(FileAttributes.Hidden)).Count();
            fileCount += files.Where(f => !f.Attributes.HasFlag(FileAttributes.Hidden)).Count();

            if (fileCount == 0)
                Console.WriteLine("Empty: H:{0} {1}",hiddenFileCount, dir.FullName);
            
            return fileCount;
        }
    }
}
