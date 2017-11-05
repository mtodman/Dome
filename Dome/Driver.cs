//tabs=4
// --------------------------------------------------------------------------------
// ASCOM Dome driver for MattsDome
//
// Description:	ASCOM Dome Interface. Works with Arduino Dome controller for Sirius Domes. 
//
// Implements:	ASCOM Dome interface version: 
// Author:		(MIT) Matt Todman <matthew.todman@gmail.com>
//
// Edit Log:
//
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------
// 10-Mar-2013	MIT	0.1.0	Basic Working Version (with free bugs) - Works with Arduino code Arduino_Dome_Controller_0.4 & 0.5
// 20-Jul-2013  MIT 0.2.3   Added Traffic view via console output
// 27-Jul-2013  MIT 0.2.5   Corrected slew command direction when moving from (0-90 deg) to (270 - 359 deg)
// --------------------------------------------------------------------------------
//

// This is used to define code in the template that is specific to one class implementation
// unused code canbe deleted and this definition removed.
#define Dome

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ASCOM;
using ASCOM.Utilities;
using ASCOM.DeviceInterface;
using System.Globalization;
using System.Collections;
using System.Threading;


namespace ASCOM.MattsDome
{
    //
    // Your driver's DeviceID is ASCOM.MattsDome.Dome
    //
    // The Guid attribute sets the CLSID for ASCOM.MattsDome.Dome
    // The ClassInterface/None addribute prevents an empty interface called
    // _MattsDome from being created and used as the [default] interface
    //
    // TODO right click on IDomeV2 and select "Implement Interface" to
    // generate the property amd method definitions for the driver.
    //
    // TODO Replace the not implemented exceptions with code to implement the function or
    // throw the appropriate ASCOM exception.
    //

    /// <summary>
    /// ASCOM Dome Driver for MattsDome.
    /// </summary>
    [Guid("0d658cf8-eb81-4b62-8cb8-bd28a574a341")]
    [ClassInterface(ClassInterfaceType.None)]
    public class Dome : IDomeV2
    {
        /// <summary>
        /// ASCOM DeviceID (COM ProgID) for this driver.
        /// The DeviceID is used by ASCOM applications to load the driver at runtime.
        /// </summary>
        private static string driverID = "ASCOM.MattsDome.Dome";
        /// <summary>
        /// Driver description that displays in the ASCOM Chooser.
        /// </summary>
        private static string driverDescription = "ASCOM Dome Driver for MattsDome.";

        const byte ARD_TEST_CONNECTION = 199;
        const byte ARD_SLEW_TO_TARGET_CW = 200;
        const byte ARD_SLEW_TO_TARGET_CCW = 201;
        const byte ARD_SLEW_ABORT = 202;
        const byte ARD_GET_AZIMUTH = 203;
        const byte ARD_SET_AZIMUTH = 204;
        const byte ARD_OPEN_SHUTTER = 205;
        const byte ARD_CLOSE_SHUTTER = 206;
        const byte ARD_PARK_DOME = 207;
        const byte ARD_SET_PARK_POS = 208;
        const byte ARD_FIND_HOME = 209;
        const byte ARD_IS_HOME = 210;
        const byte ARD_IS_SLEWING = 211;
        const byte ARD_SLEW_START_CW = 212;
        const byte ARD_SLEW_START_CCW = 213;
        const byte ARD_SLEW_STOP = 214;
        const byte ARD_SET_HOME_POS = 215;
        const byte ARD_SET_TICKS_PER_REV = 216;
        const byte ARD_GET_HOME_POS = 217;
        const byte ARD_GET_PARK_POS = 218;
        const byte ARD_GET_TICKS_PER_REV = 219;
        const int ARD_OK = 33;

        private int TicksPerDegree;

        private ArduinoComms serial_port = new ArduinoComms(Properties.Settings.Default.CommPort);
        private static readonly Profile Profile = new Profile();

        logWindow L;
        Thread oThread;
        static readonly object locker = new object();
        delegate void SetTextCallback(string text);


        /// <summary>
        /// Initializes a new instance of the <see cref="MattsDome"/> class.
        /// Must be public for COM registration.
        /// </summary>
        public Dome()
        {
            Slaved = false;
            get_values();
            TicksPerDegree = Properties.Settings.Default.TicksPerDomeRotation / 360;
        }

