using MorphxLibs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MaurerRose {
    public partial class FormMainDirectBitmap : Form {
        private DirectBitmap dbmp;

        private double angleStep = 1.0;
        private int n = 5;
        private int d = 97;
        private int m = 4; // Margin

        public FormMainDirectBitmap() {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.OptimizedDoubleBuffer |
                          ControlStyles.UserPaint, true);

            this.Load += (s, e) => {
                RebuildBitmap();

                this.Resize += (s, e) => { RebuildBitmap(); };
                this.Paint += (s, e) => {
                    Graphics g = e.Graphics;
                    g.DrawImageUnscaled(dbmp.Bitmap, 0, 0);

                    int fh = this.Font.Height;
                    g.DrawString($"Angle Step: {angleStep:F2}", this.Font, Brushes.Black, 0, fh * 0);
                    g.DrawString($"n: {n}", this.Font, Brushes.Black, 0, fh * 1);
                    g.DrawString($"d: {d}", this.Font, Brushes.Black, 0, fh * 2);
                };

                Task.Run(() => {
                    while(true) {
                        Thread.Sleep(30);
                        this.Invalidate();
                    }
                });
            };
        }

        private void RebuildBitmap() {
            dbmp = new DirectBitmap(this);
            dbmp.Clear(Color.Gainsboro);

            BuildRose();
        }

        private void BuildRose() {
            double w = dbmp.Width / 2;
            double h = dbmp.Height / 2;
            double r = Math.Min(w, h) - m;

            List<Point> p = new List<Point>();

            for(double a = 0; a <= 360.0; a += angleStep) {
                double k = a * Constants.ToRad * d;
                double r1 = r * Math.Sin(n * k);

                double x = w + r1 * Math.Cos(k);
                double y = h - r1 * Math.Sin(k);
                p.Add(new Point((int)x, (int)y));
            }

            dbmp.DrawPolygon(Color.DimGray, p.ToArray());
        }
    }
}
