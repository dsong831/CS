using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WFA001_MultiForms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            button1.Text = "Click";
            label1.Text = " ";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label1.Text = "Welcome to my project !!";
        }

        private void label1_Click(object sender, EventArgs e)
        {
            label1.Text = "Introduce new window !!";

            Form form2 = new Form2();
            this.AddOwnedForm(form2);
            form2.Show();
        }
    }
}
