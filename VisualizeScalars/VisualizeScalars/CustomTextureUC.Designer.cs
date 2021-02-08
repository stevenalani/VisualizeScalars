
namespace VisualizeScalars
{
    partial class CustomTextureUC
    {
        /// <summary> 
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Komponenten-Designer generierter Code

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tbxLeftHand = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbxComparer = new System.Windows.Forms.ComboBox();
            this.tbxRightHand = new System.Windows.Forms.TextBox();
            this.cmdCalculate = new System.Windows.Forms.Button();
            this.colorSelection1 = new ColorSelection();
            this.cmdDelete = new System.Windows.Forms.Button();
            this.tbxBetween = new System.Windows.Forms.TextBox();
            this.cmAddProperty = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.SuspendLayout();
            // 
            // tbxLeftHand
            // 
            this.tbxLeftHand.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.tbxLeftHand.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.tbxLeftHand.Location = new System.Drawing.Point(91, 9);
            this.tbxLeftHand.Name = "tbxLeftHand";
            this.tbxLeftHand.Size = new System.Drawing.Size(380, 23);
            this.tbxLeftHand.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "Berechnung";
            // 
            // cbxComparer
            // 
            this.cbxComparer.FormattingEnabled = true;
            this.cbxComparer.Items.AddRange(new object[] {
            "Nur linke Seite",
            "Nur rechte Seite",
            ">",
            "<",
            "=",
            "zwischen"});
            this.cbxComparer.Location = new System.Drawing.Point(91, 38);
            this.cbxComparer.Name = "cbxComparer";
            this.cbxComparer.Size = new System.Drawing.Size(98, 23);
            this.cbxComparer.TabIndex = 2;
            this.cbxComparer.SelectedIndexChanged += new System.EventHandler(this.cbxComparer_SelectedIndexChanged);
            // 
            // tbxRightHand
            // 
            this.tbxRightHand.Location = new System.Drawing.Point(91, 67);
            this.tbxRightHand.Name = "tbxRightHand";
            this.tbxRightHand.Size = new System.Drawing.Size(380, 23);
            this.tbxRightHand.TabIndex = 3;
            // 
            // cmdCalculate
            // 
            this.cmdCalculate.Location = new System.Drawing.Point(9, 129);
            this.cmdCalculate.Name = "cmdCalculate";
            this.cmdCalculate.Size = new System.Drawing.Size(176, 23);
            this.cmdCalculate.TabIndex = 4;
            this.cmdCalculate.Text = "Berechnen";
            this.cmdCalculate.UseVisualStyleBackColor = true;
            this.cmdCalculate.Click += new System.EventHandler(this.cmdCalculate_Click);
            // 
            // colorSelection1
            // 
            this.colorSelection1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.colorSelection1.Location = new System.Drawing.Point(0, 158);
            this.colorSelection1.Name = "colorSelection1";
            this.colorSelection1.Size = new System.Drawing.Size(716, 85);
            this.colorSelection1.TabIndex = 5;
            // 
            // cmdDelete
            // 
            this.cmdDelete.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cmdDelete.FlatAppearance.BorderSize = 2;
            this.cmdDelete.Location = new System.Drawing.Point(477, 8);
            this.cmdDelete.Name = "cmdDelete";
            this.cmdDelete.Size = new System.Drawing.Size(230, 23);
            this.cmdDelete.TabIndex = 6;
            this.cmdDelete.Text = "Textur Entfernen";
            this.cmdDelete.UseVisualStyleBackColor = true;
            this.cmdDelete.Click += new System.EventHandler(this.cmdDelete_Click);
            // 
            // tbxBetween
            // 
            this.tbxBetween.Location = new System.Drawing.Point(91, 96);
            this.tbxBetween.Name = "tbxBetween";
            this.tbxBetween.Size = new System.Drawing.Size(380, 23);
            this.tbxBetween.TabIndex = 7;
            // 
            // cmAddProperty
            // 
            this.cmAddProperty.Name = "cmAddProperty";
            this.cmAddProperty.Size = new System.Drawing.Size(61, 4);
            // 
            // CustomTextureUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tbxBetween);
            this.Controls.Add(this.cmdDelete);
            this.Controls.Add(this.colorSelection1);
            this.Controls.Add(this.cmdCalculate);
            this.Controls.Add(this.tbxRightHand);
            this.Controls.Add(this.cbxComparer);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbxLeftHand);
            this.Name = "CustomTextureUC";
            this.Size = new System.Drawing.Size(716, 243);
            this.Load += new System.EventHandler(this.CustomTextureUC_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbxLeftHand;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbxComparer;
        private System.Windows.Forms.TextBox tbxRightHand;
        private System.Windows.Forms.Button cmdCalculate;
        internal ColorSelection colorSelection1;
        private System.Windows.Forms.Button cmdDelete;
        private System.Windows.Forms.TextBox tbxBetween;
        private System.Windows.Forms.ContextMenuStrip cmAddProperty;
    }
}