        #region ASCOM Registration
        /// <summary>
        /// Register or unregister the driver with the ASCOM Platform.
        /// This is harmless if the driver is already registered/unregistered.
        /// </summary>
        /// <param name="bRegister">If <c>true</c>, registers the driver, otherwise unregisters it.</param>
        private static void RegUnregASCOM(bool bRegister)
        {
            using (var P = new ASCOM.Utilities.Profile())
            {
                P.DeviceType = "Dome";
                if (bRegister)
                {
                    P.Register(driverID, driverDescription);
                }
                else
                {
                    P.Unregister(driverID);
                }
            }
        }

        /// <summary>
        /// This function registers the driver with the ASCOM Chooser and
        /// is called automatically whenever this class is registered for COM Interop.
        /// </summary>
        /// <param name="t">Type of the class being registered, not used.</param>
        /// <remarks>
        /// This method typically runs in two distinct situations:
        /// <list type="numbered">
        /// <item>
        /// In Visual Studio, when the project is successfully built.
        /// For this to work correctly, the option <c>Register for COM Interop</c>
        /// must be enabled in the project settings.
        /// </item>
        /// <item>During setup, when the installer registers the assembly for COM Interop.</item>
        /// </list>
        /// This technique should mean that it is never necessary to manually register a driver with ASCOM.
        /// </remarks>
        [ComRegisterFunction]
        public static void RegisterASCOM(Type t)
        {
            RegUnregASCOM(true);
        }

        /// <summary>
        /// This function unregisters the driver from the ASCOM Chooser and
        /// is called automatically whenever this class is unregistered from COM Interop.
        /// </summary>
        /// <param name="t">Type of the class being registered, not used.</param>
        /// <remarks>
        /// This method typically runs in two distinct situations:
        /// <list type="numbered">
        /// <item>
        /// In Visual Studio, when the project is cleaned or prior to rebuilding.
        /// For this to work correctly, the option <c>Register for COM Interop</c>
        /// must be enabled in the project settings.
        /// </item>
        /// <item>During uninstall, when the installer unregisters the assembly from COM Interop.</item>
        /// </list>
        /// This technique should mean that it is never necessary to manually unregister a driver from ASCOM.
        /// </remarks>
        [ComUnregisterFunction]
        public static void UnregisterASCOM(Type t)
        {
            RegUnregASCOM(false);
        }
        #endregion

        //
        // PUBLIC COM INTERFACE IDomeV2 IMPLEMENTATION
        //

        /// <summary>
        /// Displays the Setup Dialog form.
        /// If the user clicks the OK button to dismiss the form, then
        /// the new settings are saved, otherwise the old values are reloaded.
        /// THIS IS THE ONLY PLACE WHERE SHOWING USER INTERFACE IS ALLOWED!
        /// </summary>
        public void SetupDialog()
        {
            // consider only showing the setup dialog if not connected
            // or call a different dialog if connected
            if (IsConnected)
                System.Windows.Forms.MessageBox.Show("Already connected, just press OK");

            using (SetupDialogForm F = new SetupDialogForm())
            {
                var result = F.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    Properties.Settings.Default.Save();
                    return;
                }
                Properties.Settings.Default.Reload();
            }
        }


        #region common properties and methods. All set to no action

        public System.Collections.ArrayList SupportedActions
        {
            get { return new ArrayList(); }
        }

        public string Action(string actionName, string actionParameters)
        {
            throw new ASCOM.MethodNotImplementedException("Action");
        }

        public void CommandBlind(string command, bool raw)
        {
            CheckConnected("CommandBlind");
            // Call CommandString and return as soon as it finishes
            this.CommandString(command, raw);
            // or
            throw new ASCOM.MethodNotImplementedException("CommandBlind");
        }

        public bool CommandBool(string command, bool raw)
        {
            CheckConnected("CommandBool");
            string ret = CommandString(command, raw);
            // TODO decode the return string and return true or false
            // or
            throw new ASCOM.MethodNotImplementedException("CommandBool");
        }

        public string CommandString(string command, bool raw)
        {
            CheckConnected("CommandString");
            // it's a good idea to put all the low level communication with the device here,
            // then all communication calls this function
            // you need something to ensure that only one command is in progress at a time

            throw new ASCOM.MethodNotImplementedException("CommandString");
        }

        #endregion

        #region public properties and methods
        public void Dispose()
        {
            //this.Dispose();
            //throw new System.NotImplementedException();
        }

