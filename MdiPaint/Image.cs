using System;
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
    public partial class Image : Form
    {
        private int X, Y;
        Main parentForm;
        public Color penColor;
        Graphics img;
        Bitmap newImg = new Bitmap(200,300);

        public Image(Main parentForm)
        {
            InitializeComponent();
            this.parentForm = parentForm;
            penColor = parentForm.penColor;
        }

        private void draw(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                img = Graphics.FromImage(newImg); // CreateGraphics();
                img.DrawLine(new Pen(penColor),X,Y,e.X,e.Y);
                Invalidate();
                X = e.X;
                Y = e.Y;
            }
            else
            {
            }

            parentForm.changeXY(e.X, e.Y);
        }

        private void leavingForm(object sender, EventArgs e)
        {
            parentForm.changeXY(0,0);
        }

        private void drawStart(object sender, MouseEventArgs e)
        {
            X = e.X;
            Y = e.Y;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            CreateGraphics().DrawImage(newImg, 0, 0); // 0 0 - место на форме с которого срисовывается
        }

        public void SaveAs(string path)
        {
            newImg.Save(path+".bmp");
        }
    }
}
