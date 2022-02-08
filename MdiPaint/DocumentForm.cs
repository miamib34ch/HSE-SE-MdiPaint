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
        private bool localChanged;
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
            if (e.Button == MouseButtons.Left)
            {
                switch (parentForm.tools) 
                {
                    case Tools.Pen:
                        img = Graphics.FromImage(Image);
                        img.DrawLine(new Pen(MainForm.penColor, MainForm.penSize), X, Y, e.X, e.Y);
                        X = e.X;
                        Y = e.Y;
                        Invalidate();
                        parentForm.changed = true;
                        break;
                    case Tools.Eraser:
                        img = Graphics.FromImage(Image);
                        img.FillEllipse(new SolidBrush(Color.White), e.X, e.Y, MainForm.penSize*2, MainForm.penSize*2);
                        Invalidate();
                        parentForm.changed = true;
                        break;
                    case Tools.Line:
                        tmp = new Bitmap(Image.Width, Image.Height);
                        using (var g = Graphics.FromImage(tmp))
                        {
                            g.DrawLine(new Pen(MainForm.penColor, MainForm.penSize), X, Y, e.X, e.Y);
                        }
                        Invalidate();
                        break;
                }
            }
            parentForm.changeXY(e.X, e.Y);
        }

        private void DocumentForm_MouseUp(object sender, MouseEventArgs e)
        {
            if (parentForm.tools == Tools.Line)
            {
                img = Graphics.FromImage(Image);
                img.DrawLine(new Pen(MainForm.penColor, MainForm.penSize), X, Y, e.X, e.Y);
                tmp = new Bitmap(1,1);
                X = e.X;
                Y = e.Y;
                Invalidate();
                parentForm.changed = true;
            }
            if (parentForm.tools == Tools.Ellipse) 
            { 
                img = Graphics.FromImage(Image);
                img.DrawEllipse(new Pen(MainForm.penColor, MainForm.penSize), X, Y, MainForm.penSize, MainForm.penSize);
                Invalidate();
                parentForm.changed = true;
            }
            if (parentForm.tools == Tools.Star)
            {
                img = Graphics.FromImage(Image);
                PointF[] pts = StarPoints(starEnd, new Rectangle(new Point(X,Y),new Size(MainForm.penSize,MainForm.penSize)));
                img.DrawPolygon(new Pen(MainForm.penColor, MainForm.penSize), pts);
                Invalidate();
                parentForm.changed = true;
            }
        }

        private void leavingForm(object sender, EventArgs e)
        {
            parentForm.changeXY(0,0);
        }

        public void Update()
        {
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.DrawImage(Image, 0, 0); // 0 0 - место на форме с которого срисовывается
            if (parentForm.tools == Tools.Line)
                e.Graphics.DrawImage(tmp, 0, 0);
        }

        private void DocumentForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (localChanged)
            {
                var r = MessageBox.Show("Изображение было изменено. Сохранить?", "Сохранение изображения", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
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
            Bitmap tmp = (Bitmap)Image.Clone();
            Image = new Bitmap(parentForm.WidthImage, parentForm.HeightImage);
            img = Graphics.FromImage(Image);
            img.Clear(Color.White);
            for (int Xcount = 0; Xcount <  tmp.Width && Xcount < Image.Width; Xcount++)
            {
                for (int Ycount = 0; Ycount < tmp.Height && Ycount < Image.Height; Ycount++)
                {
                    Image.SetPixel(Xcount, Ycount, tmp.GetPixel(Xcount, Ycount));
                }
            }
            Invalidate();
            parentForm.changed = true;
        } //изменение размера холста

        private PointF[] StarPoints(int num_points, Rectangle bounds)
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

    }
}
