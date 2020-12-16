
using OpenTK;
using OpenTK.Graphics;

namespace VisualizeScalars
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.label9 = new System.Windows.Forms.Label();
            this.trackBar2 = new System.Windows.Forms.TrackBar();
            this.label8 = new System.Windows.Forms.Label();
            this.btnUpdateData = new System.Windows.Forms.Button();
            this.cbxMeshMode = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.mtbxInterpolation = new System.Windows.Forms.MaskedTextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cbxSmoothing = new System.Windows.Forms.ComboBox();
            this.tbxEast = new System.Windows.Forms.TextBox();
            this.tbxNorth = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tbxWest = new System.Windows.Forms.TextBox();
            this.tbxSouth = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.cmdLoadMap = new System.Windows.Forms.Button();
            this.glControl = new OpenTK.GLControl();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.SuspendLayout();
            //
            // splitContainer1
            //
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(12, 12);
            this.splitContainer1.Name = "splitContainer1";
            //
            // splitContainer1.Panel1
            //
            this.splitContainer1.Panel1.Controls.Add(this.label9);
            this.splitContainer1.Panel1.Controls.Add(this.trackBar2);
            this.splitContainer1.Panel1.Controls.Add(this.label8);
            this.splitContainer1.Panel1.Controls.Add(this.btnUpdateData);
            this.splitContainer1.Panel1.Controls.Add(this.cbxMeshMode);
            this.splitContainer1.Panel1.Controls.Add(this.label7);
            this.splitContainer1.Panel1.Controls.Add(this.mtbxInterpolation);
            this.splitContainer1.Panel1.Controls.Add(this.label6);
            this.splitContainer1.Panel1.Controls.Add(this.label5);
            this.splitContainer1.Panel1.Controls.Add(this.cbxSmoothing);
            this.splitContainer1.Panel1.Controls.Add(this.tbxEast);
            this.splitContainer1.Panel1.Controls.Add(this.tbxNorth);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.Controls.Add(this.label4);
            this.splitContainer1.Panel1.Controls.Add(this.tbxWest);
            this.splitContainer1.Panel1.Controls.Add(this.tbxSouth);
            this.splitContainer1.Panel1.Controls.Add(this.label3);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.trackBar1);
            this.splitContainer1.Panel1.Controls.Add(this.cmdLoadMap);
            //
            // splitContainer1.Panel2
            //
            this.splitContainer1.Panel2.Controls.Add(this.glControl);
            this.splitContainer1.Size = new System.Drawing.Size(947, 475);
            this.splitContainer1.SplitterDistance = 315;
            this.splitContainer1.TabIndex = 1;
            //
            // label9
            //
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(16, 409);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(73, 15);
            this.label9.TabIndex = 22;
            this.label9.Text = "CameraNear";
            //
            // trackBar2
            //
            this.trackBar2.Location = new System.Drawing.Point(16, 427);
            this.trackBar2.Maximum = 10000;
            this.trackBar2.Minimum = 1;
            this.trackBar2.Name = "trackBar2";
            this.trackBar2.Size = new System.Drawing.Size(229, 45);
            this.trackBar2.TabIndex = 21;
            this.trackBar2.Value = 1;
            this.trackBar2.Scroll += new System.EventHandler(this.trackBar2_Scroll);
            //
            // label8
            //
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(16, 332);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(64, 15);
            this.label8.TabIndex = 20;
            this.label8.Text = "CameraFar";
            //
            // btnUpdateData
            //
            this.btnUpdateData.Location = new System.Drawing.Point(16, 93);
            this.btnUpdateData.Name = "btnUpdateData";
            this.btnUpdateData.Size = new System.Drawing.Size(124, 23);
            this.btnUpdateData.TabIndex = 19;
            this.btnUpdateData.Text = "Daten aktualisieren";
            this.btnUpdateData.UseVisualStyleBackColor = true;
            this.btnUpdateData.Click += new System.EventHandler(this.btnUpdateData_Click);
            //
            // cbxMeshMode
            //
            this.cbxMeshMode.FormattingEnabled = true;
            this.cbxMeshMode.Items.AddRange(new object[] {
            "Marching Cubes",
            "Cubes",
            "Cubes Greedy"});
            this.cbxMeshMode.Location = new System.Drawing.Point(15, 244);
            this.cbxMeshMode.Name = "cbxMeshMode";
            this.cbxMeshMode.Size = new System.Drawing.Size(121, 23);
            this.cbxMeshMode.TabIndex = 18;
            //
            // label7
            //
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(15, 226);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(76, 15);
            this.label7.TabIndex = 17;
            this.label7.Text = "Mesh Modus";
            //
            // mtbxInterpolation
            //
            this.mtbxInterpolation.Location = new System.Drawing.Point(15, 156);
            this.mtbxInterpolation.Mask = "90";
            this.mtbxInterpolation.Name = "mtbxInterpolation";
            this.mtbxInterpolation.Size = new System.Drawing.Size(100, 23);
            this.mtbxInterpolation.TabIndex = 16;
            //
            // label6
            //
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(15, 138);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(75, 15);
            this.label6.TabIndex = 15;
            this.label6.Text = "Interpolation";
            //
            // label5
            //
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 182);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 15);
            this.label5.TabIndex = 14;
            this.label5.Text = "Glättung";
            //
            // cbxSmoothing
            //
            this.cbxSmoothing.FormattingEnabled = true;
            this.cbxSmoothing.Items.AddRange(new object[] {
            "Keine",
            "Laplacian Filter 1x",
            "Laplacian Filter 2x",
            "Laplacian Filter 5x",
            "Laplacian Filter 10x",
            "Laplacian Filter HC 1x",
            "Laplacian Filter HC 2x",
            "Laplacian Filter HC 5x",
            "Laplacian Filter HC 10x"});
            this.cbxSmoothing.Location = new System.Drawing.Point(15, 200);
            this.cbxSmoothing.Name = "cbxSmoothing";
            this.cbxSmoothing.Size = new System.Drawing.Size(121, 23);
            this.cbxSmoothing.TabIndex = 13;
            //
            // tbxEast
            //
            this.tbxEast.Location = new System.Drawing.Point(171, 64);
            this.tbxEast.Name = "tbxEast";
            this.tbxEast.Size = new System.Drawing.Size(125, 23);
            this.tbxEast.TabIndex = 12;
            //
            // tbxNorth
            //
            this.tbxNorth.Location = new System.Drawing.Point(15, 64);
            this.tbxNorth.Name = "tbxNorth";
            this.tbxNorth.Size = new System.Drawing.Size(125, 23);
            this.tbxNorth.TabIndex = 11;
            //
            // label2
            //
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(171, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(25, 15);
            this.label2.TabIndex = 9;
            this.label2.Text = "Ost";
            //
            // label4
            //
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 50);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 15);
            this.label4.TabIndex = 10;
            this.label4.Text = "Nord";
            //
            // tbxWest
            //
            this.tbxWest.Location = new System.Drawing.Point(171, 24);
            this.tbxWest.Name = "tbxWest";
            this.tbxWest.Size = new System.Drawing.Size(125, 23);
            this.tbxWest.TabIndex = 8;
            //
            // tbxSouth
            //
            this.tbxSouth.Location = new System.Drawing.Point(15, 24);
            this.tbxSouth.Name = "tbxSouth";
            this.tbxSouth.Size = new System.Drawing.Size(125, 23);
            this.tbxSouth.TabIndex = 7;
            //
            // label3
            //
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(171, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "West";
            //
            // label1
            //
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(27, 15);
            this.label1.TabIndex = 4;
            this.label1.Text = "Süd";
            //
            // trackBar1
            //
            this.trackBar1.Location = new System.Drawing.Point(16, 350);
            this.trackBar1.Maximum = 10000;
            this.trackBar1.Minimum = 1;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(229, 45);
            this.trackBar1.TabIndex = 2;
            this.trackBar1.Value = 1;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            //
            // cmdLoadMap
            //
            this.cmdLoadMap.Location = new System.Drawing.Point(15, 273);
            this.cmdLoadMap.Name = "cmdLoadMap";
            this.cmdLoadMap.Size = new System.Drawing.Size(157, 23);
            this.cmdLoadMap.TabIndex = 0;
            this.cmdLoadMap.Text = "Model aktualisieren";
            this.cmdLoadMap.UseVisualStyleBackColor = true;
            this.cmdLoadMap.Click += new System.EventHandler(this.cmdLoadMap_Click);
            //
            // glControl
            //
            this.glControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.glControl.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.glControl.BackColor = System.Drawing.Color.Black;
            this.glControl.Location = new System.Drawing.Point(4, 3);
            this.glControl.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.glControl.Name = "glControl";
            this.glControl.Size = new System.Drawing.Size(623, 370);
            this.glControl.TabIndex = 1;
            this.glControl.VSync = true;
            this.glControl.Load += new System.EventHandler(this.glControl_Load);
            this.glControl.Scroll += new System.Windows.Forms.ScrollEventHandler(this.glControl_Scroll);
            this.glControl.Paint += new System.Windows.Forms.PaintEventHandler(this.glControl_Paint);
            this.glControl.KeyDown += new System.Windows.Forms.KeyEventHandler(this.glControl_KeyDown);
            this.glControl.MouseMove += new System.Windows.Forms.MouseEventHandler(this.glControl_MouseMove);
            this.glControl.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.glControl_MouseWheel);
            this.glControl.Resize += new System.EventHandler(this.glControl_Resize);
            //
            // Form1
            //
            this.ClientSize = new System.Drawing.Size(971, 499);
            this.Controls.Add(this.splitContainer1);
            this.Name = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load_1);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private GLControl glControl;
        private System.Windows.Forms.Button cmdLoadMap;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbxEast;
        private System.Windows.Forms.TextBox tbxNorth;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbxWest;
        private System.Windows.Forms.TextBox tbxSouth;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbxMeshMode;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.MaskedTextBox mtbxInterpolation;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cbxSmoothing;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnUpdateData;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TrackBar trackBar2;
    }
}
