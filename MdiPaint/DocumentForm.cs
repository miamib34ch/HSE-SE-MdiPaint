using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MdiPaint
{
    public partial class DocumentForm : Form
    {
        private int X, Y;
        private MainForm parentForm;
        private Graphics img;
        public bool localChanged;
        public Bitmap Image { get; set; }
        private Bitmap tmp { get; set; }
        public static int starEnd { get; set; }

        public DocumentForm(MainForm parentForm)
        {
            InitializeComponent();
            this.parentForm = parentForm;
            Image = new Bitmap(parentForm.WidthImage, parentForm.HeightImage);
            tmp = new Bitmap(Image.Width, Image.Height);
            img = Graphics.FromImage(Image);
            img.Clear(Color.White);
            localChanged = true;
            starEnd = 5;
            this.Text = "Image" + $"{parentForm.count}";
        }

        public DocumentForm(MainForm parentForm, string filename)
        {
            InitializeComponent();
            this.parentForm = parentForm;
            using (Stream s = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                Image = new Bitmap(s);
            }
            tmp = new Bitmap(Image.Width, Image.Height);
            this.Text = filename; 
            localChanged = false;
            starEnd = 5;
        }

        private void drawStart(object sender, MouseEventArgs e)
        {
            X = e.X;
            Y = e.Y;
        }

        private void draw(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Left)
                {
                    switch (parentForm.tools)
                    {
                        case Tools.Pen:
                            img = Graphics.FromImage(Image);
                            var pen = new Pen(MainForm.penColor, MainForm.penSize);
                            pen.StartCap = LineCap.Round;
                            pen.EndCap = LineCap.Round;
                            img.DrawLine(pen, X, Y, e.X, e.Y);
                            X = e.X;
                            Y = e.Y;
                            Invalidate();
                            parentForm.changed = true;
                            localChanged = true;
                            break;
                        case Tools.Eraser:
                            img = Graphics.FromImage(Image);
                            var pen2 = new Pen(Color.White, MainForm.penSize);
                            pen2.StartCap = LineCap.Round;
                            pen2.EndCap = LineCap.Round;
                            img.DrawLine(pen2, X, Y, e.X, e.Y);
                            X = e.X;
                            Y = e.Y;
                            Invalidate();
                            parentForm.changed = true;
                            localChanged = true;
                            break;
                        case Tools.Line:
                            tmp = new Bitmap(Image.Width, Image.Height);
                            using (var g = Graphics.FromImage(tmp))
                            {
                                g.DrawLine(new Pen(MainForm.penColor, MainForm.penSize), X, Y, e.X, e.Y);
                            }
                            Invalidate();
                            break;
                        case Tools.Ellipse:
                            tmp = new Bitmap(Image.Width, Image.Height);
                            using (var g = Graphics.FromImage(tmp))
                            {
                                g.DrawEllipse(new Pen(MainForm.penColor, MainForm.penSize), X, Y, e.X - X, e.Y - Y);
                            }
                            Invalidate();
                            break;
                        case Tools.Star:
                            tmp = new Bitmap(Image.Width, Image.Height);
                            PointF[] pts = StarPoints(starEnd, new Rectangle(new Point(X, Y), new Size(e.X - X, e.Y - Y)));
                            using (var g = Graphics.FromImage(tmp))
                            {
                                g.DrawPolygon(new Pen(MainForm.penColor, MainForm.penSize), pts);
                            }
                            Invalidate();
                            break;
                    }
                }
                parentForm.changeXY(e.X, e.Y);
            }
            catch
            {

            }
        }

        private void DocumentForm_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (parentForm.tools == Tools.Line)
                {
                    img = Graphics.FromImage(Image);
                    img.DrawLine(new Pen(MainForm.penColor, MainForm.penSize), X, Y, e.X, e.Y);
                    tmp = new Bitmap(1, 1);
                    Invalidate();
                    parentForm.changed = true;
                    localChanged = true;
                }
                if (parentForm.tools == Tools.Ellipse)
                {
                    img = Graphics.FromImage(Image);
                    img.DrawEllipse(new Pen(MainForm.penColor, MainForm.penSize), X, Y, e.X - X, e.Y - Y);
                    tmp = new Bitmap(1, 1);
                    Invalidate();
                    parentForm.changed = true;
                    localChanged = true;
                }
                if (parentForm.tools == Tools.Star)
                {
                    img = Graphics.FromImage(Image);
                    PointF[] pts = StarPoints(starEnd, new Rectangle(new Point(X, Y), new Size(e.X - X, e.Y - Y)));
                    img.DrawPolygon(new Pen(MainForm.penColor, MainForm.penSize), pts);
                    tmp = new Bitmap(1, 1);
                    Invalidate();
                    parentForm.changed = true;
                    localChanged = true;
                }
            }
            catch { }
        }

        private void leavingForm(object sender, EventArgs e)
        {
            parentForm.changeXY(0,0);
        }

        public void newUpdate()
        {
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.DrawImage(Image, 0, 0); // 0 0 - место на форме с которого срисовывается
            if (parentForm.tools == Tools.Line || parentForm.tools == Tools.Ellipse || parentForm.tools == Tools.Star)
                e.Graphics.DrawImage(tmp, 0, 0);
        }

        private void DocumentForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (localChanged)
            {
                var r = MessageBox.Show($"Изображение {this.Text} было изменено. Сохранить?", "Сохранение изображения", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                switch (r)
                {
                    case DialogResult.Cancel:
                        e.Cancel = true;
                        break;
                    case DialogResult.Yes:
                        parentForm.сохранитьToolStripMenuItem_Click(sender, e);
                        break;
                }
            }
            if (parentForm.MdiChildren.Length == 1)
                parentForm.changed = false;
        } //предложение сохранить при закрытии дочернего окна

        public void changeSize()
        {
            try
            {
                Bitmap tmp = (Bitmap)Image.Clone();
                Image = new Bitmap(parentForm.WidthImage, parentForm.HeightImage);
                img = Graphics.FromImage(Image);
                img.Clear(Color.White);
                for (int Xcount = 0; Xcount < tmp.Width && Xcount < Image.Width; Xcount++)
                {
                    for (int Ycount = 0; Ycount < tmp.Height && Ycount < Image.Height; Ycount++)
                    {
                        Image.SetPixel(Xcount, Ycount, tmp.GetPixel(Xcount, Ycount));
                    }
                }
                Invalidate();
                parentForm.changed = true;
                localChanged = true;
            }
            catch { }
        } //изменение размера холста

        private PointF[] StarPoints(int num_points, Rectangle bounds)
        {
            try
            {
                PointF[] pts = new PointF[num_points];

                double rx = bounds.Width / 2;
                double ry = bounds.Height / 2;
                double cx = bounds.X + rx;
                double cy = bounds.Y + ry;

                double theta = -Math.PI / 2;
                double dtheta = 4 * Math.PI / num_points;
                for (int i = 0; i < num_points; i++)
                {
                    pts[i] = new PointF(
                        (float)(cx + rx * Math.Cos(theta)),
                        (float)(cy + ry * Math.Sin(theta)));
                    theta += dtheta;
                }

                return pts;
            }
            catch
            {
                return null;
            }
        }

    }
}
