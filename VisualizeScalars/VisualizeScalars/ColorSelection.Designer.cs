
namespace VisualizeScalars
{
    partial class ColorSelection
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
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.tbxRadius = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbUseScalarValues = new System.Windows.Forms.CheckBox();
            this.lbOpacity = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tbAlpha = new System.Windows.Forms.TrackBar();
            this.panelColorPreview = new System.Windows.Forms.Panel();
            this.cmdPickColor = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbAlpha)).BeginInit();
            this.SuspendLayout();
            // 
            // tbxRadius
            // 
            this.tbxRadius.Location = new System.Drawing.Point(98, 18);
            this.tbxRadius.Name = "tbxRadius";
            this.tbxRadius.Size = new System.Drawing.Size(28, 23);
            this.tbxRadius.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "Radius in Pixel";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbUseScalarValues);
            this.groupBox1.Controls.Add(this.lbOpacity);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.tbAlpha);
            this.groupBox1.Controls.Add(this.panelColorPreview);
            this.groupBox1.Controls.Add(this.cmdPickColor);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.tbxRadius);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(695, 59);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // cbUseScalarValues
            // 
            this.cbUseScalarValues.AutoSize = true;
            this.cbUseScalarValues.Location = new System.Drawing.Point(481, 20);
            this.cbUseScalarValues.Name = "cbUseScalarValues";
            this.cbUseScalarValues.Size = new System.Drawing.Size(127, 19);
            this.cbUseScalarValues.TabIndex = 9;
            this.cbUseScalarValues.Text = "Opazität von Skalar";
            this.cbUseScalarValues.UseVisualStyleBackColor = true;
            // 
            // lbOpacity
            // 
            this.lbOpacity.AutoSize = true;
            this.lbOpacity.Location = new System.Drawing.Point(444, 21);
            this.lbOpacity.Name = "lbOpacity";
            this.lbOpacity.Size = new System.Drawing.Size(13, 15);
            this.lbOpacity.TabIndex = 8;
            this.lbOpacity.Text = "0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(277, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 15);
            this.label1.TabIndex = 7;
            this.label1.Text = "Opazität";
            // 
            // tbAlpha
            // 
            this.tbAlpha.Location = new System.Drawing.Point(334, 14);
            this.tbAlpha.Maximum = 100;
            this.tbAlpha.Name = "tbAlpha";
            this.tbAlpha.Size = new System.Drawing.Size(104, 45);
            this.tbAlpha.TabIndex = 6;
            this.tbAlpha.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // panelColorPreview
            // 
            this.panelColorPreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelColorPreview.Location = new System.Drawing.Point(227, 18);
            this.panelColorPreview.Name = "panelColorPreview";
            this.panelColorPreview.Size = new System.Drawing.Size(44, 23);
            this.panelColorPreview.TabIndex = 5;
            // 
            // cmdPickColor
            // 
            this.cmdPickColor.Location = new System.Drawing.Point(132, 17);
            this.cmdPickColor.Name = "cmdPickColor";
            this.cmdPickColor.Size = new System.Drawing.Size(89, 23);
            this.cmdPickColor.TabIndex = 4;
            this.cmdPickColor.Text = "Farbauswahl";
            this.cmdPickColor.UseVisualStyleBackColor = true;
            this.cmdPickColor.Click += new System.EventHandler(this.cmdPickColor_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(614, 17);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Vorschau";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ColorSelection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "ColorSelection";
            this.Size = new System.Drawing.Size(695, 59);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbAlpha)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.TextBox tbxRadius;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button cmdPickColor;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panelColorPreview;
        private System.Windows.Forms.Label lbOpacity;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TrackBar tbAlpha;
        internal System.Windows.Forms.GroupBox groupBox1;
        internal System.Windows.Forms.CheckBox cbUseScalarValues;
    }
}
