using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Autopass
{
    class Gopass
    {
        public Gopass()
        { }
        /// <summary>
        /// Call gopass in order to retrieve the list of passwords entries
        /// Then fill the listView with the output
        /// </summary>
        /// <param name="passwordsList">The main listView of the application</param>
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
        /// <summary>
        /// Type an entry using Sendkeys depending on the entry autotype key.
        /// </summary>
        /// 
        /// <param name="entry">
        /// An entry is a gopass entry, E.G: websites/github
        /// Its content looks like this:
        /// <code>
        ///   aRdmPassword
        ///   user: foo.bar@mail.com
        ///   autotype: user !Tab pass !Return
        /// </code>
        /// </param>

        /// <remarks>
        /// Senkeys doesn't work well when keyboard locale input is different than EN_US,
        /// therefore we switch the keyboard language (system wide) before the operation.
        /// We switch it back to default afterward
        /// 
        /// With windows APIs we cannot get the Foreground windows id before launching the App.
        /// We also cannot have 2 windows focus at a time.
        /// Therefore before launching the app you need to have the focus on where you want to type the entry,
        /// then launch the app with a shortcut. On '{ENTER}' pressed the code will execute an ALT+TAB to get
        /// back the focus on the correct window then will type the entry
        /// 
        /// Between each sendkeys we wait 100ms before using it again, because senkeys is faster than
        /// the actual change in the UI.
        /// </remarks>
        public void typeEntry(String entry)
        {
            String currentInputLocale = KeyboardLayout.GetLayoutCode();
            Hashtable passwordEntry = this.getPasswordEntryHashTable(entry);
            String autoTypeOpts = (String)passwordEntry["autotype"];
            String[] autoTypeOptList = autoTypeOpts.Split(new String[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            SendKeys.SendWait("%{Tab}");
            Thread.Sleep(100);

            KeyboardLayout.SwitchToEnUSLanguage();
            foreach (var opt in autoTypeOptList)
            {
                if (passwordEntry.ContainsKey(opt))
                {
                    String value = (String)passwordEntry[opt];
                    SendKeys.SendWait(value);
                    Thread.Sleep(100);
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
                    MessageBox.Show(
                        "Failed to autotype:" + opt,
                        "Autopass",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }
            }
            KeyboardLayout.SwitchToDefaultLanguage(currentInputLocale);
        }

        /// <summary>
        /// Create an Hashtable containing each value of the password entry.
        /// </summary>
        /// <param name="entry">
        /// An entry is a gopass entry, E.G: websites/github
        /// Its content looks like this:
        /// <code>
        ///   aRdmPassword
        ///   user: foo.bar@mail.com
        ///   autotype: user !Tab pass !Return
        /// </code>
        /// </param>
        /// <remarks>
        /// SendKeys API have reserved char for itself, so if a password's char is one of these
        /// special chars, we have to enclose them with "{ SpecialChar }"
        /// </remarks>
        /// <returns>HashTable</returns>
        public Hashtable getPasswordEntryHashTable(String entry)
        {
            var passwordEntry = Util.execProgramAndReturnOutput("gopass", $"show {entry}");
            Hashtable entryHash = new Hashtable();
            foreach (var el in passwordEntry.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries))
            {
                // Cannot get index with the foreach keyword
                if (!el.Contains(": "))
                {
                    string password = Regex.Replace(el, "[+^%()\\{\\}]", "{$0}");
                    entryHash.Add("pass", password);
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
