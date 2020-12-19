
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
            this.checkBox1 = new System.Windows.Forms.CheckBox();
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
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.glControl = new OpenTK.GLControl();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.clbScalarselection = new System.Windows.Forms.CheckedListBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbSpecularFactor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbAmbientFactor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbDiffuseFactor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbLightZ)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbLightX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbLightY)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox2);
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            this.splitContainer1.Panel1.Controls.Add(this.checkBox1);
            this.splitContainer1.Panel1.Controls.Add(this.label9);
            this.splitContainer1.Panel1.Controls.Add(this.trackBar2);
            this.splitContainer1.Panel1.Controls.Add(this.label8);
            this.splitContainer1.Panel1.Controls.Add(this.cbxMeshMode);
            this.splitContainer1.Panel1.Controls.Add(this.label7);
            this.splitContainer1.Panel1.Controls.Add(this.mtbxInterpolation);
            this.splitContainer1.Panel1.Controls.Add(this.label6);
            this.splitContainer1.Panel1.Controls.Add(this.label5);
            this.splitContainer1.Panel1.Controls.Add(this.cbxSmoothing);
            this.splitContainer1.Panel1.Controls.Add(this.trackBar1);
            this.splitContainer1.Panel1.Controls.Add(this.cmdLoadMap);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(1308, 797);
            this.splitContainer1.SplitterDistance = 300;
            this.splitContainer1.TabIndex = 1;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(171, 521);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(83, 19);
            this.checkBox1.TabIndex = 23;
            this.checkBox1.Text = "checkBox1";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(19, 718);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(73, 15);
            this.label9.TabIndex = 22;
            this.label9.Text = "CameraNear";
            // 
            // trackBar2
            // 
            this.trackBar2.Location = new System.Drawing.Point(19, 736);
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
            this.label8.Location = new System.Drawing.Point(19, 659);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(64, 15);
            this.label8.TabIndex = 20;
            this.label8.Text = "CameraFar";
            // 
            // btnUpdateData
            // 
            this.btnUpdateData.Location = new System.Drawing.Point(7, 109);
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
            this.cbxMeshMode.Location = new System.Drawing.Point(19, 604);
            this.cbxMeshMode.Name = "cbxMeshMode";
            this.cbxMeshMode.Size = new System.Drawing.Size(121, 23);
            this.cbxMeshMode.TabIndex = 18;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(19, 586);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(76, 15);
            this.label7.TabIndex = 17;
            this.label7.Text = "Mesh Modus";
            // 
            // mtbxInterpolation
            // 
            this.mtbxInterpolation.Location = new System.Drawing.Point(19, 516);
            this.mtbxInterpolation.Mask = "90";
            this.mtbxInterpolation.Name = "mtbxInterpolation";
            this.mtbxInterpolation.Size = new System.Drawing.Size(100, 23);
            this.mtbxInterpolation.TabIndex = 16;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(19, 498);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(75, 15);
            this.label6.TabIndex = 15;
            this.label6.Text = "Interpolation";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(19, 542);
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
            this.cbxSmoothing.Location = new System.Drawing.Point(19, 560);
            this.cbxSmoothing.Name = "cbxSmoothing";
            this.cbxSmoothing.Size = new System.Drawing.Size(121, 23);
            this.cbxSmoothing.TabIndex = 13;
            this.cbxSmoothing.SelectedIndexChanged += new System.EventHandler(this.cbxSmoothing_SelectedIndexChanged);
            // 
            // tbxEast
            // 
            this.tbxEast.Location = new System.Drawing.Point(137, 80);
            this.tbxEast.Name = "tbxEast";
            this.tbxEast.Size = new System.Drawing.Size(125, 23);
            this.tbxEast.TabIndex = 12;
            // 
            // tbxNorth
            // 
            this.tbxNorth.Location = new System.Drawing.Point(6, 80);
            this.tbxNorth.Name = "tbxNorth";
            this.tbxNorth.Size = new System.Drawing.Size(125, 23);
            this.tbxNorth.TabIndex = 11;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(137, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(25, 15);
            this.label2.TabIndex = 9;
            this.label2.Text = "Ost";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 66);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 15);
            this.label4.TabIndex = 10;
            this.label4.Text = "Nord";
            // 
            // tbxWest
            // 
            this.tbxWest.Location = new System.Drawing.Point(137, 40);
            this.tbxWest.Name = "tbxWest";
            this.tbxWest.Size = new System.Drawing.Size(125, 23);
            this.tbxWest.TabIndex = 8;
            // 
            // tbxSouth
            // 
            this.tbxSouth.Location = new System.Drawing.Point(6, 40);
            this.tbxSouth.Name = "tbxSouth";
            this.tbxSouth.Size = new System.Drawing.Size(125, 23);
            this.tbxSouth.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(137, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "West";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(27, 15);
            this.label1.TabIndex = 4;
            this.label1.Text = "Süd";
            // 
            // trackBar1
            // 
            this.trackBar1.Location = new System.Drawing.Point(19, 677);
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
            this.cmdLoadMap.Location = new System.Drawing.Point(19, 633);
            this.cmdLoadMap.Name = "cmdLoadMap";
            this.cmdLoadMap.Size = new System.Drawing.Size(157, 23);
            this.cmdLoadMap.TabIndex = 0;
            this.cmdLoadMap.Text = "Model aktualisieren";
            this.cmdLoadMap.UseVisualStyleBackColor = true;
            this.cmdLoadMap.Click += new System.EventHandler(this.cmdLoadMap_Click);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitContainer2.Location = new System.Drawing.Point(0, -10);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.glControl);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.splitContainer3);
            this.splitContainer2.Size = new System.Drawing.Size(1004, 807);
            this.splitContainer2.SplitterDistance = 600;
            this.splitContainer2.TabIndex = 2;
            // 
            // glControl
            // 
            this.glControl.AutoSize = true;
            this.glControl.BackColor = System.Drawing.Color.Black;
            this.glControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.glControl.Location = new System.Drawing.Point(0, 0);
            this.glControl.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.glControl.Name = "glControl";
            this.glControl.Size = new System.Drawing.Size(1004, 600);
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
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitContainer3.Location = new System.Drawing.Point(0, 16);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.label13);
            this.splitContainer3.Panel1.Controls.Add(this.tbSpecularFactor);
            this.splitContainer3.Panel1.Controls.Add(this.label14);
            this.splitContainer3.Panel1.Controls.Add(this.tbAmbientFactor);
            this.splitContainer3.Panel1.Controls.Add(this.label15);
            this.splitContainer3.Panel1.Controls.Add(this.tbDiffuseFactor);
            this.splitContainer3.Panel1.Controls.Add(this.label12);
            this.splitContainer3.Panel1.Controls.Add(this.tbLightZ);
            this.splitContainer3.Panel1.Controls.Add(this.label11);
            this.splitContainer3.Panel1.Controls.Add(this.tbLightX);
            this.splitContainer3.Panel1.Controls.Add(this.label10);
            this.splitContainer3.Panel1.Controls.Add(this.tbLightY);
            this.splitContainer3.Size = new System.Drawing.Size(1004, 187);
            this.splitContainer3.SplitterDistance = 333;
            this.splitContainer3.TabIndex = 0;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(151, 129);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(83, 15);
            this.label13.TabIndex = 56;
            this.label13.Text = "Specularfaktor";
            // 
            // tbSpecularFactor
            // 
            this.tbSpecularFactor.Location = new System.Drawing.Point(147, 147);
            this.tbSpecularFactor.Maximum = 256000;
            this.tbSpecularFactor.Name = "tbSpecularFactor";
            this.tbSpecularFactor.Size = new System.Drawing.Size(104, 45);
            this.tbSpecularFactor.TabIndex = 53;
            this.tbSpecularFactor.Scroll += new System.EventHandler(this.tbSpecularFactor_Scroll);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(147, 78);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(75, 15);
            this.label14.TabIndex = 54;
            this.label14.Text = "Diffusefaktor";
            // 
            // tbAmbientFactor
            // 
            this.tbAmbientFactor.Location = new System.Drawing.Point(147, 30);
            this.tbAmbientFactor.Maximum = 1000;
            this.tbAmbientFactor.Name = "tbAmbientFactor";
            this.tbAmbientFactor.Size = new System.Drawing.Size(104, 45);
            this.tbAmbientFactor.TabIndex = 51;
            this.tbAmbientFactor.Scroll += new System.EventHandler(this.trackBar7_Scroll);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(147, 12);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(84, 15);
            this.label15.TabIndex = 55;
            this.label15.Text = "Ambientfactor";
            // 
            // tbDiffuseFactor
            // 
            this.tbDiffuseFactor.Location = new System.Drawing.Point(147, 96);
            this.tbDiffuseFactor.Maximum = 1000;
            this.tbDiffuseFactor.Name = "tbDiffuseFactor";
            this.tbDiffuseFactor.Size = new System.Drawing.Size(104, 45);
            this.tbDiffuseFactor.TabIndex = 52;
            this.tbDiffuseFactor.Scroll += new System.EventHandler(this.tbDiffuseFactor_Scroll);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(18, 129);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(60, 15);
            this.label12.TabIndex = 50;
            this.label12.Text = "LightPosZ";
            // 
            // tbLightZ
            // 
            this.tbLightZ.Location = new System.Drawing.Point(14, 147);
            this.tbLightZ.Maximum = 1000;
            this.tbLightZ.Minimum = -1000;
            this.tbLightZ.Name = "tbLightZ";
            this.tbLightZ.Size = new System.Drawing.Size(104, 45);
            this.tbLightZ.TabIndex = 47;
            this.tbLightZ.Scroll += new System.EventHandler(this.trackBar5_Scroll);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(14, 78);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(60, 15);
            this.label11.TabIndex = 48;
            this.label11.Text = "LightPosY";
            // 
            // tbLightX
            // 
            this.tbLightX.Location = new System.Drawing.Point(14, 30);
            this.tbLightX.Maximum = 1000;
            this.tbLightX.Minimum = -1000;
            this.tbLightX.Name = "tbLightX";
            this.tbLightX.Size = new System.Drawing.Size(104, 45);
            this.tbLightX.TabIndex = 45;
            this.tbLightX.Scroll += new System.EventHandler(this.trackBar3_Scroll);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(14, 12);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(60, 15);
            this.label10.TabIndex = 49;
            this.label10.Text = "LightPosX";
            // 
            // tbLightY
            // 
            this.tbLightY.Location = new System.Drawing.Point(14, 96);
            this.tbLightY.Maximum = 2000;
            this.tbLightY.Minimum = -2000;
            this.tbLightY.Name = "tbLightY";
            this.tbLightY.Size = new System.Drawing.Size(104, 45);
            this.tbLightY.TabIndex = 46;
            this.tbLightY.Scroll += new System.EventHandler(this.trackBar4_Scroll);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1322, 831);
            this.tabControl1.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.splitContainer1);
            this.tabPage1.Location = new System.Drawing.Point(4, 24);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1314, 803);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.textBox1);
            this.tabPage2.Controls.Add(this.label16);
            this.tabPage2.Controls.Add(this.comboBox1);
            this.tabPage2.Location = new System.Drawing.Point(4, 24);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1314, 803);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Segoe UI", 5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.textBox1.Location = new System.Drawing.Point(3, 49);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox1.Size = new System.Drawing.Size(1308, 751);
            this.textBox1.TabIndex = 3;
            this.textBox1.WordWrap = false;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(9, 23);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(53, 15);
            this.label16.TabIndex = 2;
            this.label16.Text = "DataGrid";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(68, 20);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(221, 23);
            this.comboBox1.TabIndex = 1;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tbxSouth);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.tbxWest);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.btnUpdateData);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.tbxNorth);
            this.groupBox1.Controls.Add(this.tbxEast);
            this.groupBox1.Location = new System.Drawing.Point(5, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(292, 147);
            this.groupBox1.TabIndex = 24;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Datenset laden";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.clbScalarselection);
            this.groupBox2.Location = new System.Drawing.Point(3, 164);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(294, 251);
            this.groupBox2.TabIndex = 25;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Skalarauswahl";
            // 
            // clbScalarselection
            // 
            this.clbScalarselection.FormattingEnabled = true;
            this.clbScalarselection.Location = new System.Drawing.Point(8, 22);
            this.clbScalarselection.Name = "clbScalarselection";
            this.clbScalarselection.Size = new System.Drawing.Size(280, 220);
            this.clbScalarselection.TabIndex = 0;
            this.clbScalarselection.SelectedIndexChanged += new System.EventHandler(this.clbScalarselection_SelectedIndexChanged);
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(1322, 831);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load_1);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tbSpecularFactor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbAmbientFactor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbDiffuseFactor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbLightZ)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbLightX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbLightY)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
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
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TrackBar tbLightZ;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TrackBar tbLightX;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TrackBar tbLightY;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TrackBar tbSpecularFactor;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TrackBar tbAmbientFactor;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TrackBar tbDiffuseFactor;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckedListBox clbScalarselection;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}
