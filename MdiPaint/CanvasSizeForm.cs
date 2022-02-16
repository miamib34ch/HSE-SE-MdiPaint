using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MdiPaint
{
    public partial class CanvasSizeForm : Form
    {

        MainForm mainForm;

        public CanvasSizeForm(MainForm m)
        {
            InitializeComponent();
            mainForm = m;
            textBox1.Text = $"{mainForm.WidthImage}";
            textBox2.Text = $"{mainForm.HeightImage}";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (int.TryParse(textBox1.Text, out int Width) && int.TryParse(textBox2.Text, out int Height))
            {
                if (Width > 0 && Height > 0) {
                    mainForm.WidthImage = Width;
                    mainForm.HeightImage = Height; }
            }
            else
            {
                
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(textBox1.Text, out int w) || textBox1.Text == "")
            {
                if (w <= 0 && textBox1.Text != "")
                {
                    MessageBox.Show("Вы ввели отрицательное число, введите положительное!");
                    textBox1.Clear();
                }
            }
            else
            {
                MessageBox.Show("Вы ввели символ, вводите цифры!");
                textBox1.Clear();
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(textBox2.Text, out int w) || textBox2.Text == "")
            {
                if (w <= 0 && textBox2.Text != "")
                {
                    MessageBox.Show("Вы ввели отрицательное число, введите положительное!");
                    textBox1.Clear();
                }
            }
            else
            {
                MessageBox.Show("Вы ввели символ, вводите цифры!");
                textBox2.Clear();
            }
        }
    }
}
