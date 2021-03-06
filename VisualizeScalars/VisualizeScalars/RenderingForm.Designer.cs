﻿
using System.Windows.Forms;

namespace VisualizeScalars
{
    partial class RenderingForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.glControl = new OpenTK.GLControl();
            this.tbTAlpha = new System.Windows.Forms.TrackBar();
            this.lbMapPosY = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tbMapPosY = new System.Windows.Forms.TrackBar();
            this.lbTextureOffset = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.trackBar2 = new System.Windows.Forms.TrackBar();
            this.tbTextureOffset = new System.Windows.Forms.TrackBar();
            this.label8 = new System.Windows.Forms.Label();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.lLightPosZ = new System.Windows.Forms.Label();
            this.lLightPosY = new System.Windows.Forms.Label();
            this.lLightPosX = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.tbSpecularFactor = new System.Windows.Forms.TrackBar();
            this.label14 = new System.Windows.Forms.Label();
            this.tbAmbientFactor = new System.Windows.Forms.TrackBar();
            this.label15 = new System.Windows.Forms.Label();
            this.tbDiffuseFactor = new System.Windows.Forms.TrackBar();
            this.label12 = new System.Windows.Forms.Label();
            this.tbLightZ = new System.Windows.Forms.TrackBar();
            this.label11 = new System.Windows.Forms.Label();
            this.tbLightX = new System.Windows.Forms.TrackBar();
            this.label10 = new System.Windows.Forms.Label();
            this.tbLightY = new System.Windows.Forms.TrackBar();
            this.ttpPosDetail = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbTAlpha)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbMapPosY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTextureOffset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbSpecularFactor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbAmbientFactor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbDiffuseFactor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbLightZ)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbLightX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbLightY)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.glControl);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tbTAlpha);
            this.splitContainer1.Panel2.Controls.Add(this.lbMapPosY);
            this.splitContainer1.Panel2.Controls.Add(this.label2);
            this.splitContainer1.Panel2.Controls.Add(this.label1);
            this.splitContainer1.Panel2.Controls.Add(this.tbMapPosY);
            this.splitContainer1.Panel2.Controls.Add(this.lbTextureOffset);
            this.splitContainer1.Panel2.Controls.Add(this.label9);
            this.splitContainer1.Panel2.Controls.Add(this.label18);
            this.splitContainer1.Panel2.Controls.Add(this.trackBar2);
            this.splitContainer1.Panel2.Controls.Add(this.tbTextureOffset);
            this.splitContainer1.Panel2.Controls.Add(this.label8);
            this.splitContainer1.Panel2.Controls.Add(this.trackBar1);
            this.splitContainer1.Panel2.Controls.Add(this.lLightPosZ);
            this.splitContainer1.Panel2.Controls.Add(this.lLightPosY);
            this.splitContainer1.Panel2.Controls.Add(this.lLightPosX);
            this.splitContainer1.Panel2.Controls.Add(this.label13);
            this.splitContainer1.Panel2.Controls.Add(this.tbSpecularFactor);
            this.splitContainer1.Panel2.Controls.Add(this.label14);
            this.splitContainer1.Panel2.Controls.Add(this.tbAmbientFactor);
            this.splitContainer1.Panel2.Controls.Add(this.label15);
            this.splitContainer1.Panel2.Controls.Add(this.tbDiffuseFactor);
            this.splitContainer1.Panel2.Controls.Add(this.label12);
            this.splitContainer1.Panel2.Controls.Add(this.tbLightZ);
            this.splitContainer1.Panel2.Controls.Add(this.label11);
            this.splitContainer1.Panel2.Controls.Add(this.tbLightX);
            this.splitContainer1.Panel2.Controls.Add(this.label10);
            this.splitContainer1.Panel2.Controls.Add(this.tbLightY);
            this.splitContainer1.Size = new System.Drawing.Size(1025, 784);
            this.splitContainer1.SplitterDistance = 550;
            this.splitContainer1.TabIndex = 0;
            // 
            // glControl
            // 
            this.glControl.AutoSize = true;
            this.glControl.BackColor = System.Drawing.Color.Black;
            this.glControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.glControl.Location = new System.Drawing.Point(0, 0);
            this.glControl.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.glControl.Name = "glControl";
            this.glControl.Size = new System.Drawing.Size(1025, 550);
            this.glControl.TabIndex = 2;
            this.glControl.VSync = true;
            this.glControl.Load += new System.EventHandler(this.glControl_Load);
            this.glControl.Click += new System.EventHandler(this.glControl_Click);
            this.glControl.Paint += new System.Windows.Forms.PaintEventHandler(this.glControl_Paint);
            this.glControl.DoubleClick += new System.EventHandler(this.glControl_DoubleClick_1);
            this.glControl.KeyDown += new System.Windows.Forms.KeyEventHandler(this.glControl_KeyDown);
            this.glControl.MouseMove += new System.Windows.Forms.MouseEventHandler(this.glControl_MouseMove);
            this.glControl.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.glControl_MouseWheel);
            this.glControl.Resize += new System.EventHandler(this.glControl_Resize);
            // 
            // tbTAlpha
            // 
            this.tbTAlpha.Location = new System.Drawing.Point(546, 152);
            this.tbTAlpha.Maximum = 100;
            this.tbTAlpha.Minimum = 1;
            this.tbTAlpha.Name = "tbTAlpha";
            this.tbTAlpha.Size = new System.Drawing.Size(218, 45);
            this.tbTAlpha.TabIndex = 82;
            this.tbTAlpha.Value = 75;
            this.tbTAlpha.Scroll += new System.EventHandler(this.tbTAlpha_Scroll);
            // 
            // lbMapPosY
            // 
            this.lbMapPosY.AutoSize = true;
            this.lbMapPosY.Location = new System.Drawing.Point(751, 88);
            this.lbMapPosY.Name = "lbMapPosY";
            this.lbMapPosY.Size = new System.Drawing.Size(13, 15);
            this.lbMapPosY.TabIndex = 81;
            this.lbMapPosY.Text = "0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(546, 134);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 15);
            this.label2.TabIndex = 80;
            this.label2.Text = "Terrain- Alpha";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(546, 68);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 15);
            this.label1.TabIndex = 80;
            this.label1.Text = "Karten- Offset";
            // 
            // tbMapPosY
            // 
            this.tbMapPosY.Location = new System.Drawing.Point(546, 86);
            this.tbMapPosY.Maximum = 20000;
            this.tbMapPosY.Minimum = -20000;
            this.tbMapPosY.Name = "tbMapPosY";
            this.tbMapPosY.Size = new System.Drawing.Size(199, 45);
            this.tbMapPosY.TabIndex = 79;
            this.tbMapPosY.Scroll += new System.EventHandler(this.tbMapPosY_Scroll);
            // 
            // lbTextureOffset
            // 
            this.lbTextureOffset.AutoSize = true;
            this.lbTextureOffset.Location = new System.Drawing.Point(751, 27);
            this.lbTextureOffset.Name = "lbTextureOffset";
            this.lbTextureOffset.Size = new System.Drawing.Size(13, 15);
            this.lbTextureOffset.TabIndex = 32;
            this.lbTextureOffset.Text = "1";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(305, 68);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(73, 15);
            this.label9.TabIndex = 78;
            this.label9.Text = "CameraNear";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(546, 9);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(89, 15);
            this.label18.TabIndex = 31;
            this.label18.Text = "Textur Y- Offset";
            // 
            // trackBar2
            // 
            this.trackBar2.Location = new System.Drawing.Point(305, 86);
            this.trackBar2.Maximum = 10000;
            this.trackBar2.Minimum = 1;
            this.trackBar2.Name = "trackBar2";
            this.trackBar2.Size = new System.Drawing.Size(229, 45);
            this.trackBar2.TabIndex = 77;
            this.trackBar2.Value = 9999;
            this.trackBar2.Scroll += new System.EventHandler(this.trackBar2_Scroll);
            // 
            // tbTextureOffset
            // 
            this.tbTextureOffset.Location = new System.Drawing.Point(546, 27);
            this.tbTextureOffset.Maximum = 100000;
            this.tbTextureOffset.Minimum = -1000;
            this.tbTextureOffset.Name = "tbTextureOffset";
            this.tbTextureOffset.Size = new System.Drawing.Size(199, 45);
            this.tbTextureOffset.TabIndex = 30;
            this.tbTextureOffset.Value = 1;
            this.tbTextureOffset.Scroll += new System.EventHandler(this.tbTextureOffset_Scroll);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(305, 9);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(64, 15);
            this.label8.TabIndex = 76;
            this.label8.Text = "CameraFar";
            // 
            // trackBar1
            // 
            this.trackBar1.Location = new System.Drawing.Point(305, 27);
            this.trackBar1.Maximum = 10000;
            this.trackBar1.Minimum = 10;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(229, 45);
            this.trackBar1.TabIndex = 75;
            this.trackBar1.Value = 10000;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // lLightPosZ
            // 
            this.lLightPosZ.AutoSize = true;
            this.lLightPosZ.Location = new System.Drawing.Point(123, 173);
            this.lLightPosZ.Name = "lLightPosZ";
            this.lLightPosZ.Size = new System.Drawing.Size(44, 15);
            this.lLightPosZ.TabIndex = 74;
            this.lLightPosZ.Text = "label19";
            // 
            // lLightPosY
            // 
            this.lLightPosY.AutoSize = true;
            this.lLightPosY.Location = new System.Drawing.Point(122, 101);
            this.lLightPosY.Name = "lLightPosY";
            this.lLightPosY.Size = new System.Drawing.Size(44, 15);
            this.lLightPosY.TabIndex = 73;
            this.lLightPosY.Text = "label19";
            // 
            // lLightPosX
            // 
            this.lLightPosX.AutoSize = true;
            this.lLightPosX.Location = new System.Drawing.Point(122, 40);
            this.lLightPosX.Name = "lLightPosX";
            this.lLightPosX.Size = new System.Drawing.Size(44, 15);
            this.lLightPosX.TabIndex = 72;
            this.lLightPosX.Text = "label19";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(180, 136);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(83, 15);
            this.label13.TabIndex = 71;
            this.label13.Text = "Specularfaktor";
            // 
            // tbSpecularFactor
            // 
            this.tbSpecularFactor.Location = new System.Drawing.Point(180, 154);
            this.tbSpecularFactor.Maximum = 256000;
            this.tbSpecularFactor.Minimum = 32;
            this.tbSpecularFactor.Name = "tbSpecularFactor";
            this.tbSpecularFactor.Size = new System.Drawing.Size(104, 45);
            this.tbSpecularFactor.TabIndex = 68;
            this.tbSpecularFactor.Value = 32;
            this.tbSpecularFactor.Scroll += new System.EventHandler(this.tbSpecularFactor_Scroll);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(180, 70);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(75, 15);
            this.label14.TabIndex = 69;
            this.label14.Text = "Diffusefaktor";
            // 
            // tbAmbientFactor
            // 
            this.tbAmbientFactor.Location = new System.Drawing.Point(180, 27);
            this.tbAmbientFactor.Maximum = 1000;
            this.tbAmbientFactor.Name = "tbAmbientFactor";
            this.tbAmbientFactor.Size = new System.Drawing.Size(104, 45);
            this.tbAmbientFactor.TabIndex = 66;
            this.tbAmbientFactor.Scroll += new System.EventHandler(this.tbAmbientFactor_Scroll);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(180, 9);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(84, 15);
            this.label15.TabIndex = 70;
            this.label15.Text = "Ambientfactor";
            // 
            // tbDiffuseFactor
            // 
            this.tbDiffuseFactor.Location = new System.Drawing.Point(180, 88);
            this.tbDiffuseFactor.Maximum = 1000;
            this.tbDiffuseFactor.Name = "tbDiffuseFactor";
            this.tbDiffuseFactor.Size = new System.Drawing.Size(104, 45);
            this.tbDiffuseFactor.TabIndex = 67;
            this.tbDiffuseFactor.Value = 500;
            this.tbDiffuseFactor.Scroll += new System.EventHandler(this.tbDiffuseFactor_Scroll);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(13, 136);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(60, 15);
            this.label12.TabIndex = 65;
            this.label12.Text = "LightPosZ";
            // 
            // tbLightZ
            // 
            this.tbLightZ.Location = new System.Drawing.Point(13, 152);
            this.tbLightZ.Maximum = 5000;
            this.tbLightZ.Minimum = -5000;
            this.tbLightZ.Name = "tbLightZ";
            this.tbLightZ.Size = new System.Drawing.Size(104, 45);
            this.tbLightZ.TabIndex = 62;
            this.tbLightZ.Scroll += new System.EventHandler(this.tbLightZ_Scroll);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(12, 70);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(60, 15);
            this.label11.TabIndex = 63;
            this.label11.Text = "LightPosY";
            // 
            // tbLightX
            // 
            this.tbLightX.Location = new System.Drawing.Point(13, 27);
            this.tbLightX.Maximum = 5000;
            this.tbLightX.Minimum = -5000;
            this.tbLightX.Name = "tbLightX";
            this.tbLightX.Size = new System.Drawing.Size(104, 45);
            this.tbLightX.TabIndex = 60;
            this.tbLightX.Scroll += new System.EventHandler(this.tbLightX_Scroll);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(13, 9);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(60, 15);
            this.label10.TabIndex = 64;
            this.label10.Text = "LightPosX";
            // 
            // tbLightY
            // 
            this.tbLightY.Location = new System.Drawing.Point(12, 88);
            this.tbLightY.Maximum = 2000;
            this.tbLightY.Minimum = -2000;
            this.tbLightY.Name = "tbLightY";
            this.tbLightY.Size = new System.Drawing.Size(104, 45);
            this.tbLightY.TabIndex = 61;
            this.tbLightY.Scroll += new System.EventHandler(this.tbLightY_Scroll);
            // 
            // RenderingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1025, 784);
            this.Controls.Add(this.splitContainer1);
            this.Name = "RenderingForm";
            this.Text = "RenderingForm";
            this.Load += new System.EventHandler(this.RenderingView_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tbTAlpha)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbMapPosY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTextureOffset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbSpecularFactor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbAmbientFactor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbDiffuseFactor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbLightZ)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbLightX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbLightY)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        internal OpenTK.GLControl glControl;
        private System.Windows.Forms.Label lLightPosZ;
        private System.Windows.Forms.Label lLightPosY;
        private System.Windows.Forms.Label lLightPosX;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TrackBar tbSpecularFactor;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TrackBar tbAmbientFactor;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TrackBar tbDiffuseFactor;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TrackBar tbLightZ;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TrackBar tbLightX;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TrackBar tbLightY;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TrackBar trackBar2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TrackBar trackBar1;
        private Label lbTextureOffset;
        private Label label18;
        private TrackBar tbTextureOffset;
        private Label lbMapPosY;
        private Label label1;
        private TrackBar tbMapPosY;
        internal ToolTip ttpPosDetail;
        private TrackBar tbTAlpha;
        private Label label2;
    }
}