using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace VisualizeScalars.Helpers
{
    public static class Extensions
    {
        /*public static OpenTK.Vector3 ToVector3(this g3.Vector3d self)
        {
            return new OpenTK.Vector3((float) self.x,(float) self.y,(float) self.z);
        }
        public static g3.Vector3d ToVector3d(this Vector3 self)
        {
            return new g3.Vector3d(self.X, self.Y, self.Z);
        }
        public static g3.Vector3f ToVector3f(this Vector3 self)
        {
            return new g3.Vector3f(self.X, self.Y, self.Z);
        }
        public static Vector3 ToVector3(this g3.Vector3f self)
        {
            return new Vector3(self.x,self.y,self.z);
        }
       
        public static Matrix4 ToMatrix4(this Matrix4x4 matrix4X4)
        {
            var result = new Matrix4();
            result.M11 = matrix4X4.A1;
            result.M12 = matrix4X4.A2;
            result.M13 = matrix4X4.A3;
            result.M14 = matrix4X4.A4;
            result.M21 = matrix4X4.B1;
            result.M22 = matrix4X4.B2;
            result.M23 = matrix4X4.B3;
            result.M24 = matrix4X4.B4;
            result.M31 = matrix4X4.C1;
            result.M32 = matrix4X4.C2;
            result.M33 = matrix4X4.C3;
            result.M34 = matrix4X4.C4;
            result.M41 = matrix4X4.D1;
            result.M42 = matrix4X4.D2;
            result.M43 = matrix4X4.D3;
            result.M44 = matrix4X4.D4;
            return result;
        }
       */
        /// <summary>
        ///     Resize the image to the specified Width and Height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The Width to resize to.</param>
        /// <param name="height">The Height to resize to.</param>
        /// <returns>The resized image.</returns>
        /// https://stackoverflow.com/questions/1922040/how-to-resize-an-image-c-sharp
        public static Image ResizeImage(this Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        private static float Lerp(float s, float e, float t)
        {
            return s + (e - s) * t;
        }

        private static float Blerp(float c00, float c10, float c01, float c11, float tx, float ty)
        {
            return Lerp(Lerp(c00, c10, tx), Lerp(c01, c11, tx), ty);
        }


        public static short[,] Scale(this short[,] self, float scale)
        {
            var oldWidth = self.GetLength(0);
            var oldHeight = self.GetLength(1);
            var newWidth = (int) (oldWidth * scale);
            var newHeight = (int) (oldHeight * scale);
            var newData = new short[newWidth, newHeight];

            for (var x = 0; x < newWidth; x++)
            for (var y = 0; y < newHeight; y++)
            {
                var gx = (float) x / newWidth * (oldWidth - 1);
                var gy = (float) y / newHeight * (oldHeight - 1);
                var gxi = (int) gx;
                var gyi = (int) gy;
                var c00 = self[gxi, gyi];
                var c10 = self[gxi + 1, gyi];
                var c01 = self[gxi, gyi + 1];
                var c11 = self[gxi + 1, gyi + 1];

                var red = (short) Blerp(c00, c10, c01, c11, gx - gxi, gy - gyi);
                newData[x, y] = red;
            }

            return newData;
        }

        // RosettaGit/content/drafts/bilinear_interpolation.md
        // https://github.com/ad-si/RosettaGit/blob/master/content/drafts/bilinear_interpolation.md
        public static float[,] Scale(this float[,] self, float scale)
        {
            if (scale <= 1) return self;
            var oldWidth = self.GetLength(0);
            var oldHeight = self.GetLength(1);
            var newWidth = (int) (oldWidth * scale);
            var newHeight = (int) (oldHeight * scale);
            var newData = new float[newWidth, newHeight];

            for (var x = 0; x < newWidth; x++)
            for (var y = 0; y < newHeight; y++)
            {
                var gx = (float) x / newWidth * (oldWidth - 1);
                var gy = (float) y / newHeight * (oldHeight - 1);
                var gxi = (int) gx;
                var gyi = (int) gy;
                var c00 = self[gxi, gyi];
                var c10 = self[gxi + 1, gyi];
                var c01 = self[gxi, gyi + 1];
                var c11 = self[gxi + 1, gyi + 1];

                var red = Blerp(c00, c10, c01, c11, gx - gxi, gy - gyi);
                newData[x, y] = red;
            }

            return newData;
        }

        public static MinMaxPair GetMinAndMax(this float[,] grid)
        {
            MinMaxPair minmax = new MinMaxPair();
            for (int j = 0; j < grid.GetLength(1); j++)
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                if (minmax.Min > grid[i, j]) minmax.Min = grid[i, j];
                if (minmax.Max < grid[i, j]) minmax.Max = grid[i, j];
            }
            return minmax;
        } 
        public static Bitmap CreateBitmap(this float[,] grid, Color color, int radius,bool isFixedColor = false)
        {
            var height = grid.GetLength(1);
            var width = grid.GetLength(0);
            MinMaxPair minMax = grid.GetMinAndMax();
            Bitmap img = new Bitmap(width, height);
            using Graphics newGraphics = Graphics.FromImage(img);
            newGraphics.FillRectangle(new SolidBrush(Color.Transparent), 0, 0, width, height);
            for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
            {
                var value = (int) ((grid[x, y] - minMax.Min) / (minMax.Max - minMax.Min));
                if (value > 0.0f)
                {
                    if(!isFixedColor)
                    color = Color.FromArgb(value *255, color.R, color.G, color.B);
                    newGraphics.FillEllipse(new SolidBrush(color), x - radius, y - radius, 2 * radius, 2 * radius);
                }
            }

            return img;
        }
    }

    public struct MinMaxPair
    {
        public float Min;
        public float Max;
    }
}