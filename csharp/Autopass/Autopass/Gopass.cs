using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Autopass
{
    class Gopass
    {

        public Gopass()
        { }

        public void fillList(ListView passwordsList)
        // Call gopass exe and return password entries from standart output
        {
            var passwordsEntries = Util.execProgramAndReturnOutput("gopass", "ls --flat");
            foreach (var el in passwordsEntries.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries))
            {
                ListViewItem item = new ListViewItem
                {
                    Text = el
                };
                passwordsList.Items.Add(item);
            }
        }

        public void typeEntry(Hashtable passwordEntry)
        {
            String autoTypeOpts = (String)passwordEntry["autotype"];
            var autoTypeOptList = autoTypeOpts.Split(new String[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            // Windows doesn't handle 2 focus at a time
            // Cannot get foreground window id before launching the programm
            // This "hack" seems to do the work
            SendKeys.SendWait("%{Tab}");
            // Executing a TAB is slower than the process
            // Therefore it can start to send keys before the TAB actually complete
            Thread.Sleep(100);

            foreach (var opt in autoTypeOptList)
            {
                if (passwordEntry.ContainsKey(opt))
                {
                    String value = (String)passwordEntry[opt];
                    SendKeys.SendWait(value);
                }
                else if (opt.Contains("!"))
                {
                    var _opt = opt.Replace("!", "");
                    // Another differences with linux
                    // Linux: Return, Windows: ENTER
                    // Linux: Tab, Windows: TAB
                    var cmd = _opt.Contains("Return") ? "{ENTER}" : "{TAB}";
                    SendKeys.SendWait(cmd);
                    Thread.Sleep(100);

                }
                else
                {
                    SendKeys.Send(opt);
                }
            }
        }

        public Hashtable getPasswordEntryHashTable(String entry)
        {
            var passwordEntry = Util.execProgramAndReturnOutput("gopass", $"show {entry}");
            Hashtable entryHash = new Hashtable();
            foreach (var el in passwordEntry.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (!el.Contains(':'))
                {
                    entryHash.Add("pass", el);
                }
                else
                {
                    var splitedEntry = el.Split(new String[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                    entryHash.Add(splitedEntry[0].Trim(), splitedEntry[1].Trim());
                }
            }
            return entryHash;
        }
    }
}
