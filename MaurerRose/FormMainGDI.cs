using DirectBitmapLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MaurerRose {
    public partial class FormMainGDI : Form {
        double angle;
        double angleStep = 1.0;
        List<Point> points = new List<Point>();
        Pen p = new Pen(Color.DimGray, 2);

        public FormMainGDI() {
            InitializeComponent();

            this.Text = "Maurer Rose";
            this.Size = new Size(800, 800);
            this.StartPosition = FormStartPosition.CenterScreen;

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.UserPaint, true);

            this.Load += (s, e) => {
                Reset();

                this.Resize += (s, e) => { Reset(); };
                this.Paint += (s, e) => { DrawRose(e.Graphics); };

                Task.Run(() => {
                    while(true) {
                        if(angle <= 360) {
                            Thread.Sleep(5);
                            this.Invalidate();
                        }
                    }
                });
            };
        }

        private void Reset() {
            points.Clear();
            angle = 0.0;
        }

        private void DrawRose(Graphics g) {
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            if(angle <= 360) {
                int n = 6;
                int d = 71;

                int m = 4;

                double w = this.DisplayRectangle.Width / 2;
                double h = this.DisplayRectangle.Height / 2;
                double r = Math.Min(w, h) - m;

                double k = angle * DirectBitmapExtensions.ToRad * d;
                double r1 = r * Math.Sin(n * k);

                double x = w + r1 * Math.Cos(k);
                double y = h - r1 * Math.Sin(k);
                points.Add(new Point((int)x, (int)y));

                angle += angleStep;
            }

            if(points.Count > 1) g.DrawPolygon(p, points.ToArray());
        }
    }
}
