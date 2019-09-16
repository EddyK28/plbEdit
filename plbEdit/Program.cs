using plbEdit.Format;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace plbEdit
{
    static class Program
    {
        [DllImport("kernel32.dll")]
        private static extern bool AllocConsole();

        [DllImport("kernel32.dll")]
        private static extern bool AttachConsole(int pid);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool FreeConsole();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            bool bConsole = true;
            MessagePrinter.writeConsole = true;
            string file = null;

            if (args.Length == 1 && File.Exists(args[0]))
                file = args[0];

            //Command Line Mode
            else if (args.Length > 0)
            {
                if (!bConsole)
                    AllocConsole();

                if (args[0].Equals("/?") || args[0].Equals("/help", StringComparison.OrdinalIgnoreCase))
                    ShowHelp();
                else
                    ParseArgs(args);

                return;
            }

            //Detect launch from conole
            FreeConsole();
            MessagePrinter.writeConsole = bConsole = AttachConsole(-1);

            //Gui Mode
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            PlbEdit plbEdit = new PlbEdit();
            if (file != null) plbEdit.OpenFile(file);
            Application.Run(plbEdit);
        }

        static void ShowHelp()
        {
            Console.WriteLine("plbEdit: An experimental PMD 3/4 placement data editor.  By EddyK28.\n\n" +
                              "Usage: plbEdit <option> <input file> <output file>\n" +
                              "Options:\n" +
                              "\t/wj\t/writeJson\tOpen a PLB file and write as JSON\n" +
                              "\t/wx\t/writeXml\tOpen a PLB file and write as XML\n" +
                              "\t/rj\t/readJson\tOpen a PLB JSON and convert to PLB\n" +
                              "\t/rx\t/readXml\tOpen a PLB XML and convert to PLB\n" +
                              "\t/t\t/test\t\tOpen and re-save a PLB file for testing");
        }

        static void ParseArgs(string[] args)
        {
            int mode = 0;

            if (args[0].Equals("/wj") || args[0].Equals("/writeJson", StringComparison.OrdinalIgnoreCase))
                mode = 1;
            else if (args[0].Equals("/wx") || args[0].Equals("/writeXml", StringComparison.OrdinalIgnoreCase))
                mode = 2;
            else if (args[0].Equals("/rj") || args[0].Equals("/readJson", StringComparison.OrdinalIgnoreCase))
                mode = 3;
            else if (args[0].Equals("/rx") || args[0].Equals("/readXml", StringComparison.OrdinalIgnoreCase))
                mode = 4;
            else if (args[0].Equals("/t") || args[0].Equals("/test", StringComparison.OrdinalIgnoreCase))
                mode = 5;
            else
            {
                Console.WriteLine("ERROR: Invalid Argument(s)\n");
                ShowHelp();
                return;
            }

            if (args.Length < 3)
            {
                Console.WriteLine("ERROR: Missing File Argument(s)\n");
                return;
            }
            else if (args.Length > 3)
            {
                Console.WriteLine("ERROR: Unexpected File Argument(s)\n");
                return;
            }

            try
            {
                //open files
                Stream fileIn  = new FileStream(args[1], FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                Stream fileOut = new FileStream(args[2], FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                PLB data = null;

                //select action based on mode
                if (mode < 3)   //write json/xml
                {
                    data = new PLB(new PLBReader(fileIn));
                    PlbEdit.SerializePLB(fileOut, data, mode);
                }
                else if (mode < 5)  //read json/xml
                {
                    PlbEdit.DeserializePLB(fileIn, ref data, mode-2);
                    data.Build(new PLBWriter(fileOut));
                }
                else
                {
                    data = new PLB(new PLBReader(fileIn));
                    data.Build(new PLBWriter(fileOut));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"ERROR: {e.Message}\n");
            }
        }
    }
}
