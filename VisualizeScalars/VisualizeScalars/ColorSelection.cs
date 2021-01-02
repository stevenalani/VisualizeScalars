using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenTK;
using VisualizeScalars.Helpers;

namespace VisualizeScalars
{
    public partial class ColorSelection : UserControl
    {
        public float[,] Grid;
        public Color Color => Color.FromArgb((int) (alpha*255),colorDialog1.Color);
        public float alpha => tbAlpha.Value / 100f;
        public int Radius => int.Parse(tbxRadius.Text);
        public ColorSelection()
        {
            InitializeComponent();
            tbxRadius.Text = "1";
        }
        
        public ColorSelection(string name, float[,] grid)
        {

            InitializeComponent();
            this.groupBox1.Text = name;
            this.Grid = grid;
            tbxRadius.Text = "1";
        }


        private void cmdPickColor_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog(this);
            panelColorPreview.BackColor = colorDialog1.Color;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Grid == null) return;
            var bmp = Grid.CreateBitmap(colorDialog1.Color, Radius,cbUseScalarValues.Checked);
            TexturePreviewForm previewForm = new TexturePreviewForm(bmp); 
            previewForm.ShowDialog(this);
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            lbOpacity.Text = (tbAlpha.Value / 100f).ToString();
        }
    }
}
