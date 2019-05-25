namespace Autopass
{
    partial class Form1
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.passwordsList = new System.Windows.Forms.ListView();
            this.passwordHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.passwordSearch = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // passwordsList
            // 
            this.passwordsList.BackColor = System.Drawing.SystemColors.Info;
            this.passwordsList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.passwordHeader});
            this.passwordsList.Font = new System.Drawing.Font("Rockwell", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.passwordsList.FullRowSelect = true;
            this.passwordsList.GridLines = true;
            this.passwordsList.HideSelection = false;
            this.passwordsList.Location = new System.Drawing.Point(12, 48);
            this.passwordsList.MultiSelect = false;
            this.passwordsList.Name = "passwordsList";
            this.passwordsList.Scrollable = false;
            this.passwordsList.Size = new System.Drawing.Size(770, 628);
            this.passwordsList.TabIndex = 0;
            this.passwordsList.UseCompatibleStateImageBehavior = false;
            this.passwordsList.View = System.Windows.Forms.View.Details;
            this.passwordsList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.passwordsList_KeyDown);
            // 
            // passwordHeader
            // 
            this.passwordHeader.Text = "Passwords";
            this.passwordHeader.Width = 816;
            // 
            // passwordSearch
            // 
            this.passwordSearch.AcceptsReturn = true;
            this.passwordSearch.Font = new System.Drawing.Font("Rockwell", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.passwordSearch.HideSelection = false;
            this.passwordSearch.Location = new System.Drawing.Point(12, 10);
            this.passwordSearch.Name = "passwordSearch";
            this.passwordSearch.Size = new System.Drawing.Size(770, 32);
            this.passwordSearch.TabIndex = 1;
            this.passwordSearch.TextChanged += new System.EventHandler(this.passwordSearch_TextChanged);
            this.passwordSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.passwordSearch_KeyDown);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(794, 688);
            this.Controls.Add(this.passwordSearch);
            this.Controls.Add(this.passwordsList);
            this.Name = "Form1";
            this.Text = "Autopass";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView passwordsList;
        private System.Windows.Forms.ColumnHeader passwordHeader;
        private System.Windows.Forms.TextBox passwordSearch;
    }
}

