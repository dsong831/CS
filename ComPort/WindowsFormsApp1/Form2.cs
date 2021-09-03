using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;  // by dSong

namespace WindowsFormsApp1
{
    public partial class Form2 : Form
    {
        Form1 objForm1;
        string _1stDataIn;
        string _2ndDataIn;

        public Form2(Form1 obj)
        {
            InitializeComponent();
            objForm1 = obj;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            button_1stOpen.Enabled = true;
            button_1stClose.Enabled = false;
            button_1stTextSend.Enabled = false;
            comboBox_1stBaudRate.Text = "9600";
            comboBox_1stEndLine.Text = "WriteLine";
            string[] portLists = SerialPort.GetPortNames();
            comboBox_1stComPort.Items.AddRange(portLists);

            button_2ndOpen.Enabled = true;
            button_2ndClose.Enabled = false;
            button_2ndTextSend.Enabled = false;
            comboBox_2ndBaudRate.Text = "9600";
            comboBox_2ndEndLine.Text = "WriteLine";
            comboBox_2ndComPort.Items.AddRange(portLists);
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                try
                {
                    serialPort1.Close();

                    button_1stOpen.Enabled = true;
                    button_1stClose.Enabled = false;
                    button_1stTextSend.Enabled = false;
                    progressBar_1stStatus.Value = 0;
                }
                catch (Exception error)
                {
                    MessageBox.Show(error.Message);
                }
            }

            if (serialPort2.IsOpen)
            {
                try
                {
                    serialPort2.Close();

                    button_2ndOpen.Enabled = true;
                    button_2ndClose.Enabled = false;
                    button_2ndTextSend.Enabled = false;
                    progressBar_2ndStatus.Value = 0;
                }
                catch (Exception error)
                {
                    MessageBox.Show(error.Message);
                }
            }

            objForm1.Show();
        }

        #region 1st COM PORT
        private void button_1stOpen_Click(object sender, EventArgs e)
        {
            try
            {
                serialPort1.PortName = comboBox_1stComPort.Text;
                serialPort1.BaudRate = Convert.ToInt32(comboBox_1stBaudRate.Text);
                serialPort1.Open();

                button_1stOpen.Enabled = false;
                button_1stClose.Enabled = true;
                button_1stTextSend.Enabled = true;
                progressBar_1stStatus.Value = 100;
            }
            catch(Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void button_1stClose_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                try
                {
                    serialPort1.Close();

                    button_1stOpen.Enabled = true;
                    button_1stClose.Enabled = false;
                    button_1stTextSend.Enabled = false;
                    progressBar_1stStatus.Value = 0;
                }
                catch (Exception error)
                {
                    MessageBox.Show(error.Message);
                }
            }
        }

        private void button_1stTextSend_Click(object sender, EventArgs e)
        {
            if(comboBox_1stEndLine.Text == "WriteLine")
            {
                serialPort1.WriteLine(textBox_1stTextSend.Text);
            }
            else if(comboBox_1stEndLine.Text == "Write")
            {
                serialPort1.Write(textBox_1stTextSend.Text);
            }
        }

        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            _1stDataIn = serialPort1.ReadExisting();
            this.Invoke(new EventHandler(_1stShowData));
        }

        private void _1stShowData(object sender, EventArgs e)
        {
            richTextBox_1stTextReceive.Text += _1stDataIn;
        }

        private void richTextBox_1stTextReceive_TextChanged(object sender, EventArgs e)
        {
            richTextBox_1stTextReceive.SelectionStart = richTextBox_1stTextReceive.Text.Length;
            richTextBox_1stTextReceive.ScrollToCaret();
        }
        #endregion

        #region 2nd COM PORT
        private void button_2ndOpen_Click(object sender, EventArgs e)
        {
            try
            {
                serialPort2.PortName = comboBox_2ndComPort.Text;
                serialPort2.BaudRate = Convert.ToInt32(comboBox_2ndBaudRate.Text);
                serialPort2.Open();

                button_2ndOpen.Enabled = false;
                button_2ndClose.Enabled = true;
                button_2ndTextSend.Enabled = true;
                progressBar_2ndStatus.Value = 100;
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void button_2ndClose_Click(object sender, EventArgs e)
        {
            if (serialPort2.IsOpen)
            {
                try
                {
                    serialPort2.Close();

                    button_2ndOpen.Enabled = true;
                    button_2ndClose.Enabled = false;
                    button_2ndTextSend.Enabled = false;
                    progressBar_2ndStatus.Value = 0;
                }
                catch (Exception error)
                {
                    MessageBox.Show(error.Message);
                }
            }
        }

        private void button_2ndTextSend_Click(object sender, EventArgs e)
        {
            if (comboBox_2ndEndLine.Text == "WriteLine")
            {
                serialPort2.WriteLine(textBox_2ndTextSend.Text);
            }
            else if (comboBox_2ndEndLine.Text == "Write")
            {
                serialPort2.Write(textBox_2ndTextSend.Text);
            }
        }

        private void serialPort2_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            _2ndDataIn = serialPort2.ReadExisting();
            this.Invoke(new EventHandler(_2ndShowData));
        }

        private void _2ndShowData(object sender, EventArgs e)
        {
            richTextBox_2ndTextReceive.Text += _2ndDataIn;
        }

        private void richTextBox_2ndTextReceive_TextChanged(object sender, EventArgs e)
        {
            richTextBox_2ndTextReceive.SelectionStart = richTextBox_2ndTextReceive.Text.Length;
            richTextBox_2ndTextReceive.ScrollToCaret();
        }
        #endregion

    }
}
