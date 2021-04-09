//tabs=4
// --------------------------------------------------------------------------------
// ASCOM Dome driver for MattsDome
//
// Description:	ASCOM Dome Interface. Works with Arduino Dome controller. 
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
// 10-Apr-2018  MIT 0.3.0   Enhancements to logging + Enable / disable logging window and creation of log to file
// 12-May-2018  MIT 0.4.0   Added function to turn the LCD backlight ON / OFF from the setup dialogue - works with arduino 0.14
// 21-Apr-2019  MIT 0.5.0   Corrected slaving operation (changed canSlave from true to false). Fixed bug in SetupDialogueForm code which
//                          didn't check for instantiation of log window and log file before attempting writes.
// 29-Jan-2021  MIT 0.6.0   Implementing ASCOM TraceLogger to allow writing of logging messages to disk and improved tracing capability.
//                          Fixed bug with SlewToAzimuth method whereby it was referencing the Azimuth function instead of the azimuth variable.
// --------------------------------------------------------------------------------
//
// 21-Apr-2019 - Tested and working with arduino 0.14

// This is used to define code in the template that is specific to one class implementation
// unused code canbe deleted and this definition removed.
#define Dome

using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ASCOM;
using ASCOM.Utilities;
using ASCOM.DeviceInterface;
using System.Globalization;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;


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
        const byte ARD_GET_STATUS = 220; //For indi framework
        const byte ARD_LCD_ENABLE = 221;
        const int ARD_OK = 33;
        const int ARD_KO = 99;

        private int TicksPerDegree;
        public bool log_window_enabled = true;
        private ArduinoComms serial_port = new ArduinoComms(Properties.Settings.Default.CommPort);
        private static readonly Profile Profile = new Profile();

        logWindow L;
        Thread oThread;
        static readonly object locker = new object();
        delegate void SetTextCallback(string text);
        delegate void CloseDelegate();
        //StreamWriter w;
        public ASCOM.Utilities.TraceLogger trace;
        private string traceSource = "Matt's Dome";


        /// <summary>
        /// Initializes a new instance of the <see cref="MattsDome"/> class.
        /// Must be public for COM registration.
        /// </summary>
        public Dome()
        {
            Slaved = false;
            Get_values();
            TicksPerDegree = Properties.Settings.Default.TicksPerDomeRotation / 360;
            log_window_enabled = Properties.Settings.Default.LogWindowEnabled;
            trace = new ASCOM.Utilities.TraceLogger();
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
                if (log_window_enabled)
                {
                    oThread = new Thread(new ThreadStart(ShowCounterForm));
                }

                if (value == IsConnected)
                    return;

                if (value)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        int result = serial_port.SendCommand((byte)199, 0);
                        if (result == 33)
                        {
                            isConnected = true;
                            if (log_window_enabled)
                            {
                                oThread.IsBackground = true;
                                oThread.Start();
                            }
                            if (Properties.Settings.Default.LogToFile)
                            {
                                trace = new ASCOM.Utilities.TraceLogger();
                                trace.Enabled = true;
                                trace.LogMessage(traceSource, "Log Started");
                            }

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
                                isConnected = false;
                            }
                        }
                        else
                        {
                            isConnected = false;
                        }
                    }

                }
                else
                {
                    isConnected = false;
                    if (log_window_enabled) 
                    {
                        oThread.Abort();
                        L.Invoke(new CloseDelegate(L.Close)); //Required because we are closing the logwindow form from another thread
                        if (trace.Enabled)
                        {
                            trace.Enabled = false;
                            trace.Dispose();
                        }
                    }
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
        public void Slew_CW()
        {
            WriteToLog(DateTime.Now.ToString() + ": Application >> Driver: Start Manual CW slew", 2);
            int response = serial_port.SendCommand(ARD_SLEW_START_CW, 0);
            WriteToLog(DateTime.Now.ToString() + ": Driver >> Arduino: ARD_SLEW_START_CW", 2);
            if (response != 33)
            {
                WriteToLog(DateTime.Now.ToString() + ": Invalid response received from the Arduino", 2);
                throw new ASCOM.InvalidOperationException("Invalid response received from the Arduino");
            }
            else
            {
                WriteToLog(DateTime.Now.ToString() + ": Arduino >> Driver: Ok (33) response received", 2);
            }
        }

        /// <summary>
        /// Sends a command to the dome to begin slewing in a counter clockwise direction. This slew command can be stopped with the slew_Stop() command
        /// </summary>
        public void Slew_CCW()
        {
            WriteToLog(DateTime.Now.ToString() + ": Application >> Driver: Start Manual CCW slew", 2);
            int response = serial_port.SendCommand(ARD_SLEW_START_CCW, 0);
            WriteToLog(DateTime.Now.ToString() + ": Driver >> Arduino: ARD_SLEW_START_CCW", 2);
            if (response != 33)
            {
                WriteToLog(DateTime.Now.ToString() + ": Invalid response received from the Arduino", 2);
                throw new ASCOM.InvalidOperationException("Invalid response received from the Arduino");
            }
            else
            {
                WriteToLog(DateTime.Now.ToString() + ": Arduino >> Driver: Ok (33) response received", 2);
            }
        }

        /// <summary>
        /// Sends a command to the dome to stop any slew that was initiated by the slew_CW() or slew_CCW() commands.
        /// </summary>
        public void Slew_Stop()
        {
            WriteToLog(DateTime.Now.ToString() + ": Application >> Driver: Stop slew", 2);
            int response = serial_port.SendCommand(ARD_SLEW_STOP, 0);
            WriteToLog(DateTime.Now.ToString() + ": Driver >> Arduino: ARD_SLEW_STOP", 2);
            if (response != 33)
            {
                WriteToLog(DateTime.Now.ToString() + ": Invalid response received from the Arduino", 2);
                throw new ASCOM.InvalidOperationException("Invalid response received from the Arduino");
            }
            else
            {
                WriteToLog(DateTime.Now.ToString() + ": Arduino >> Driver: Ok (33) response received", 2);
            }
        }

        public void Set_home_pos(int angle)
        {
            WriteToLog(DateTime.Now.ToString() + ": Application >> Driver: Set Home Position to " + angle.ToString() + " degrees.", 2);
            int response = serial_port.SendCommand(ARD_SET_HOME_POS, angle);
            WriteToLog(DateTime.Now.ToString() + ": Driver >> Arduino: ARD_SET_HOME_POS to " + angle.ToString(), 2);
            if (response != angle) { throw new ASCOM.InvalidOperationException("Invalid response received from the Arduino"); }
            else { WriteToLog(DateTime.Now.ToString() + ": Arduino >> Driver: value returned = " + angle.ToString(), 2); }
        }

        public void Set_ticks_per_rev(int ticks)
        {
            WriteToLog(DateTime.Now.ToString() + ": Application >> Driver: Set ticks/rev to " + ticks.ToString(), 2);
            int response = serial_port.SendCommand(ARD_SET_TICKS_PER_REV, ticks);
            WriteToLog(DateTime.Now.ToString() + ": Driver >> Arduino: ARD_SET_TICKS_PER_REV to " + ticks.ToString(), 2);
            if (response != ticks) { throw new ASCOM.InvalidOperationException("Invalid response received from the Arduino"); }
            else { WriteToLog(DateTime.Now.ToString() + ": Arduino >> Driver: value returned = " + ticks.ToString(), 2); }
        }

        public void Set_park_pos(int degrees)
        {
            WriteToLog(DateTime.Now.ToString() + ": Application >> Driver: Set Park Position to " + degrees.ToString() + " degrees.", 2);
            int response = serial_port.SendCommand(ARD_SET_PARK_POS, degrees);
            WriteToLog(DateTime.Now.ToString() + ": Driver >> Arduino: ARD_SET_PARK_POS to " + degrees.ToString() + " degrees", 2);
            if (response != degrees) { throw new ASCOM.InvalidOperationException("Invalid response received from the Arduino"); }
            else { WriteToLog(DateTime.Now.ToString() + ": Arduino >> Driver: value returned = " + degrees.ToString(), 2); }
        }

        public void Get_values()
        {
            Properties.Settings.Default.HomeSensorPosition = Convert.ToInt16(serial_port.SendCommand(ARD_GET_HOME_POS, 0));
            Properties.Settings.Default.ParkPosition = Convert.ToInt16(serial_port.SendCommand(ARD_GET_PARK_POS, 0));
            Properties.Settings.Default.TicksPerDomeRotation = Convert.ToInt32(serial_port.SendCommand(ARD_GET_TICKS_PER_REV, 0));
        }

        public int GetAzimuth()
        {
            WriteToLog(DateTime.Now.ToString() + ": Application >> Driver: Get Current Azimuth ", 2);
            int az = serial_port.SendCommand(ARD_GET_AZIMUTH, 0);
            WriteToLog(DateTime.Now.ToString() + ": Driver >> Arduino: ARD_GET_AZIMUTH", 2);
            if (az < 0 || az >= 361)
            {
                WriteToLog(DateTime.Now.ToString() + ": Invalid response recieved from the Arduino: " + az.ToString(), 2);
                throw new ASCOM.InvalidOperationException("Invalid response received from the Arduino. az = " + az.ToString() + " degrees");
            }
            else { WriteToLog(DateTime.Now.ToString() + ": Arduino >> Driver: value returned = " + az.ToString() + " degrees.", 2); }
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
            L.txtLogWindow.Text += text + "\r\n";
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

        private void WriteToLog(string text, int level)
        {
            if (level <= Properties.Settings.Default.LogLevel)
            {
                if (log_window_enabled && L != null)
                {
                    //The following is statement is required to ensure safe threading code
                    if (L.txtLogWindow.InvokeRequired)
                    {
                        SetTextCallback d = new SetTextCallback(SetText);
                        L.Invoke(d, new object[] { text });
                    }
                    else
                    {
                        L.txtLogWindow.Text += text + "\r\n";
                        // The following 2 lines force the textbox to scroll down automatically as new lines added.
                        L.txtLogWindow.Select(L.txtLogWindow.Text.Length, 0);
                        L.txtLogWindow.ScrollToCaret();
                    }
                }

                if (Properties.Settings.Default.LogToFile)
                {
                    trace.LogMessage(traceSource, text);
                    //w.Write(text + "\r\n");
                }
            }

        }

        //private void WriteToLogFile(string text)
        //{
        //    w.Write(text);
        //}

        #endregion

        public void AbortSlew()
        {
            WriteToLog(DateTime.Now.ToString() + ": Application >> Driver: AbortSlew", 2);
            if (!Slewing) return;
            if (Slaved) slaved = false;
            int response = serial_port.SendCommand(ARD_SLEW_ABORT, 0);
            WriteToLog(DateTime.Now.ToString() + ": Driver >> Arduino: ARD_SLEW_ABORT", 2);
            if (response != ARD_OK)
            {
                WriteToLog(DateTime.Now.ToString() + ": Invalid response recieved from the Arduino: " + response.ToString(), 2);
                throw new ASCOM.InvalidValueException("AbortSlew command resulted in an invalid response from the Arduino board");
            }
            else
            {
                WriteToLog(DateTime.Now.ToString() + ": Arduino >> Driver: Ok (33) response received from Abortslew command" + response.ToString(), 2);
            }
        }

        public double Altitude
        {
            get { throw new ASCOM.PropertyNotImplementedException(); }
        }

        public bool AtHome
        {
            get
            {
                WriteToLog(DateTime.Now.ToString() + ": Application >> Driver: AtHome?", 2);
                int response = serial_port.SendCommand(ARD_IS_HOME, 0);
                WriteToLog(DateTime.Now.ToString() + ": Driver >> Arduino: ARD_IS_HOME", 2);
                if (response == ARD_OK)
                {
                    WriteToLog(DateTime.Now.ToString() + ": Arduino >> Driver: Ok (33) response received from AtHome? command" + response.ToString(), 2);
                    return true;
                }
                else if (response == ARD_KO)
                {
                    WriteToLog(DateTime.Now.ToString() + ": Arduino >> Driver: KO (99) response received from AtHome? command" + response.ToString(), 2);
                    return false;
                }
                else
                {
                    WriteToLog(DateTime.Now.ToString() + " Arduino >> Driver: Invalid response received from AtHome? command" + response.ToString(), 2);
                    throw new ASCOM.InvalidValueException("AtHome command resulted in an invalid response from the Arduino board");
                }
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
            get
            {
                WriteToLog(DateTime.Now.ToString() + ": Application >> Driver: Get Azimuth", 2);
                azimuth = serial_port.SendCommand(ARD_GET_AZIMUTH, 0);
                WriteToLog(DateTime.Now.ToString() + ": Driver >> Arduino: ARD_GET_AZIMUTH", 2);
                if (azimuth < 0 || azimuth >= 361)
                {
                    WriteToLog(DateTime.Now.ToString() + ": Invalid response recieved from the Arduino: " + azimuth.ToString(), 2);
                    throw new ASCOM.InvalidOperationException("Invalid response received from the Arduino. azimuth = " + azimuth.ToString() + " degrees");
                }
                else
                {
                    WriteToLog(DateTime.Now.ToString() + ": Arduino >> Driver: Azimuth = " + azimuth.ToString() + " degrees.", 2);
                    return azimuth;
                }
            }
        }

        public bool CanFindHome
        {
            get
            {
                WriteToLog(DateTime.Now.ToString() + ": Application >> Driver: CanFindHome? ", 2);
                WriteToLog(DateTime.Now.ToString() + ": Driver >> Application: CanFindHome = True ", 2);
                return true;
            }
        }

        public bool CanPark
        {
            get
            {
                WriteToLog(DateTime.Now.ToString() + ": Application >> Driver: CanPark? ", 2);
                WriteToLog(DateTime.Now.ToString() + ": Driver >> Application: CanPark = True ", 2);
                return true;
            }
        }

        public bool CanSetAltitude
        {
            get
            {
                WriteToLog(DateTime.Now.ToString() + ": Application >> Driver: CanSetAltitude? ", 2);
                WriteToLog(DateTime.Now.ToString() + ": Driver >> Application: CanSetAltitude = False ", 2);
                return false;
            }
        }

        public bool CanSetAzimuth
        {
            get
            {
                WriteToLog(DateTime.Now.ToString() + ": Application >> Driver: CanSetAzimuth? ", 2);
                WriteToLog(DateTime.Now.ToString() + ": Driver >> Application: CanSetAzimuth = True ", 2);
                return true;
            }
        }

        public bool CanSetPark
        {
            get
            {
                WriteToLog(DateTime.Now.ToString() + ": Application >> Driver: CanSetPark? ", 2);
                WriteToLog(DateTime.Now.ToString() + ": Driver >> Application: CanSetPark = True ", 2);
                return true;
            }
        }

        public bool CanSetShutter
        {
            get
            {
                WriteToLog(DateTime.Now.ToString() + ": Application >> Driver: CanSetShutter? ", 2);
                WriteToLog(DateTime.Now.ToString() + ": Driver >> Application: CanSetShutter = False ", 2);
                return false;
            }
        }

        public bool CanSlave
        {
            get
            {
                WriteToLog(DateTime.Now.ToString() + ": Application >> Driver: CanSlave? ", 2);
                WriteToLog(DateTime.Now.ToString() + ": Driver >> Application: CanSlave = False ", 2);
                return false;
            }
        }

        public bool CanSyncAzimuth
        {
            get
            {
                WriteToLog(DateTime.Now.ToString() + ": Application >> Driver: CanSyncAzimuth? ", 2);
                WriteToLog(DateTime.Now.ToString() + ": Driver >> Application: CanSyncAzimuth = True ", 2);
                return true;
            }
        }

        public void CloseShutter()
        {
            throw new ASCOM.InvalidOperationException("No Shutter");
        }

        public void FindHome()
        {
            FindDomeHome();
        }
        public async Task FindDomeHome()
        {
            WriteToLog(DateTime.Now.ToString() + ": Application >> Driver: FindHome", 2);
            if (Slaved) throw new ASCOM.SlavedException();
            int response = serial_port.SendCommand(ARD_FIND_HOME, 0);
            WriteToLog(DateTime.Now.ToString() + ": Driver >> Arduino: ARD_FIND_HOME", 2);
            if (response != ARD_OK)
            {
                WriteToLog(DateTime.Now.ToString() + ": Invalid response recieved from the Arduino: " + response.ToString(), 2);
                //Throw an exception - Comms failure
                throw new ASCOM.DriverException("Invalid response received from the Arduino");
            }
            else
            {
                WriteToLog(DateTime.Now.ToString() + ": OK signal recieved from the Arduino: " + response.ToString(), 2);
                WriteToLog(DateTime.Now.ToString() + ": The dome is now homing", 2);
                //perform a periodic check to see if the Arduino is in the home position
                DateTime starttime = DateTime.Now;
                DateTime endtime = starttime.AddSeconds((double)Properties.Settings.Default.FindHomeTimeout);
                await Task.Run(() =>
                {
                    while (DateTime.Now < endtime)
                    {
                        if (AtHome == true)
                        {
                            WriteToLog(DateTime.Now.ToString() + ": The dome is now homed", 2);
                            azimuth = Properties.Settings.Default.HomeSensorPosition;
                            //TODO
                            ////////////Need to Sync Azimuth here
                            return;
                        }
                        else System.Threading.Thread.Sleep(2000);
                    }
                    // Home Position not found within timeout period
                    WriteToLog(DateTime.Now.ToString() + ": Home Position not found within timeout period", 2);
                    throw new ASCOM.DriverException("Home Position not found within timeout period");
                }
                );
                
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
            { throw new ASCOM.PropertyNotImplementedException(); }
        }

        private bool slaved = false;
        public bool Slaved
        {
            get
            {
                return false;
            }
            set
            {
                //slaved = value;
                //{ throw new PropertyNotImplementedException("Slaved", false); }
                //throw new ASCOM.DriverException("Dome hardware slaving is not supported");
                //throw new ASCOM.InvalidValueException("Dome hardware slaving is not supported");
            }
        }

        public void SlewToAltitude(double Altitude)
        {
            throw new ASCOM.MethodNotImplementedException();
            //throw new System.NotImplementedException();
        }

        public void SlewToAzimuth(double TargetAzimuth)
        {
            WriteToLog(DateTime.Now.ToString() + ": Application >> Driver: SlewToAzimuth " + TargetAzimuth.ToString(), 2);
            WriteToLog(DateTime.Now.ToString() + ": (Current Dome Azimuth = " + azimuth.ToString() + ")", 2);
            double CurrentAzimuth = azimuth;
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
                            WriteToLog(DateTime.Now.ToString() + ": Driver >> Arduino: ARD_SLEW_TO_TARGET_CW " + ((short)TargetAzimuth).ToString(), 2);
                        }
                        else
                        {
                            //Counter Clockwise Command
                            response = serial_port.SendCommand(ARD_SLEW_TO_TARGET_CCW, (short)TargetAzimuth);
                            WriteToLog(DateTime.Now.ToString() + ": Driver >> Arduino: ARD_SLEW_TO_TARGET_CCW " + ((short)TargetAzimuth).ToString(), 2);
                        }
                    }
                    else
                    {
                        // //Clockwise Command
                        response = serial_port.SendCommand(ARD_SLEW_TO_TARGET_CW, (short)TargetAzimuth);
                        WriteToLog(DateTime.Now.ToString() + ": Driver >> Arduino: ARD_SLEW_TO_TARGET_CW " + ((short)TargetAzimuth).ToString(), 2);
                    }
                }
                else //Current dome azimuth position < 180 degrees
                {
                    int delta = Convert.ToInt16(TargetAzimuth - CurrentAzimuth);
                    if (delta <= 180 && delta > 0)
                    {
                        // //Clockwise Command
                        response = serial_port.SendCommand(ARD_SLEW_TO_TARGET_CW, (short)TargetAzimuth);
                        WriteToLog(DateTime.Now.ToString() + ": Driver >> Arduino: ARD_SLEW_TO_TARGET_CW " + ((short)TargetAzimuth).ToString(), 2);
                    }
                    else if (delta <= 0)
                    {
                        // CCW Command
                        response = serial_port.SendCommand(ARD_SLEW_TO_TARGET_CCW, (short)TargetAzimuth);
                        WriteToLog(DateTime.Now.ToString() + ": Driver >> Arduino: ARD_SLEW_TO_TARGET_CCW " + ((short)TargetAzimuth).ToString(), 2);
                    }
                    else
                    {
                        // CCW Command (changed from CW on 20130727)
                        response = serial_port.SendCommand(ARD_SLEW_TO_TARGET_CCW, (short)TargetAzimuth);
                        WriteToLog(DateTime.Now.ToString() + ": Driver >> Arduino: ARD_SLEW_TO_TARGET_CCW " + ((short)TargetAzimuth).ToString(), 2);
                    }
                }
                if (response != ARD_OK)
                {
                    WriteToLog(DateTime.Now.ToString() + ": Invalid response recieved from the Arduino: " + response.ToString(), 2);
                    throw new ASCOM.DriverException("Invalid response recieved from the Arduino");
                }
                else
                {
                    WriteToLog(DateTime.Now.ToString() + ": Arduino >> Driver: OK (33) signal recieved: " + response.ToString(), 2);
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
            WriteToLog(DateTime.Now.ToString() + ": Application >> Driver: Sync Azymuth to " + TargetAzimuth.ToString() + " degrees", 2);
            if (TargetAzimuth < 0 || TargetAzimuth > 360)
            {
                WriteToLog(DateTime.Now.ToString() + ": Invalid Azimuth provided by application: " + TargetAzimuth.ToString() + " degrees", 2);
                throw new ASCOM.InvalidValueException();
            }
            int response = serial_port.SendCommand(ARD_SET_AZIMUTH, (short)TargetAzimuth);
            WriteToLog(DateTime.Now.ToString() + ": Driver >> Arduino: ARD_SET_AZIMUTH: " + ((short)TargetAzimuth).ToString(), 2);
            if (response != ARD_OK)
            {
                WriteToLog(DateTime.Now.ToString() + ": Invalid response recieved from the Arduino: " + response.ToString(), 2);
                throw new ASCOM.DriverException("Attempt to Sync Azimuth failed. Invalid response received from the arduino");
            }
            else
            {
                azimuth = TargetAzimuth;
                WriteToLog(DateTime.Now.ToString() + ": Arduino >> Driver: OK (33) signal recieved: " + response.ToString(), 2);
            }
        }

        public void SetLCD(bool state)
        {
            short LCDState;
            if (state)
            {
                LCDState = 1;
            }
            else
            {
                LCDState = 0;
            }
            int response = serial_port.SendCommand(ARD_LCD_ENABLE, LCDState);
            if (response != ARD_OK)
            {
                throw new ASCOM.DriverException("Attempt to set LCD Backlight failed");
            }
        }
    }
}
