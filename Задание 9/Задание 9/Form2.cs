using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Задание_9
{
    public partial class Form2 : Form
    {
        public event EventHandler<NameEnteredEventArgs> NameEntered;
        private string Nickname = "Гость";
        public static bool flag = false;
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            flag = true;
            if (textBox1.Text == string.Empty)
            {
                MessageBox.Show("Поле не может быть пустым");
                return;
            }   
            Nickname = textBox1.Text;
            eh();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            eh();
        }
        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            eh();
        }
        private void eh()
        {
            NameEntered?.Invoke(this, new NameEnteredEventArgs(Nickname));
            Close();
        }
    }
    public class NameEnteredEventArgs : EventArgs
    {
        public string Name { get; }

        public NameEnteredEventArgs(string name)
        {
            Name = name;
        }
    }
}
