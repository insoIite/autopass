using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Autopass
{
    class Util
    {
        public static String execProgramAndReturnOutput(String prg, String args)
        {
            var process = new Process();
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.FileName = prg;
            process.StartInfo.Arguments = args.ToString();
            process.Start();

            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return output;
        }

        public static void searchAndSelect(TextBox passwordSearch, ListView passwordsList)
        {
            foreach (ListViewItem item in passwordsList.Items)
            {
                if (item.Text.Contains(passwordSearch.Text))
                {
                    passwordsList.Items.Remove(item);
                    passwordsList.Items.Insert(0, item);
                    passwordsList.TopItem = item;
                    passwordsList.Items[item.Index].Selected = true;
                    passwordsList.Items[item.Index].Focused = true;
                }
            }
        }
    }
}
