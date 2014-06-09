using System;
using SuperEngine;
using SuperEngineLib.Maths;
using OpenTK;
using System.Drawing;

namespace SuperEngine {
    class Program {
        struct MyData {
			public Spline<SplineNode4>.SplineSegment ColorSpline;
            public bool Print;
            public int I;
			public MyData(Spline<SplineNode4>.SplineSegment s) {
                ColorSpline = s;
                Print = false;
                I = 0;
            }
			public MyData(Spline<SplineNode4>.SplineSegment s, bool p) {
                ColorSpline = s;
                Print = p;
                I = 0;
            }
        }

        static void Main() {
			Spline<SplineNode2>.SplineSegment s1;
			Spline<SplineNode2>.SplineSegment s2;
			Spline<SplineNode2>.SplineSegment s3;
			Spline<SplineNode2>.SplineSegment s4;

			Spline<SplineNode2>.SplineSegment s5;
			Spline<SplineNode2>.SplineSegment s6;
			Spline<SplineNode2>.SplineSegment s7;
			Spline<SplineNode2>.SplineSegment s8;

			Spline<SplineNode4>.SplineSegment s9;
			Spline<SplineNode4>.SplineSegment s10;

			Spline<SplineNode2>.SplineSegment s11;
			Spline<SplineNode2>.SplineSegment s12;
			Spline<SplineNode2>.SplineSegment s13;
			Spline<SplineNode4>.SplineSegment s14;

            //Console.ReadKey();

            s1 = new Spline<SplineNode2>.SplineSegment(new Vector2(500, 500), new Vector2(500, 1000));
			s2 = new Spline<SplineNode2>.SplineSegment(new Vector2(500, 1000), new Vector2(1000, 1000), s1, null);
			s3 = new Spline<SplineNode2>.SplineSegment(new Vector2(1000, 1000), new Vector2(1000, 500), s2, null);
			s4 = new Spline<SplineNode2>.SplineSegment(new Vector2(1000, 500), new Vector2(500, 500), s3, s1);

			s5 = new Spline<SplineNode2>.SplineSegment(new Vector2(500, 500), new Vector2(500, 1000));
			s6 = new Spline<SplineNode2>.SplineSegment(new Vector2(500, 1000), new Vector2(1000, 1000));
			s7 = new Spline<SplineNode2>.SplineSegment(new Vector2(1000, 1000), new Vector2(1000, 500));
			s8 = new Spline<SplineNode2>.SplineSegment(new Vector2(1000, 500), new Vector2(500, 500));

			s9 = new Spline<SplineNode4>.SplineSegment(new Vector4(255, 255, 0, 0), new Vector4(255, 0, 255, 0));
			s10 = new Spline<SplineNode4>.SplineSegment(new Vector4(255, 0, 0, 255), new Vector4(255, 255, 0, 255));

			s11 = new Spline<SplineNode2>.SplineSegment(new Vector2(0, 500), new Vector2(1500, 500));
			s12 = new Spline<SplineNode2>.SplineSegment(new Vector2(0, -10000), new Vector2(0, 500));
			s13 = new Spline<SplineNode2>.SplineSegment(new Vector2(1500, 500), new Vector2(1500, -10000));

			s14 = new Spline<SplineNode4>.SplineSegment(new Vector4(255, 255, 0, 0), new Vector4(255, 0, 0, 255));

            s11.Prev = s12;
            s11.Next = s13;

            var bmp = new Bitmap(1500, 1500);

            for (int x = 0; x < bmp.Width; x++) {
                for (int y = 0; y < bmp.Height; y++) {
                    bmp.SetPixel(x, y, Color.White);
                }
            }

			Spline<SplineNode2>.SplineSegment.DrawPoint func = delegate(Spline<SplineNode2>.SplineSegment spline, double t, SplineNode2 p, object data) {
                Vector2 vec = p;
                int x = (int)Math.Round(vec.X);
                int y = (int)Math.Round(vec.Y);
                var myData = (MyData)data;
                myData.I++;
                if (myData.Print) {
                    Console.Write("X: ");
                    Console.Write(x);
                    Console.Write(", Y: ");
                    Console.Write(y);
                    Console.WriteLine();
                }
                x = Math.Max(0, Math.Min(x, bmp.Width - 1));
                y = Math.Max(0, Math.Min(y, bmp.Height - 1));
				Spline<SplineNode4>.SplineSegment colorSpline = myData.ColorSpline;
				Vector4 col = colorSpline.Point(t);
				Color color = Color.FromArgb(/*Math.Min(Math.Max(*/(byte)Math.Round(col.X)/*, 255), 0)*/, 
				                             /*Math.Min(Math.Max(*/(byte)Math.Round(col.Y)/*, 255), 0)*/, 
				                             /*Math.Min(Math.Max(*/(byte)Math.Round(col.Z)/*, 255), 0)*/, 
				                             /*Math.Min(Math.Max(*/(byte)Math.Round(col.W)/*, 255), 0)*/);
                bmp.SetPixel(x, y, color);
                if (myData.Print && myData.I % 100 == 0) {
                    //bmp.SetPixel(x, y + 1, color);
                    //bmp.SetPixel(x, y - 1, color);
                    //bmp.SetPixel(x, y + 2, color);
                    //bmp.SetPixel(x, y - 2, color);
                    try {
                        //bmp.Save("out.png");
                    } catch (Exception) {
                    }
                }
            };
			double quality = 1;
			s5.DrawPoints(func, new MyData(s9), quality);
            Console.WriteLine();
			s6.DrawPoints(func, new MyData(s9), quality);
            Console.WriteLine();
			s7.DrawPoints(func, new MyData(s9), quality);
            Console.WriteLine();
			s8.DrawPoints(func, new MyData(s9), quality);
            Console.WriteLine();
            Console.WriteLine();

			s1.DrawPoints(func, new MyData(s10), quality);
            Console.WriteLine();
			s2.DrawPoints(func, new MyData(s10), quality);
            Console.WriteLine();
			s3.DrawPoints(func, new MyData(s10), quality);
            Console.WriteLine();
			s4.DrawPoints(func, new MyData(s10), quality);
            Console.WriteLine();
            Console.WriteLine();

			s11.DrawPoints(func, new MyData(s14, true), quality);

            bmp.Save("out.png");

            //Console.ReadKey();
        }
    }
}