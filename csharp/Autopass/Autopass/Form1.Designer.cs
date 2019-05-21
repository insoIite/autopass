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
            this.passwordsList.Location = new System.Drawing.Point(12, 12);
            this.passwordsList.Name = "passwordsList";
            this.passwordsList.Size = new System.Drawing.Size(776, 563);
            this.passwordsList.TabIndex = 0;
            this.passwordsList.UseCompatibleStateImageBehavior = false;
            this.passwordsList.View = System.Windows.Forms.View.Details;
            this.passwordsList.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            this.passwordsList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listView1_KeyDown);
            // 
            // passwordHeader
            // 
            this.passwordHeader.Text = "Passwords";
            this.passwordHeader.Width = 816;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 587);
            this.Controls.Add(this.passwordsList);
            this.Name = "Form1";
            this.Text = "Autopass";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView passwordsList;
        private System.Windows.Forms.ColumnHeader passwordHeader;
    }
}

