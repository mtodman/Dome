using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;


namespace ASCOM.MattsDome
{
    [ComVisible(false)]					// Form not registered for COM!
    public partial class SetupDialogForm : Form
    {
        Dome dome;
        private bool sensorPosChanged;
        private bool parkPosChanged;
        private bool ticksPerRevChanged;

        public SetupDialogForm()
        {
            InitializeComponent();
            dome = new Dome();
            cmbComm.Text = Properties.Settings.Default.CommPort;
            txtHomeSensorPos.Text = Properties.Settings.Default.HomeSensorPosition.ToString();
            txtParkPos.Text = Properties.Settings.Default.ParkPosition.ToString();
            txtTicksPerRev.Text = Properties.Settings.Default.TicksPerDomeRotation.ToString();
            txtFindHomeTimeout.Text = Properties.Settings.Default.FindHomeTimeout.ToString();
            foreach (string str in SerialPort.GetPortNames())
            {
                this.cmbComm.Items.Add(str);
            }
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.CommPort = cmbComm.Text;
            Properties.Settings.Default.HomeSensorPosition = Convert.ToInt16(txtHomeSensorPos.Text);
            Properties.Settings.Default.ParkPosition = Convert.ToInt16(txtParkPos.Text);
            Properties.Settings.Default.TicksPerDomeRotation = Convert.ToInt32(txtTicksPerRev.Text);
            Properties.Settings.Default.FindHomeTimeout = Convert.ToInt16(txtFindHomeTimeout.Text);
            if (parkPosChanged)
            {
                dome.SetSerial(cmbComm.Text);
                dome.set_park_pos(Convert.ToInt16(txtParkPos.Text));
            }
            if (ticksPerRevChanged)
            {
                dome.SetSerial(cmbComm.Text);
                dome.set_ticks_per_rev(Convert.ToInt16(txtTicksPerRev.Text));
            }
            if (sensorPosChanged)
            {
                dome.SetSerial(cmbComm.Text);
                dome.set_home_pos(Convert.ToInt16(txtHomeSensorPos.Text));
            }
            dome.Dispose();
            Close();
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            dome.Dispose();
            Close();
        }

        private void BrowseToAscom(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("http://ascom-standards.org/");
            }
            catch (System.ComponentModel.Win32Exception noBrowser)
            {
                if (noBrowser.ErrorCode == -2147467259)
                    MessageBox.Show(noBrowser.Message);
            }
            catch (System.Exception other)
            {
                MessageBox.Show(other.Message);
            }
        }

        private void cmdTest_Click(object sender, EventArgs e)
        {
            ArduinoComms m_serial = new ArduinoComms(this.cmbComm.Text);
            for (int i = 0; i < 2; i++)
            {
                int result = m_serial.SendCommand((byte)199, 0);
                if (result == 33) 
                {
                    MessageBox.Show("Connection Test OK");
                    break;
                }
                else if (result == 254)
                {
                    if (i == 0)
                    {
                        System.Threading.Thread.Sleep(500);
                    }
                    else
                    {
                        MessageBox.Show("Connection Test Timed Out");
                    }
                }
                else
                {
                    MessageBox.Show("connection Test Failed");
                }
            }
            m_serial.Dispose();

        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            if (dome.Slaved) return;
            dome.SetSerial(cmbComm.Text);
            dome.FindHome();
        }

        private void btnPark_Click(object sender, EventArgs e)
        {
            if (dome.Slaved) return;
            dome.SetSerial(cmbComm.Text);
            dome.Park();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            dome.Connected = true;
        }

        private void btnCCW_MouseDown(object sender, MouseEventArgs e)
        {
            if (dome.Slaved) return;
            dome.SetSerial(cmbComm.Text);
            dome.slew_CCW();
        }

        private void btnCCW_MouseUp(object sender, MouseEventArgs e)
        {
            if (dome.Slaved) return;
            dome.SetSerial(cmbComm.Text);
            dome.slew_Stop();
        }

        private void btnCW_MouseDown(object sender, MouseEventArgs e)
        {
            if (dome.Slaved) return;
            dome.SetSerial(cmbComm.Text);
            dome.slew_CW();
        }

        private void btnCW_MouseUp(object sender, MouseEventArgs e)
        {
            if (dome.Slaved) return;
            dome.SetSerial(cmbComm.Text);
            dome.slew_Stop();
        }

        private void txtHomeSensorPos_Leave(object sender, EventArgs e)
        {
            sensorPosChanged = true;
        }

        private void txtParkPos_Leave(object sender, EventArgs e)
        {
            parkPosChanged = true;
        }

        private void txtTicksPerRev_Leave(object sender, EventArgs e)
        {
            ticksPerRevChanged = true;
        }

        private void btnGetAzimuth_Click(object sender, EventArgs e)
        {
            txtAzimuth.Text = dome.getAzimuth().ToString();
        }


        //Debugging Code


        private void btnSlewToTargetCW_Click(object sender, EventArgs e)
        {
            if (dome.Slaved) return;
            dome.SetSerial(cmbComm.Text);
            dome.Connected = true;
            dome.SlewToAzimuth(Convert.ToDouble(txtTargetAzimuth.Text));
        }

        private void btnSlewToTargetCCW_Click(object sender, EventArgs e)
        {
            if (dome.Slaved) return;
            dome.SetSerial(cmbComm.Text);
            dome.AbortSlew();
        }

        private void btnSetAzimuth_Click(object sender, EventArgs e)
        {
            if (dome.Slaved) return;
            dome.SetSerial(cmbComm.Text);
            dome.SyncToAzimuth(Convert.ToDouble(txtSetAzimuth.Text));
            txtAzimuth.Text = dome.getAzimuth().ToString();
        }

    }
}