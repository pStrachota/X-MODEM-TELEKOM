using System;
using System.IO;
using System.IO.Ports;
using System.Windows;

namespace Model
{
    internal class Transmitter
    {

        private readonly int CRC_16_CCITT = 0;
        private readonly int ALGEBRAIC = 1;
        private SerialPort _portNumber;

        public Transmitter(string portName, int baudrate)
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


        public void Send(byte[] bytesToSend, int checksumMode)
        {
            try
            {
                if (!_portNumber.IsOpen)
                    _portNumber.Open();
                using (_portNumber)
                {
                    if (checksumMode == CRC_16_CCITT)
                    {
                        while (Convert.ToInt32(_portNumber.ReadLine()) != Signals.C)
                        {
                        }
                    }
                    else if (checksumMode == ALGEBRAIC)
                    {
                        while (Convert.ToInt32(_portNumber.ReadLine()) != Signals.NAK)
                        {
                        }
                    }

                    byte[] sentBytes = new byte[128];
                    for (int i = 0; i <= bytesToSend.Length / 128; i++)
                    {
                        do
                        {
                            _portNumber.WriteLine(Signals.SOH.ToString());
                            _portNumber.WriteLine(i.ToString());
                            _portNumber.WriteLine((255 - i).ToString());
                            for (int j = 0; j < 128; j++)
                            {
                                if (i * 128 + j == bytesToSend.Length)
                                {
                                    for (int k = 0; k < 128 - j; k++)
                                    {
                                        _portNumber.Write(Convert.ToChar(Signals.SUB).ToString());
                                        sentBytes[j + k] = Signals.SUB;
                                    }

                                    break;
                                }

                                _portNumber.Write(Convert.ToChar(bytesToSend[i * 128 + j]).ToString());
                                sentBytes[j] = bytesToSend[i * 128 + j];
                            }

                            if (checksumMode == CRC_16_CCITT)
                            {
                                byte[] crc = Checksum.CreateCheckSumCRC(sentBytes);
                                for (int j = 0; j < 2; j++)
                                {
                                    _portNumber.Write(Convert.ToChar(crc[j]).ToString());
                                }
                            }
                            else if (checksumMode == ALGEBRAIC)
                            {
                                byte basic = Checksum.AlgebraicChecksum(sentBytes);
                                _portNumber.Write(Convert.ToChar(Convert.ToChar(basic)).ToString());
                            }
                        } while (Convert.ToInt32(_portNumber.ReadLine()) == Signals.NAK);
                    }

                    do
                    {
                        _portNumber.WriteLine(Signals.EOT.ToString());
                    } while (Convert.ToInt32(_portNumber.ReadLine()) != Signals.ACK);
                }
            }
            catch (Exception ex)
            {
                if (ex is InvalidOperationException || ex is TimeoutException || ex is IOException)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }

                throw;
            }
        }
    }
}