using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperEngine;
using SuperEngineLib.Maths;
using OpenTK;
using System.Drawing;

namespace SuperEngine {
    class Program {
        struct MyData {
            public Spline<SplineNode4> colorSpline;
            public bool print;
            public int i;
            public MyData(Spline<SplineNode4> s) {
                colorSpline = s;
                print = false;
                i = 0;
            }
            public MyData(Spline<SplineNode4> s, bool p) {
                colorSpline = s;
                print = p;
                i = 0;
            }
        }
        static void Main(string[] args) {
            Spline<SplineNode2> s1;
            Spline<SplineNode2> s2;
            Spline<SplineNode2> s3;
            Spline<SplineNode2> s4;

            Spline<SplineNode2> s5;
            Spline<SplineNode2> s6;
            Spline<SplineNode2> s7;
            Spline<SplineNode2> s8;

            Spline<SplineNode4> s9;
            Spline<SplineNode4> s10;

            Spline<SplineNode2> s11;
            Spline<SplineNode2> s12;
            Spline<SplineNode2> s13;
            Spline<SplineNode4> s14;

            //Console.ReadKey();

            s1 = new Spline<SplineNode2>(new Vector2(500, 500), new Vector2(500, 1000));
            s2 = new Spline<SplineNode2>(new Vector2(500, 1000), new Vector2(1000, 1000), s1, null);
            s3 = new Spline<SplineNode2>(new Vector2(1000, 1000), new Vector2(1000, 500), s2, null);
            s4 = new Spline<SplineNode2>(new Vector2(1000, 500), new Vector2(500, 500), s3, s1);

            s5 = new Spline<SplineNode2>(new Vector2(500, 500), new Vector2(500, 1000));
            s6 = new Spline<SplineNode2>(new Vector2(500, 1000), new Vector2(1000, 1000));
            s7 = new Spline<SplineNode2>(new Vector2(1000, 1000), new Vector2(1000, 500));
            s8 = new Spline<SplineNode2>(new Vector2(1000, 500), new Vector2(500, 500));

            s9 = new Spline<SplineNode4>(new Vector4(255, 255, 0, 0), new Vector4(255, 0, 255, 0));
            s10 = new Spline<SplineNode4>(new Vector4(255, 0, 0, 255), new Vector4(255, 255, 0, 255));

            s11 = new Spline<SplineNode2>(new Vector2(0, 500), new Vector2(1500, 500));
            s12 = new Spline<SplineNode2>(new Vector2(0, -10000), new Vector2(0, 500));
            s13 = new Spline<SplineNode2>(new Vector2(1500, 500), new Vector2(1500, -10000));

            s14 = new Spline<SplineNode4>(new Vector4(255, 255, 0, 0), new Vector4(255, 0, 0, 255));

            s11.Prev = s12;
            s11.Next = s13;

            SplineBase.tension = 0.75F;

            Bitmap bmp = new Bitmap(1500, 1500);

            for (int x = 0; x < bmp.Width; x++) {
                for (int y = 0; y < bmp.Height; y++) {
                    bmp.SetPixel(x, y, Color.White);
                }
            }

            Spline<SplineNode2>.DrawPoint func = delegate(Spline<SplineNode2> spline, float t, SplineNode2 p, object data) {
                Vector2 vec = p;
                int x = (int)Math.Floor(vec.X);
                int y = (int)Math.Floor(vec.Y);
                MyData myData = (MyData)data;
                myData.i++;
                if (myData.print) {
                    Console.Write("X: ");
                    Console.Write(x);
                    Console.Write(", Y: ");
                    Console.Write(y);
                    Console.WriteLine();
                }
                x = Math.Max(0, Math.Min(x, bmp.Width - 1));
                y = Math.Max(0, Math.Min(y, bmp.Height - 1));
                Spline<SplineNode4> colorSpline = myData.colorSpline;
                Vector4 col = colorSpline.Point(t);
                Color color = Color.FromArgb((int)Math.Round(col.X), (int)Math.Round(col.Y), (int)Math.Round(col.Z), (int)Math.Round(col.W));
                bmp.SetPixel(x, y, color);
                if (myData.print && myData.i % 100 == 0) {
                    //bmp.SetPixel(x, y + 1, color);
                    //bmp.SetPixel(x, y - 1, color);
                    //bmp.SetPixel(x, y + 2, color);
                    //bmp.SetPixel(x, y - 2, color);
                    try {
                        bmp.Save("out.png");
                    } catch (Exception) {
                    }
                }
            };

            s5.Draw(bmp, func, new MyData(s9));
            Console.WriteLine();
            s6.Draw(bmp, func, new MyData(s9));
            Console.WriteLine();
            s7.Draw(bmp, func, new MyData(s9));
            Console.WriteLine();
            s8.Draw(bmp, func, new MyData(s9));
            Console.WriteLine();
            Console.WriteLine();

            s1.Draw(bmp, func, new MyData(s10));
            Console.WriteLine();
            s2.Draw(bmp, func, new MyData(s10));
            Console.WriteLine();
            s3.Draw(bmp, func, new MyData(s10));
            Console.WriteLine();
            s4.Draw(bmp, func, new MyData(s10));
            Console.WriteLine();
            Console.WriteLine();

            s11.Draw(bmp, func, new MyData(s14, true));

            bmp.Save("out.png");

            //Console.ReadKey();
        }
    }
}