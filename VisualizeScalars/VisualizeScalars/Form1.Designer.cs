
using System.Windows.Forms;
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label17 = new System.Windows.Forms.Label();
            this.cbxScalarYMapping = new System.Windows.Forms.ComboBox();
            this.clbScalarselection = new System.Windows.Forms.CheckedListBox();
            this.mtbxInterpolation = new System.Windows.Forms.MaskedTextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tbxSouth = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tbxWest = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnUpdateData = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.tbxNorth = new System.Windows.Forms.TextBox();
            this.tbxEast = new System.Windows.Forms.TextBox();
            this.cbxMeshMode = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cbxSmoothing = new System.Windows.Forms.ComboBox();
            this.cmdLoadMap = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.cmdCreateTexture = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.dgvScalarTable = new System.Windows.Forms.DataGridView();
            this.label16 = new System.Windows.Forms.Label();
            this.cbxViewScalar = new System.Windows.Forms.ComboBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.flpTextures = new System.Windows.Forms.FlowLayoutPanel();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvScalarTable)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Controls.Add(this.label17);
            this.groupBox2.Controls.Add(this.cbxScalarYMapping);
            this.groupBox2.Controls.Add(this.clbScalarselection);
            this.groupBox2.Controls.Add(this.mtbxInterpolation);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Location = new System.Drawing.Point(3, 155);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(294, 284);
            this.groupBox2.TabIndex = 25;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Skalarauswahl";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(168, 122);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(120, 23);
            this.button1.TabIndex = 28;
            this.button1.Text = "Model laden";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(6, 188);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(107, 15);
            this.label17.TabIndex = 27;
            this.label17.Text = "Skalar Y- Mapping:";
            // 
            // cbxScalarYMapping
            // 
            this.cbxScalarYMapping.FormattingEnabled = true;
            this.cbxScalarYMapping.Location = new System.Drawing.Point(6, 206);
            this.cbxScalarYMapping.Name = "cbxScalarYMapping";
            this.cbxScalarYMapping.Size = new System.Drawing.Size(286, 23);
            this.cbxScalarYMapping.TabIndex = 26;
            this.cbxScalarYMapping.SelectedIndexChanged += new System.EventHandler(this.cbxScalarYMapping_SelectedIndexChanged);
            // 
            // clbScalarselection
            // 
            this.clbScalarselection.FormattingEnabled = true;
            this.clbScalarselection.Location = new System.Drawing.Point(8, 22);
            this.clbScalarselection.Name = "clbScalarselection";
            this.clbScalarselection.Size = new System.Drawing.Size(280, 94);
            this.clbScalarselection.TabIndex = 0;
            // 
            // mtbxInterpolation
            // 
            this.mtbxInterpolation.Location = new System.Drawing.Point(91, 152);
            this.mtbxInterpolation.Mask = "0.9";
            this.mtbxInterpolation.Name = "mtbxInterpolation";
            this.mtbxInterpolation.Size = new System.Drawing.Size(28, 23);
            this.mtbxInterpolation.TabIndex = 16;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 155);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(75, 15);
            this.label6.TabIndex = 15;
            this.label6.Text = "Interpolation";
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
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(292, 146);
            this.groupBox1.TabIndex = 24;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Datenset laden";
            // 
            // tbxSouth
            // 
            this.tbxSouth.Location = new System.Drawing.Point(6, 40);
            this.tbxSouth.Name = "tbxSouth";
            this.tbxSouth.Size = new System.Drawing.Size(125, 23);
            this.tbxSouth.TabIndex = 7;
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
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(137, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "West";
            // 
            // tbxWest
            // 
            this.tbxWest.Location = new System.Drawing.Point(137, 40);
            this.tbxWest.Name = "tbxWest";
            this.tbxWest.Size = new System.Drawing.Size(125, 23);
            this.tbxWest.TabIndex = 8;
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
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(137, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(25, 15);
            this.label2.TabIndex = 9;
            this.label2.Text = "Ost";
            // 
            // tbxNorth
            // 
            this.tbxNorth.Location = new System.Drawing.Point(6, 80);
            this.tbxNorth.Name = "tbxNorth";
            this.tbxNorth.Size = new System.Drawing.Size(125, 23);
            this.tbxNorth.TabIndex = 11;
            // 
            // tbxEast
            // 
            this.tbxEast.Location = new System.Drawing.Point(137, 80);
            this.tbxEast.Name = "tbxEast";
            this.tbxEast.Size = new System.Drawing.Size(125, 23);
            this.tbxEast.TabIndex = 12;
            // 
            // cbxMeshMode
            // 
            this.cbxMeshMode.FormattingEnabled = true;
            this.cbxMeshMode.Items.AddRange(new object[] {
            "Grid Surface",
            "Marching Cubes",
            "Cubes",
            "Cubes Greedy"});
            this.cbxMeshMode.Location = new System.Drawing.Point(15, 693);
            this.cbxMeshMode.Name = "cbxMeshMode";
            this.cbxMeshMode.Size = new System.Drawing.Size(121, 23);
            this.cbxMeshMode.TabIndex = 18;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(15, 675);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(76, 15);
            this.label7.TabIndex = 17;
            this.label7.Text = "Mesh Modus";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 631);
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
            this.cbxSmoothing.Location = new System.Drawing.Point(15, 649);
            this.cbxSmoothing.Name = "cbxSmoothing";
            this.cbxSmoothing.Size = new System.Drawing.Size(121, 23);
            this.cbxSmoothing.TabIndex = 13;
            this.cbxSmoothing.SelectedIndexChanged += new System.EventHandler(this.cbxSmoothing_SelectedIndexChanged);
            // 
            // cmdLoadMap
            // 
            this.cmdLoadMap.Location = new System.Drawing.Point(15, 722);
            this.cmdLoadMap.Name = "cmdLoadMap";
            this.cmdLoadMap.Size = new System.Drawing.Size(157, 23);
            this.cmdLoadMap.TabIndex = 0;
            this.cmdLoadMap.Text = "Model aktualisieren";
            this.cmdLoadMap.UseVisualStyleBackColor = true;
            this.cmdLoadMap.Click += new System.EventHandler(this.cmdLoadMap_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1293, 788);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.flpTextures);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Controls.Add(this.cmdCreateTexture);
            this.tabPage1.Controls.Add(this.cmdLoadMap);
            this.tabPage1.Controls.Add(this.cbxSmoothing);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.checkBox1);
            this.tabPage1.Controls.Add(this.cbxMeshMode);
            this.tabPage1.Controls.Add(this.label7);
            this.tabPage1.Location = new System.Drawing.Point(4, 24);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1285, 760);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // cmdCreateTexture
            // 
            this.cmdCreateTexture.Location = new System.Drawing.Point(199, 722);
            this.cmdCreateTexture.Name = "cmdCreateTexture";
            this.cmdCreateTexture.Size = new System.Drawing.Size(103, 23);
            this.cmdCreateTexture.TabIndex = 0;
            this.cmdCreateTexture.Text = "Textur erstellen";
            this.cmdCreateTexture.UseVisualStyleBackColor = true;
            this.cmdCreateTexture.Click += new System.EventHandler(this.cmdCreateTexture_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(166, 560);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(99, 19);
            this.checkBox1.TabIndex = 23;
            this.checkBox1.Text = "YZ- Anpassen";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.dgvScalarTable);
            this.tabPage2.Controls.Add(this.label16);
            this.tabPage2.Controls.Add(this.cbxViewScalar);
            this.tabPage2.Location = new System.Drawing.Point(4, 24);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1018, 803);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // dgvScalarTable
            // 
            this.dgvScalarTable.AllowUserToAddRows = false;
            this.dgvScalarTable.AllowUserToDeleteRows = false;
            this.dgvScalarTable.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvScalarTable.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvScalarTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvScalarTable.Location = new System.Drawing.Point(9, 49);
            this.dgvScalarTable.Name = "dgvScalarTable";
            this.dgvScalarTable.RowTemplate.Height = 25;
            this.dgvScalarTable.Size = new System.Drawing.Size(1297, 746);
            this.dgvScalarTable.TabIndex = 3;
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
            // cbxViewScalar
            // 
            this.cbxViewScalar.FormattingEnabled = true;
            this.cbxViewScalar.Location = new System.Drawing.Point(68, 20);
            this.cbxViewScalar.Name = "cbxViewScalar";
            this.cbxViewScalar.Size = new System.Drawing.Size(221, 23);
            this.cbxViewScalar.TabIndex = 1;
            this.cbxViewScalar.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            this.cbxViewScalar.Click += new System.EventHandler(this.cbxViewScalar_Click);
            // 
            // tabPage3
            // 
            this.tabPage3.Location = new System.Drawing.Point(4, 24);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(1018, 803);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "tabPage3";
            this.tabPage3.UseVisualStyleBackColor = true;
            this.tabPage3.Click += new System.EventHandler(this.tabPage3_Click);
            // 
            // flpTextures
            // 
            this.flpTextures.AutoScroll = true;
            this.flpTextures.Dock = System.Windows.Forms.DockStyle.Right;
            this.flpTextures.Location = new System.Drawing.Point(308, 3);
            this.flpTextures.Name = "flpTextures";
            this.flpTextures.Size = new System.Drawing.Size(974, 754);
            this.flpTextures.TabIndex = 30;
            // 
            // Form1
            // 
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1293, 788);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load_1);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvScalarTable)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button cmdLoadMap;
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
        private System.Windows.Forms.Button btnUpdateData;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.ComboBox cbxViewScalar;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckedListBox clbScalarselection;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dgvScalarTable;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.ComboBox cbxScalarYMapping;
        private Button button1;
        private TabPage tabPage3;
        private Button cmdCreateTexture;
        private CheckBox checkBox1;
        private FlowLayoutPanel flpTextures;
    }
}
