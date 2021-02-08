using System;
using System.Drawing;
using System.Windows.Forms;
using VisualizeScalars.Helpers;

namespace VisualizeScalars
{
    public partial class ColorSelection : UserControl
    {
        public float[,] Grid;
        public Color[] Colors
        {
            get
            {
                if (colorDialog2.Color == Color.Transparent)
                {
                    return new[]
                    {
                        System.Drawing.Color.FromArgb((int) (alpha * 255), colorDialog1.Color),
                        Color.Transparent, 
                    };
                }
                else
                {
                    return new[]
                    {
                        System.Drawing.Color.FromArgb((int)(alpha * 255), colorDialog1.Color),
                        System.Drawing.Color.FromArgb((int)(alpha * 255), colorDialog2.Color),
                    };
                }
            }
        }

        public float alpha => tbAlpha.Value / 100f;
        public int Radius => int.Parse(tbxRadius.Text);
        public ColorSelection()
        {
            InitializeComponent();
            tbxRadius.Text = "1";
            colorDialog1.Color = colorDialog2.Color = Color.Transparent;
            
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
        private void button2_Click(object sender, EventArgs e)
        {
            colorDialog2.ShowDialog(this);
            panelColorPreview2.BackColor = colorDialog2.Color;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Grid == null) return;
            var bmp = Grid.CreateBitmap(Colors, Radius,!cbUseScalarValues.Checked);
            TexturePreviewForm previewForm = new TexturePreviewForm(bmp); 
            previewForm.ShowDialog(this);
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            lbOpacity.Text = (tbAlpha.Value / 100f).ToString();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
