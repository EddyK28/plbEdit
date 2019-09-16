using System;
using System.Text;
using System.Windows.Forms;

namespace plbEdit
{
    public class MessagePrinter
    {
        public static bool writeConsole;
        private static StringBuilder messages = new StringBuilder();

        public static void AddMsg(string msg)
        {
            if (writeConsole) Console.WriteLine(msg);
            messages.AppendLine(msg);
        }

        public static void ShowMsg(string prefix, string title = "Message", MessageBoxIcon icon = MessageBoxIcon.Warning)
        {
            if (writeConsole || messages.Length < 1) return;

            MessageBox.Show(
            prefix + '\n' + messages.ToString(),
            title,
            MessageBoxButtons.OK,
            icon);
        }

        public static void ClearMsg()
        {
            messages.Clear();
        }
    }
}
