using System;
using System.Collections.Generic;
using VisualizeScalars.DataQuery;

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
            
            int oldWidth = self.GetLength(0);
            int oldHeight = self.GetLength(1);
            int newWidth = (int)(oldWidth * scale);
            int newHeight = (int)(oldHeight * scale);
            short[,] newData = new short[newWidth, newHeight];

            for (int x = 0; x < newWidth; x++)
            {
                for (int y = 0; y < newHeight; y++)
                {
                    float gx = ((float)x) / newWidth * (oldWidth - 1);
                    float gy = ((float)y) / newHeight * (oldHeight - 1);
                    int gxi = (int)gx;
                    int gyi = (int)gy;
                    short c00 = self[gxi, gyi];
                    short c10 = self[gxi + 1, gyi];
                    short c01 = self[gxi, gyi + 1];
                    short c11 = self[gxi + 1, gyi + 1];

                    short red = (short)Blerp(c00, c10, c01, c11, gx - gxi, gy - gyi);
                    newData[x, y] = red;
                }
            }

            return newData;

        }
        // RosettaGit/content/drafts/bilinear_interpolation.md
        // https://github.com/ad-si/RosettaGit/blob/master/content/drafts/bilinear_interpolation.md
        public static float[,] Scale(this float[,] self, float scale)
        {
            if (scale <= 1) return self;
            int oldWidth = self.GetLength(0);
            int oldHeight = self.GetLength(1);
            int newWidth = (int)(oldWidth * scale);
            int newHeight = (int)(oldHeight * scale);
            float[,] newData = new float[newWidth, newHeight];

            for (int x = 0; x < newWidth; x++)
            {
                for (int y = 0; y < newHeight; y++)
                {
                    float gx = ((float)x) / newWidth * (oldWidth - 1);
                    float gy = ((float)y) / newHeight * (oldHeight - 1);
                    int gxi = (int)gx;
                    int gyi = (int)gy;
                    float c00 = (float)self[gxi, gyi];
                    float c10 = (float)self[gxi + 1, gyi];
                    float c01 = (float)self[gxi, gyi + 1];
                    float c11 = (float)self[gxi + 1, gyi + 1];

                    float red = Blerp(c00, c10, c01, c11, gx - gxi, gy - gyi);
                    newData[x, y] = red;
                }
            }

            return newData;

        }

        
    }        


}