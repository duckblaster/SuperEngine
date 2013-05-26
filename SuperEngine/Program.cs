using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperEngine;
using SuperEngine.Maths;
using OpenTK;
using System.Drawing;

namespace SuperEngine {
    class Program {
        static void Main(string[] args) {
            Spline s1;
            Spline s2;
            Spline s3;
            Spline s4;
            Spline s5;

            Console.ReadKey();

            s1 = new Spline(new Vector2d(50,50), new Vector2d(50,100));
            s2 = new Spline(new Vector2d(50,100), new Vector2d(100,100), s1, null);
            s3 = new Spline(new Vector2d(100,100), new Vector2d(100,50), s2, null);
            s4 = new Spline(new Vector2d(100, 50), new Vector2d(50, 50), s3, s1);

            s5 = new Spline(new Vector2d(50, 50), new Vector2d(100, 50));

            Bitmap bmp = new Bitmap(150, 150);

            for (int x = 0; x < 150; x++) {
                for (int y = 0; y < 150; y++) {
                    bmp.SetPixel(x, y, Color.White);
                }
            }

            s1.Draw(bmp, Color.Black);
            s2.Draw(bmp, Color.Black);
            s3.Draw(bmp, Color.Black);
            s4.Draw(bmp, Color.Black);

            //s5.Draw(bmp, Color.Turquoise);

            bmp.Save("out.png");

            Console.ReadKey();
        }
    }
}
