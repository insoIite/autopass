using System;
using System.Windows.Forms;

//TODO Use env var for gopass path if it is not in $PATH

namespace Autopass
{
    public partial class Autopass : Form
    {
        Gopass gopass;
        public Autopass()
        {
            this.gopass = new Gopass();
            InitializeComponent();
            this.gopass.fillList(this.passwordsList);
            this.ActiveControl = passwordSearch;
        }

        private void passwordsList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                String entry = this.passwordsList.FocusedItem.Text;
                gopass.typeEntry(entry);
                Application.Exit();
            }
        }

        private void passwordSearch_TextChanged(object sender, EventArgs e)
        {
            Util.searchAndSelect(passwordSearch, passwordsList);
        }

        private void passwordSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                String entry = this.passwordsList.FocusedItem.Text;
                gopass.typeEntry(entry);
                Application.Exit();
            }
        }
    }
}
