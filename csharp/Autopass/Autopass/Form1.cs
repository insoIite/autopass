using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Collections;

//TODO Use env var for gopass path if it is not in $PATH

namespace Autopass
{
    public partial class Form1 : Form
    {
        // Import GetForegroundWindow
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();
        // Import SetForegroundWindow
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        // Store current focus before initializing the Form
        IntPtr currentFocus = GetForegroundWindow();

        public Form1()
        {
            InitializeComponent();
            fillList();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
                 
        }

        private void listView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                String item = passwordsList.FocusedItem.Text;
                Hashtable entryHash = getPasswordEntryHashTable(item);
                //SendKeys.Send()
                foreach (string key in entryHash.Keys)
                {
                    Console.WriteLine(String.Format("{0}: {1}", key, entryHash[key]));
                }
                this.Visible = false;
                SetForegroundWindow(currentFocus);
                typeEntry(entryHash);
            }
        }
        
        private void fillList()
        // Call gopass exe and return password entries from standart output
        {
            var passwordsEntries = execProgramAndReturnOutput("gopass", "ls --flat");
            foreach (var el in passwordsEntries.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries))
            {
                ListViewItem item = new ListViewItem
                {
                    Text = el
                };
                passwordsList.Items.Add(item);
            }
        }
        
        private String execProgramAndReturnOutput(String prg, String args)
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

        private Hashtable getPasswordEntryHashTable(String entry)
        {
            var passwordEntry = execProgramAndReturnOutput("gopass", $"show {entry}");
            Hashtable entryHash = new Hashtable();
            foreach (var el in passwordEntry.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (!el.Contains(':'))
                {
                    entryHash.Add("pass", el);
                } else
                {
                    var splitedEntry = el.Split(new String[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                    entryHash.Add(splitedEntry[0].Trim(), splitedEntry[1].Trim());
                }
            }
            return entryHash;
        }

        private void typeEntry(Hashtable passwordEntry)
        {
            String autoTypeOpts = (String)passwordEntry["autotype"];
            var autoTypeOpt = autoTypeOpts.Split(new String[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var opt in autoTypeOpt)
            {
                Trace.Write(opt);
                if (passwordEntry.ContainsKey(opt))
                {
                    String value = (String)passwordEntry[opt];
                    SendKeys.SendWait(value);
                } else if (opt.Contains("!"))
                {
                    var _opt = opt.Replace("!", "");
                    SendKeys.SendWait(opt);
                } else
                {
                    SendKeys.SendWait(opt);
                }
            }
        }
    }

    
}
