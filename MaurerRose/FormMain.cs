using DirectBitmapLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MaurerRose {
    public partial class FormMain : Form {
        private DirectBitmap dbmp;

        public FormMain() {
            InitializeComponent();

            this.Text = "Maurer Rose";
            this.Size = new Size(800, 800);
            this.StartPosition = FormStartPosition.CenterScreen;

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.UserPaint, true);

            this.Load += (s, e) => {
                RebuildBitmap();

                this.Resize += (s, e) => { RebuildBitmap(); };
                this.Paint += (s, e) => { e.Graphics.DrawImageUnscaled(dbmp.Bitmap, 0, 0); };

                Task.Run(() => {
                    while(true) {
                        Task.Delay(30);
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
            int n = 6;
            int d = 71;

            int m = 4;

            double w = dbmp.Width / 2;
            double h = dbmp.Height / 2;
            double r = Math.Min(w, h) - m;

            Point[] p = new Point[361];

            for(int a = 0; a < 361.0; a += 1) {
                double k = a * DirectBitmapExtensions.ToRad * d;
                double r1 = r * Math.Sin(n * k);

                double x = w + r1 * Math.Cos(k);
                double y = h - r1 * Math.Sin(k);
                p[a] = new Point((int)x, (int)y);
            }

            dbmp.DrawPolygon(Color.DimGray, p);
        }
    }
}
