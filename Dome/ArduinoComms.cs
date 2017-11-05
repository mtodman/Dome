using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;


namespace ASCOM.MattsDome
{
    public class ArduinoComms : System.IO.Ports.SerialPort
    {
        #region Manager Variables
        private string _portName = string.Empty;
        private SerialPort _serialPort = new SerialPort();
        bool ComPortExists = false;
        #endregion

        #region Constructor

        /// <summary>
        /// Constructor to set the properties of our Manager Class
        /// </summary>
        /// <param name="name">Desired PortName</param>
        public ArduinoComms(string name)
        {
            _portName = name;
            //Need to confirm that the ComPort exits on the calling PC

            foreach (string str in SerialPort.GetPortNames())
            {
                if (str == name) ComPortExists = true;
            }
        }


        #endregion


        public int SendCommand(byte cmd, int arg)
        {
            if (!ComPortExists)
            {
                return 255;
            }
            if (!_serialPort.IsOpen)
            {
                _serialPort.PortName = _portName;
                _serialPort.ReadTimeout = 2000;
                _serialPort.WriteTimeout = 2000;
                _serialPort.Open();
            }
            int response;
            int b1, b2;
            byte[] cmdarray = new byte[3];
            cmdarray[0] = cmd;
            cmdarray[1] = IntToByte_upper(arg);
            cmdarray[2] = IntToByte_lower(arg);
            try
            {
            _serialPort.Write(cmdarray, 0, cmdarray.Length);
            //wait for reply
                b1 = _serialPort.ReadByte();
                b2 = _serialPort.ReadByte();
                response = ((int)b1) * 256 + b2;
            }
            catch (System.TimeoutException ex)
            {
                _serialPort.Close();
                return 254;
                //throw new TimeoutException(ex.Message);
            }
            _serialPort.Close();
            return response;
        }

        private byte IntToByte_upper(int input)
        {
            return (byte)(input / 256);
        }

        private byte IntToByte_lower(int input)
        {
            return (byte)(input % 256);
        }

    }
}
