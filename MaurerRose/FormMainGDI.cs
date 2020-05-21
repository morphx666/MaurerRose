using MorphxLibs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MaurerRose {
    public partial class FormMainGDI : Form {
        private double angle;
        private List<Point> points = new List<Point>();
        private Pen p = new Pen(Color.DimGray, 2);

        private double angleStep = 1.0;
        private int n = 6;
        private int d = 71;
        private int m = 4; // Margin

        public FormMainGDI() {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint | 
                          ControlStyles.OptimizedDoubleBuffer | 
                          ControlStyles.UserPaint, true);

            this.Load += (s, e) => {
                Reset();

                this.Resize += (s, e) => { Reset(); };
                this.Paint += (s, e) => { DrawRose(e.Graphics); };
                this.KeyDown += HandleKeyDown;

                Task.Run(() => {
                    while(true) {
                        double w = this.DisplayRectangle.Width / 2;
                        double h = this.DisplayRectangle.Height / 2;
                        double r = Math.Min(w, h) - m;

                        while((angle += angleStep) <= 360.0) {
                            double k = angle * Constants.ToRad * d;
                            double r1 = r * Math.Sin(n * k);

                            double x = w + r1 * Math.Cos(k);
                            double y = h - r1 * Math.Sin(k);
                            points.Add(new Point((int)x, (int)y));

                            //Thread.Sleep(1);
                        }
                        this.Invalidate();

                        Thread.Sleep(10);
                    }
                });
            };
        }

        private void HandleKeyDown(object sender, KeyEventArgs e) {
            switch(e.KeyCode) {
                case Keys.A:
                    angleStep += 0.5 * (e.Shift ? -1 : 1);
                    if(angleStep < 0.0)
                        angleStep = 0;
                    else if(angleStep > 90.0)
                        angleStep = 90.0;
                    Reset();
                    break;
                case Keys.N:
                    n += 1 * (e.Shift ? -1 : 1);
                    Reset();
                    break;
                case Keys.D:
                    d += 1 * (e.Shift ? -1 : 1);
                    Reset();
                    break;
            }
        }

        private void Reset() {
            points.Clear();
            angle = 0.0;
        }

        private void DrawRose(Graphics g) {
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            if(points.Count > 1) g.DrawPolygon(p, points.ToArray());

            int fh = this.Font.Height;
            g.DrawString($"Angle Step: {angleStep:F2}", this.Font, Brushes.Black, 0, fh * 0);
            g.DrawString($"n: {n}", this.Font, Brushes.Black, 0, fh * 1);
            g.DrawString($"d: {d}", this.Font, Brushes.Black, 0, fh * 2);
        }
    }
}