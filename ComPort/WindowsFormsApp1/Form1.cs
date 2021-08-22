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

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string[] ports = SerialPort.GetPortNames();
            cBoxComPort.Items.AddRange(ports);

            btnOpen.Enabled = true;
            btnClose.Enabled = false;

            chBoxDtrEnable.Checked = false;
            serialPort1.DtrEnable = false;
            chBoxRtsEnable.Checked = false;
            serialPort1.RtsEnable = false;
            btnSendData.Enabled = false;
            chBoxWriteLine.Checked = false;
            chBoxWrite.Checked = true;
            sendWidth = "Write";

            chBoxAlwaysUpdate.Checked = false;
            chBoxAddToOldData.Checked = true;

            chart1.Series[0].ChartType = SeriesChartType.Line;
        }

        private void btnOpen_Click(object sender, EventArgs e)
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
                btnOpen.Enabled = false;
                btnClose.Enabled = true;
                lblStatusCom.Text = "ON";

                timer1.Interval = 500;
                timer1.Start();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message,"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnOpen.Enabled = true;
                btnClose.Enabled = false;
                lblStatusCom.Text = "OFF";
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if(serialPort1.IsOpen)
            {
                serialPort1.Close();
                progressBar1.Value = 0;
                btnOpen.Enabled = true;
                btnClose.Enabled = false;
                lblStatusCom.Text = "OFF";

                timer1.Stop();
            }
        }

        private void btnSendData_Click(object sender, EventArgs e)
        {
            if(serialPort1.IsOpen)
            {
                dataOut = tBoxDataOut.Text;
                if(sendWidth == "WriteLine")
                {
                    serialPort1.WriteLine(dataOut);
                }
                else if(sendWidth == "Write")
                {
                    serialPort1.Write(dataOut);
                }
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

        private void btnClearDataOut_Click(object sender, EventArgs e)
        {
            if(tBoxDataOut.Text != "")
            {
                tBoxDataOut.Text = "";
            }
        }

        private void tBoxDataOut_TextChanged(object sender, EventArgs e)
        {
            int dataOutLength = tBoxDataOut.TextLength;
            lblDataOutLength.Text = string.Format("{0:00}", dataOutLength);

            if(chBoxUsingEnter.Checked)
            {
                tBoxDataOut.Text = tBoxDataOut.Text.Replace(Environment.NewLine, "");
            }
        }

        private void chBoxUsingButton_CheckedChanged(object sender, EventArgs e)
        {
            if(chBoxUsingButton.Checked)
            {
                btnSendData.Enabled = true;
            }
            else
            {
                btnSendData.Enabled = false;
            }
        }

        private void tBoxDataOut_KeyDown(object sender, KeyEventArgs e)
        {
            if(chBoxUsingEnter.Checked)
            {
                if(e.KeyCode == Keys.Enter)
                {
                    if(serialPort1.IsOpen)
                    {
                        dataOut = tBoxDataOut.Text;
                        if (sendWidth == "WriteLine")
                        {
                            serialPort1.WriteLine(dataOut);
                        }
                        else if (sendWidth == "Write")
                        {
                            serialPort1.Write(dataOut);
                        }
                    }
                }
            }
        }

        private void chBoxWriteLine_CheckedChanged(object sender, EventArgs e)
        {
            if(chBoxWriteLine.Checked)
            {
                sendWidth = "WriteLine";
                chBoxWrite.Checked = false;
                chBoxWriteLine.Checked = true;
            }
        }

        private void chBoxWrite_CheckedChanged(object sender, EventArgs e)
        {
            if(chBoxWrite.Checked)
            {
                sendWidth = "Write";
                chBoxWrite.Checked = true;
                chBoxWriteLine.Checked = false;
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

            if(chBoxAlwaysUpdate.Checked)
            {
                tBoxDataIn.Text = dataIn;
            }
            else if(chBoxAddToOldData.Checked)
            {
                tBoxDataIn.Text += dataIn;
            }
        }

        private void chBoxAlwaysUpdate_CheckedChanged(object sender, EventArgs e)
        {
            if(chBoxAlwaysUpdate.Checked)
            {
                chBoxAlwaysUpdate.Checked = true;
                chBoxAddToOldData.Checked = false;
            }
            else
            {
                chBoxAddToOldData.Checked = true;
            }
        }

        private void chBoxAddToOldData_CheckedChanged(object sender, EventArgs e)
        {
            if(chBoxAddToOldData.Checked)
            {
                chBoxAlwaysUpdate.Checked = false;
                chBoxAddToOldData.Checked = true;
            }
            else
            {
                chBoxAlwaysUpdate.Checked = true;
            }
        }

        private void btnClearDataIn_Click(object sender, EventArgs e)
        {
            if(tBoxDataIn.Text != "")
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

        private void timer1_Tick(object sender, EventArgs e)
        {
            xData = tickCount;  yData = 0;
            if(dataIn != null)
            {
                yData = Convert.ToInt32(dataIn, 16);
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
    }
}
