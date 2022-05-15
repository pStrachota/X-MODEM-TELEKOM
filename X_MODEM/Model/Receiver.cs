using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;
using System.Windows;

namespace Model
{
    internal class Receiver
    {

        private readonly int CRC_16_CCITT = 0;
        private readonly int ALGEBRAIC = 1;
        
        private SerialPort _portNumber;

        public Receiver(string portName, int baudrate)
        {
            _portNumber = new SerialPort(portName)
            {
                BaudRate = baudrate,
            };
            _portNumber.Open();
        }

        public void Close()
        {
            _portNumber.Close();
        }


        public String Receive(int checksumMode)
        {
            try
            {
                if (!_portNumber.IsOpen)
                    _portNumber.Open();

                List<byte> byteListHelper = new();
                List<byte> recivedBytes = new();

                using (_portNumber)
                {
                    DateTime start = DateTime.Now;
                    bool letsStart = false;
                    while (DateTime.Today - start < TimeSpan.FromSeconds(60))
                    {
                        if (checksumMode == CRC_16_CCITT)
                        {
                            _portNumber.WriteLine(Signals.C.ToString());
                        }
                        else if(checksumMode == ALGEBRAIC)
                        {
                            _portNumber.WriteLine(Signals.NAK.ToString());
                        }

                        try
                        {
                            if (_portNumber.ReadLine() == Signals.SOH.ToString())
                            {
                                letsStart = true;
                                break;
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            throw;
                        }

                        System.Threading.Thread.Sleep(10000);
                    }

                    if (letsStart)
                    {
                        do
                        {
                            bool errorFlag = false;
                            if (255 - Convert.ToInt32(_portNumber.ReadLine()) !=
                                Convert.ToInt32(_portNumber.ReadLine()))
                            {
                                errorFlag = true;
                            }
                            else
                            {
                                for (int i = 0; i < 128; i++)
                                {
                                    int helper = _portNumber.ReadChar();
                                    byteListHelper.Add(Convert.ToByte(helper));
                                }

                                bool dataCorrectFlag = true;

                                if (checksumMode == CRC_16_CCITT)
                                {
                                    byte[] checkSumCrc = Checksum.CreateCheckSumCRC(byteListHelper.ToArray());

                                    for (int j = 0; j < 2; j++)
                                        if (checkSumCrc[j] != Convert.ToByte(_portNumber.ReadChar()))
                                        {
                                            dataCorrectFlag = false;
                                        }
                                }
                                else if (checksumMode == ALGEBRAIC)
                                {
                                    byte checksum = (byte)_portNumber.ReadByte();
                                    if (Checksum.AlgebraicChecksum(byteListHelper.ToArray()) != checksum)
                                    {
                                        dataCorrectFlag = false;
                                    }
                                }

                                if (!dataCorrectFlag)
                                    errorFlag = true;
                                else
                                {
                                    foreach (var byteHelper in byteListHelper)
                                    {
                                        recivedBytes.Add(byteHelper);
                                    }

                                    byteListHelper.Clear();
                                    _portNumber.WriteLine(Signals.ACK.ToString());
                                }
                            }

                            if (errorFlag)
                            {
                                _portNumber.DiscardInBuffer();
                                _portNumber.WriteLine(Signals.NAK.ToString());
                                byteListHelper.Clear();
                            }
                        } while (_portNumber.ReadLine() == Signals.SOH.ToString());
                    }

                    _portNumber.WriteLine(Signals.ACK.ToString());

                    return Encoding.Default.GetString(recivedBytes.ToArray());
                }
            }
            catch (TimeoutException te)
            {
                MessageBox.Show(te.Message);
            }

            return "";
        }
    }
}