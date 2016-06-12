using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iis_gzip
{
    /**
     * Program
     */
    class Program
    {
        // output file extension
        static string EXTENSION = ".gz";

        // max size of input file(128MB)
        static int MAX_SIZE = 128 * 1024 * 1024;

        /**
         * Main
         *
         * @param args string[]
         */
        static void Main(string[] args)
        {
            // Derive input file path
            string ifile;
            if (args.Length != 1)
            {
                Console.WriteLine("Argument is required. [input file path]");
                return;
            }
            ifile = args[0];
            if (!Path.IsPathRooted(@args[0]))
            {
                ifile = ".\\" + ifile;
            }
            string prefix = Path.GetDirectoryName(@ifile) + "\\" + Path.GetFileNameWithoutExtension(@ifile);
            string suffix = Path.GetExtension(@ifile);

            // Check input file
            FileInfo iInfo = new FileInfo(@ifile);
            if (!iInfo.Exists)
            {
                Console.WriteLine("Input file does not exist. [input file path] = " + ifile);
                return;
            }
            if (MAX_SIZE <= iInfo.Length)
            {
                Console.WriteLine("Input file size is too large. [max size] = " + MAX_SIZE + "byte, [input file size] = " + iInfo.Length + "byte");
                return;
            }

            // Read input file
            Byte[] inps;
            FileStream fs = null;
            try
            {
                fs = new FileStream(@ifile, FileMode.Open);
                inps = new Byte[fs.Length];
                fs.Read(inps, 0, inps.Length);
                fs.Close();
            }
            finally
            {
                if (fs != null)
                {
                    try
                    {
                        fs.Close();
                    }
                    catch (Exception e) { Console.WriteLine(e.StackTrace); }
                }
            }

            // Compress input file
            IISGzip obj = new IISGzip();
            FileStream outs = null;
            try
            {
                for (int level = 0; level <= 10; level++)
                {
                    outs = new FileStream(@prefix + "_" + String.Format("{0:00}", level) + suffix + EXTENSION, FileMode.Create);
                    obj.compress(inps, outs, level);
                    outs.Close();
                }
            }
            finally
            {
                if (outs != null)
                {
                    try
                    {
                        outs.Close();
                    }
                    catch (Exception e) { Console.WriteLine(e.StackTrace); }
                }
            }
        }
    }
}
