using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.Collections;

//TODO Use env var for gopass path if it is not in $PATH

namespace Autopass
{
    public partial class Form1 : Form
    {
        Gopass gopass;
        public Form1()
        {
            this.gopass = new Gopass();
            InitializeComponent();
            this.gopass.fillList(passwordsList);
        }

        private void listView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                String item = passwordsList.FocusedItem.Text;
                Hashtable entryHash = this.gopass.getPasswordEntryHashTable(item);
                gopass.typeEntry(entryHash);
            }
        }
    }
}