        public bool Connected
        {
            get { return IsConnected; }
            set
            {
                oThread = new Thread(new ThreadStart(ShowCounterForm));
                if (value == IsConnected)
                    return;

                if (value)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        int result = serial_port.SendCommand((byte)199, 0);
                        if (result == 33)
                        {
                            isConnected=true;
                            oThread.IsBackground = true;
                            oThread.Start();
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
                                isConnected=false;
                            }
                        }
                        else
                        {
                            isConnected=false;
                        }
                    }

                }
                else
                {
                    isConnected = false;
                    oThread.Abort();
                }
            }
        }

        public string Description
        {
            get { return driverDescription; }
        }

        public string DriverInfo
        {
            get
            {
                return (driverDescription + "Version = " + DriverVersion);
            }
        }

        public string DriverVersion
        {
            get
            {
                Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                return String.Format(CultureInfo.InvariantCulture, "{0}.{1}", version.Major, version.Minor);
            }
        }

        public short InterfaceVersion
        {
            // set by the driver wizard
            get { return 2; }
        }

        public void SetSerial(string CommPort)
        {
            serial_port = new ArduinoComms(CommPort);
        }

        /// <summary>
        /// Sends a command to the dome to begin slewing in a clockwise direction. This slew command can be stopped with the slew_Stop() command
        /// </summary>
        public void slew_CW()
        {
            int response = serial_port.SendCommand(ARD_SLEW_START_CW,0);
            if (response != 33) throw new ASCOM.InvalidOperationException("Invalid response received from the Arduino");
        }

        /// <summary>
        /// Sends a command to the dome to begin slewing in a counter clockwise direction. This slew command can be stopped with the slew_Stop() command
        /// </summary>
        public void slew_CCW()
        {
            int response = serial_port.SendCommand(ARD_SLEW_START_CCW, 0);
            if (response != 33) throw new ASCOM.InvalidOperationException("Invalid response received from the Arduino");
        }

        /// <summary>
        /// Sends a command to the dome to stop any slew that was initiated by the slew_CW() or slew_CCW() commands.
        /// </summary>
        public void slew_Stop()
        {
            int response = serial_port.SendCommand(ARD_SLEW_STOP, 0);
            if (response != 33) throw new ASCOM.InvalidOperationException("Invalid response received from the Arduino");
        }

        public void set_home_pos(int angle)
        {
            int response = serial_port.SendCommand(ARD_SET_HOME_POS, angle);
            if (response != angle) throw new ASCOM.InvalidOperationException("Invalid response received from the Arduino");
        }

        public void set_ticks_per_rev(int ticks)
        {
            int response = serial_port.SendCommand(ARD_SET_TICKS_PER_REV, ticks);
            if (response != ticks) throw new ASCOM.InvalidOperationException("Invalid response received from the Arduino");
        }

        public void set_park_pos(int degrees)
        {
            int response = serial_port.SendCommand(ARD_SET_PARK_POS, degrees);
            if (response != degrees) throw new ASCOM.InvalidOperationException("Invalid response received from the Arduino");
        }

        public void get_values()
        {
            Properties.Settings.Default.HomeSensorPosition = Convert.ToInt16(serial_port.SendCommand(ARD_GET_HOME_POS,0));
            Properties.Settings.Default.ParkPosition = Convert.ToInt16(serial_port.SendCommand(ARD_GET_PARK_POS, 0));
            Properties.Settings.Default.TicksPerDomeRotation = Convert.ToInt32(serial_port.SendCommand(ARD_GET_TICKS_PER_REV, 0));
        }

        public int getAzimuth()
        {
            int az = serial_port.SendCommand(ARD_GET_AZIMUTH, 0);
            return az;
        }

        #endregion

        #region private properties and methods
        // here are some useful properties and methods that can be used as required
        // to help with

        /// <summary>
        /// Returns true if there is a valid connection to the driver hardware
        /// </summary>
        private bool isConnected = false;
        private bool IsConnected
        {
            get
            {
                return isConnected;
                //int result = serial_port.SendCommand(ARD_TEST_CONNECTION, 0);
                //if (result == ARD_OK) return true; else return false;
            }
        }


        /// <summary>
        /// Use this function to throw an exception if we aren't connected to the hardware
        /// </summary>
        /// <param name="message"></param>
        private void CheckConnected(string message)
        {
            if (!IsConnected)
            {
                throw new ASCOM.NotConnectedException(message);
            }
        }

        /// <summary>
        /// Writes traffic info to a log file
        /// </summary>
        //private bool ShowTrafic;
        //public bool showTrafic
        //{
        //    get { return ShowTrafic; }
        //    set {ShowTrafic = value;}
        //}
        public bool showTrafic = true;


        // This method is passed in to the SetTextCallBack delegate
        // to set the Text property of C.txtPosition.Text.
        private void SetText(string text)
        {
            L.txtLogWindow.Text += text;
            // The following 2 lines force the textbox to scroll down automatically as new lines added.
            L.txtLogWindow.Select(L.txtLogWindow.Text.Length, 0);
            L.txtLogWindow.ScrollToCaret();
        }

        public void ShowCounterForm()
        {
            lock (locker)
            {

                L = new logWindow();
                L.ShowDialog();
            }
        }

        private void writeToLog(string text)
        {
            //The following is statement is required to ensure safe threading code
            if (L.txtLogWindow.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                L.Invoke(d, new object[] { text });
            }
            else
            {
                L.txtLogWindow.Text += text;
                // The following 2 lines force the textbox to scroll down automatically as new lines added.
                L.txtLogWindow.Select(L.txtLogWindow.Text.Length, 0);
                L.txtLogWindow.ScrollToCaret();
            }
        }

        #endregion

        public void AbortSlew()
        {
            if (!Slewing) return;
            if (Slaved) slaved = false;
            int response = serial_port.SendCommand(ARD_SLEW_ABORT, 0);
            if (response != ARD_OK) { throw new ASCOM.InvalidValueException("AbortSlew command resulted in an invalid response from the Arduino board"); }
        }

        public double Altitude
        {
            get { throw new ASCOM.PropertyNotImplementedException(); }
        }

        public bool AtHome
        {
            get
            {
                int response = serial_port.SendCommand(ARD_IS_HOME, 0);
                if (response == ARD_OK) return true; else return false;
            }
        }

        private bool atPark;
        public bool AtPark
        {
            get { return atPark; }
        }

        private double azimuth;
        public double Azimuth
        {
            get {
                azimuth = serial_port.SendCommand(ARD_GET_AZIMUTH, 0);
                return azimuth; }
        }

        public bool CanFindHome
        {
            get { return true; }
        }

        public bool CanPark
        {
            get { return true; }
        }

        public bool CanSetAltitude
        {
            get { return false; }
        }

        public bool CanSetAzimuth
        {
            get { return true; }
        }

        public bool CanSetPark
        {
            get { return true; }
        }

        public bool CanSetShutter
        {
            get { return false; ; }
        }

        public bool CanSlave
        {
            get { return true; }
        }

        public bool CanSyncAzimuth
        {
            get { return true; }
        }

        public void CloseShutter()
        {
            throw new ASCOM.InvalidOperationException("No Shutter");
        }

        public void FindHome()
        {
            if (Slaved) throw new ASCOM.SlavedException(); 
            int response = serial_port.SendCommand(ARD_FIND_HOME, 0);
            if (response != ARD_OK)
            {
                //Throw an exception - Comms failure
                throw new ASCOM.DriverException("Invalid response received from the Arduino");
            }
            else
            {
                //perform a periodic check to see if the Arduino is in the home position
                DateTime starttime = DateTime.Now;
                DateTime endtime = starttime.AddSeconds((double)Properties.Settings.Default.FindHomeTimeout);
                while (DateTime.Now < endtime)
                {
                    if (AtHome == true)
                    {
                        azimuth = Properties.Settings.Default.HomeSensorPosition;
                        //TODO
                        ////////////Need to Sync Azimuth here
                        return;
                    } else System.Threading.Thread.Sleep(2000);
                }
                // Home Position not found within timeout period
                throw new ASCOM.DriverException("Home Position not found within timeout period");
            }
        }

        public string Name
        {
            get { return driverID; }
        }

        public void OpenShutter()
        {
            throw new ASCOM.MethodNotImplementedException();
        }

        public void Park()
        {
            if (Slaved)
            {
                throw new ASCOM.SlavedException();
            }
            else
            {
                //FindHome();
                int response = serial_port.SendCommand(ARD_PARK_DOME, 0);
                if (response != ARD_OK)
                {
                    throw new ASCOM.DriverException("Problem encountered during Dome Parking");
                }
            }
        }

        public void SetPark()
        {
            int response = serial_port.SendCommand(ARD_SET_PARK_POS, (int)Azimuth);
            if (response != ARD_OK)
            {
                throw new ASCOM.DriverException("Problem encountered when setting Park Position");
            }
            else
            {
                Properties.Settings.Default.ParkPosition = (short)Azimuth;
            }
        }

        public ShutterState ShutterStatus
        {
            get
            {throw new ASCOM.PropertyNotImplementedException();}
        }

        private bool slaved = false;
        public bool Slaved
        {
            get
            {
                return slaved;
            }
            set
            {
                slaved = value;
            }
        }

        public void SlewToAltitude(double Altitude)
        {
            throw new ASCOM.MethodNotImplementedException();
            //throw new System.NotImplementedException();
        }

        public void SlewToAzimuth(double TargetAzimuth)
        {
            if (showTrafic)
            {
                writeToLog("From Application: SlewToAzimuth " + TargetAzimuth.ToString() + "\r\n");
                writeToLog("Current Dome Azimuth = " + Azimuth.ToString() + "\r\n");
            }
            double CurrentAzimuth = Azimuth;
            if (!IsConnected) return;
            if (TargetAzimuth < 0 || TargetAzimuth > 360)
            {
                throw new ASCOM.InvalidValueException();
            }
            if (TargetAzimuth == 360) TargetAzimuth = 0;
            if (slaved)
            {
                throw new ASCOM.SlavedException();
            }
            else
            {
                int response;
                if (CurrentAzimuth >= 180) // If the current dome azimuth position is >= 180
                {
                    int delta = Convert.ToInt16(TargetAzimuth - CurrentAzimuth);
                    if (delta <= 0)
                    {
                        if (Math.Abs(delta) >= 180)
                        {
                            //Clockwise Command
                            response = serial_port.SendCommand(ARD_SLEW_TO_TARGET_CW, (short)TargetAzimuth);
                            if (showTrafic)
                            {
                                writeToLog("To Arduino: ARD_SLEW_TO_TARGET_CW " + ((short)TargetAzimuth).ToString() + "\r\n");
                            }
                        }
                        else
                        {
                            //Counter Clockwise Command
                            response = serial_port.SendCommand(ARD_SLEW_TO_TARGET_CCW, (short)TargetAzimuth);
                            if (showTrafic)
                            {
                                writeToLog("To Arduino: ARD_SLEW_TO_TARGET_CCW " + ((short)TargetAzimuth).ToString() + "\r\n");
                            }
                        }
                    }
                    else
                    {
                        // //Clockwise Command
                        response = serial_port.SendCommand(ARD_SLEW_TO_TARGET_CW, (short)TargetAzimuth);
                        if (showTrafic)
                        {
                            writeToLog("To Arduino: ARD_SLEW_TO_TARGET_CW " + ((short)TargetAzimuth).ToString() + "\r\n");
                        }
                    }
                }
                else //Current dome azimuth position < 180 degrees
                {
                    int delta = Convert.ToInt16(TargetAzimuth - CurrentAzimuth);
                    if (delta <= 180 && delta > 0)
                    {
                        // //Clockwise Command
                        response = serial_port.SendCommand(ARD_SLEW_TO_TARGET_CW, (short)TargetAzimuth);
                        if (showTrafic)
                        {
                            writeToLog("To Arduino: ARD_SLEW_TO_TARGET_CW " + ((short)TargetAzimuth).ToString() + "\r\n");
                        }
                    }
                    else if (delta <= 0)
                    {
                        // CCW Command
                        response = serial_port.SendCommand(ARD_SLEW_TO_TARGET_CCW, (short)TargetAzimuth);
                        if (showTrafic)
                        {
                            writeToLog("To Arduino: ARD_SLEW_TO_TARGET_CCW " + ((short)TargetAzimuth).ToString() + "\r\n");
                        }
                    }
                    else
                    {
                        // CCW Command (changed from CW on 20130727)
                        response = serial_port.SendCommand(ARD_SLEW_TO_TARGET_CCW, (short)TargetAzimuth);
                        if (showTrafic)
                        {
                            writeToLog("To Arduino: ARD_SLEW_TO_TARGET_CCW " + ((short)TargetAzimuth).ToString() + "\r\n");
                        }
                    }
                }
                if (response != ARD_OK)
                {
                    throw new ASCOM.DriverException("Invalid response recieved from the Arduino");
                }
            }
        }

        public bool Slewing
        {
            get
            {
                if (slaved)
                {
                    throw new ASCOM.SlavedException();
                }
                else
                {
                    int response = serial_port.SendCommand(ARD_IS_SLEWING, 0);
                    if (response == ARD_OK)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        public void SyncToAzimuth(double TargetAzimuth)
        {
            if (TargetAzimuth < 0 || TargetAzimuth > 360)
            {
                throw new ASCOM.InvalidValueException();
            }
            if (TargetAzimuth == 360) TargetAzimuth = 0;

            int response = serial_port.SendCommand(ARD_SET_AZIMUTH, (short)TargetAzimuth);
            if (response != ARD_OK)
            {
                throw new ASCOM.DriverException("Attempt to Sync Azimuth failed");
            }
            else
            {
                azimuth = TargetAzimuth;
            }
        }
    }
}
