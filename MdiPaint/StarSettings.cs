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
    public partial class StarSettings : Form
    {
        MainForm m;

        public StarSettings(MainForm m)
        {
            InitializeComponent();
            this.m = m;
            textBox1.Text = $"{DocumentForm.starEnd}";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            m.tools = Tools.Star; 
            if(int.TryParse(textBox1.Text, out int a))
            {
                DocumentForm.starEnd = a;
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
    }
}
