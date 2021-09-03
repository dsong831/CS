using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;       // by dSong
using System.IO;             // by dSong
using System.Windows.Forms.DataVisualization.Charting;  // by dSong

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        string dataOut;     // by dSong
        string sendWidth;   // by dSong
        string dataIn;      // by dSong
        int tickCount;      // by dSong
        int xData, yData;   // by dSong

        StreamWriter objStreamWriter;
        string pathFile;
        bool state_AppendText = true;

        #region My Own Method
        private void SaveDataToTxtFile()
        {
            if(saveToTextFileToolStripMenuItem.Checked)
            {
                try
                {
                    objStreamWriter = new StreamWriter(pathFile, state_AppendText);
                    if (toolStripComboBox_writeLineOrwriteText.Text == "WriteLine")
                    {
                        objStreamWriter.WriteLine(dataIn);
                    }
                    else if (toolStripComboBox_writeLineOrwriteText.Text == "Write")
                    {
                        objStreamWriter.Write(dataIn + " ");
                    }
                    objStreamWriter.Close();
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        #endregion

        #region GUI Method
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string[] ports = SerialPort.GetPortNames();
            cBoxComPort.Items.AddRange(ports);

            chBoxDtrEnable.Checked = false;
            serialPort1.DtrEnable = false;
            chBoxRtsEnable.Checked = false;
            serialPort1.RtsEnable = false;
            btnSendData.Enabled = true;
            sendWidth = "Both";
            toolStripComboBox3.Text = "BOTTOM";

            toolStripComboBox1.Text = "Add Old Data";
            toolStripComboBox2.Text = "Both";

            toolStripComboBox_appendOrOverwriteText.Text = "Append Text";
            toolStripComboBox_writeLineOrwriteText.Text = "WriteLine";

            pathFile = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
            pathFile += @"\_My Source File\SerialData.txt";

            saveToTextFileToolStripMenuItem.Checked = false;

            chart1.Series[0].ChartType = SeriesChartType.Line;
        }

        private void oPENToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                serialPort1.PortName = cBoxComPort.Text;
                serialPort1.BaudRate = Convert.ToInt32(cBoxBaudRate.Text);
                serialPort1.DataBits = Convert.ToInt32(cBoxDataBits.Text);
                serialPort1.StopBits = (StopBits)Enum.Parse(typeof(StopBits), cBoxStopBits.Text);
                serialPort1.Parity = (Parity)Enum.Parse(typeof(Parity), cBoxParityBits.Text);

                serialPort1.Open();
                progressBar1.Value = 100;

                timer1.Interval = 500;
                timer1.Start();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cLOSEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                serialPort1.Close();
                progressBar1.Value = 0;

                timer1.Stop();
            }
        }

        private void btnSendData_Click(object sender, EventArgs e)
        {
            if(serialPort1.IsOpen)
            {
                dataOut = tBoxDataOut.Text;
                if(sendWidth == "None")
                {
                    serialPort1.Write(dataOut);
                }
                else if(sendWidth == "Both")
                {
                    serialPort1.Write(dataOut + "\r\n");
                }
                else if (sendWidth == "New Line")
                {
                    serialPort1.Write(dataOut + "\n");
                }
                else if (sendWidth == "Carriage Return")
                {
                    serialPort1.Write(dataOut + "\r");
                }
            }
        }

        private void toolStripComboBox2_DropDownClosed(object sender, EventArgs e)
        {
            if (toolStripComboBox2.Text == "None")
            {
                sendWidth = "None";
            }
            else if (toolStripComboBox2.Text == "Both")
            {
                sendWidth = "Both";
            }
            else if (toolStripComboBox2.Text == "New Line")
            {
                sendWidth = "New Line";
            }
            else if (toolStripComboBox2.Text == "Carriage Return")
            {
                sendWidth = "Carriage Return";
            }
        }

        private void chBoxDtrEnable_CheckedChanged(object sender, EventArgs e)
        {
            if(chBoxDtrEnable.Checked)
            {
                serialPort1.DtrEnable = true;
                MessageBox.Show("DTR Enable", "warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                serialPort1.DtrEnable = false;
            }
        }

        private void chBoxRtsEnable_CheckedChanged(object sender, EventArgs e)
        {
            if (chBoxRtsEnable.Checked)
            {
                serialPort1.RtsEnable = true;
                MessageBox.Show("RTS Enable", "warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                serialPort1.RtsEnable = false;
            }
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tBoxDataOut.Text != "")
            {
                tBoxDataOut.Text = "";
            }
        }

        private void tBoxDataOut_TextChanged(object sender, EventArgs e)
        {
            int dataOutLength = tBoxDataOut.TextLength;
            lblDataOutLength.Text = string.Format("{0:00}", dataOutLength);
        }

        private void tBoxDataOut_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.doSomething();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void doSomething()
        {
            if (serialPort1.IsOpen)
            {
                dataOut = tBoxDataOut.Text;
                if (sendWidth == "None")
                {
                    serialPort1.Write(dataOut);
                }
                else if (sendWidth == "Both")
                {
                    serialPort1.Write(dataOut + "\r\n");
                }
                else if (sendWidth == "New Line")
                {
                    serialPort1.Write(dataOut + "\n");
                }
                else if (sendWidth == "Carriage Return")
                {
                    serialPort1.Write(dataOut + "\r");
                }
            }

        }

        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            dataIn = serialPort1.ReadExisting();
            this.Invoke(new EventHandler(ShowData));
        }
        
        private void ShowData(object sender, EventArgs e)
        {
            int dataInLength = dataIn.Length;
            lblDataInLength.Text = string.Format("{0:00}", dataInLength);

            if(toolStripComboBox1.Text == "Always Update")
            {
                tBoxDataIn.Text = dataIn;
            }
            else if(toolStripComboBox1.Text == "Add Old Data")
            {
                if(toolStripComboBox3.Text == "TOP")
                {
                    tBoxDataIn.Text = tBoxDataIn.Text.Insert(0, dataIn);
                }
                else if (toolStripComboBox3.Text == "BOTTOM")
                {
                    tBoxDataIn.Text += dataIn;
                }
            }

            SaveDataToTxtFile();
        }

        private void clearToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (tBoxDataIn.Text != "")
            {
                tBoxDataIn.Text = "";
            }
        }

        private void lblStatusCom_Click(object sender, EventArgs e)
        {
//            Form form2 = new Form2();
//            this.AddOwnedForm(form2);
//            form2.Show();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Create by dsong", "dsong", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void eXITToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void toolStripComboBox_appendOrOverwriteText_DropDownClosed(object sender, EventArgs e)
        {
            if(toolStripComboBox_appendOrOverwriteText.Text == "Append Text")
            {
                state_AppendText = true;
            }
            else
            {
                state_AppendText = false;
            }
        }

        private void oPENToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Form2 objForm2 = new Form2(this);
            objForm2.Show();
            this.Hide();
        }

        private void tBoxDataIn_TextChanged(object sender, EventArgs e)
        {
            if (toolStripComboBox3.Text == "BOTTOM")
            {
                tBoxDataIn.SelectionStart = tBoxDataIn.Text.Length;
                tBoxDataIn.ScrollToCaret();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            xData = tickCount;  yData = 0;
            if(dataIn != null)
            {
                try
                {
                    yData = Convert.ToInt32(dataIn, 16);
                }
                catch (Exception err)
                {
                    // MessageBox.Show(err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            chart1.Series[0].Points.AddXY(xData, yData);
            if (chart1.Series[0].Points.Count > 100)
            {
                chart1.Series[0].Points.RemoveAt(0);
            }
            chart1.ChartAreas[0].AxisX.Minimum = chart1.Series[0].Points[0].XValue;
            chart1.ChartAreas[0].AxisX.Maximum = tickCount;

            tickCount++;
        }

        #endregion
    }
}
