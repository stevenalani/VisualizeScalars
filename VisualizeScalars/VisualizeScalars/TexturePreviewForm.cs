using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace VisualizeScalars
{
    public partial class TexturePreviewForm : Form
    {
        public TexturePreviewForm(Bitmap bitmap)
        {
            InitializeComponent();
            this.pictureBox1.Image = bitmap;
            this.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            this.pictureBox1.Update();
        }

    }
}
