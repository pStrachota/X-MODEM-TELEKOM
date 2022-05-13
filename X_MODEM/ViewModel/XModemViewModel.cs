using CommunityToolkit.Mvvm.Input;
using Model;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.IO.Ports;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ViewModel
{
    public class XModemViewModel : INotifyPropertyChanged
    {

        private Receiver _receiver;
        private Transmitter _transmitter;

        private string _selectedSenderCom;
        private string _selectedReceiverCom;

        private int _whichChecksumSender = 5;
        private int _whichChecksumReceiver = 5;

        private string _selectedSenderChecksum = "BASIC";
        private string _selectedReceiverChecksum = "BASIC";

        private string _senderMessage;
        private string _receiverMessage;

        private int _selectedSenderBaudrate = 9600;
        private int _selectedReceiverBaudrate = 9600;

        private String _message = "";

        public event PropertyChangedEventHandler? PropertyChanged;

        public void RaisePropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public XModemViewModel()
        {
            OpenFile = new RelayCommand(() =>
            {
                OpenFileDialog openFileDialog = new() { Filter = "Text files (*.txt)|*.txt" };
                if (openFileDialog.ShowDialog() != true)
                    return;

                SenderTextBox = File.ReadAllText(openFileDialog.FileName);
            });

            Send = new RelayCommand(() =>
            {
                if (_transmitter == null || _selectedSenderCom == null)
                {
                    MessageBox.Show("Choose serial port.");
                    return;
                }

                if (SenderTextBox == null)
                {
                    MessageBox.Show("Enter message to be sent.");
                    return;
                }

                Task.Run(() => _transmitter.Send(Encoding.GetEncoding(28591).GetBytes(SenderTextBox), _whichChecksumSender));
            });

            BackgroundWorker reciveWorker = new();
            reciveWorker.DoWork += ReciveMessage;
            reciveWorker.RunWorkerCompleted += ReciveWorker_RunWorkerCompleted;

            Receive = new RelayCommand(() =>
            {
                if (_receiver == null || _selectedReceiverCom == null)
                {
                    MessageBox.Show("Choose serial port.");
                    return;
                }

                if (!reciveWorker.IsBusy)
                    reciveWorker.RunWorkerAsync();
            });
        }

        private void ReciveWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ReceiverTextBox = _message;
        }

        public void ReciveMessage(object sender, EventArgs e)
        {
            Task a = Task.Run(() => _message = _receiver.Receive(_whichChecksumReceiver));
            a.Wait();
        }

        public ObservableCollection<string> ComPorts { get; set; } =
            new ObservableCollection<string>(SerialPort.GetPortNames());

        public ObservableCollection<string> Baudrate { get; set; } =
            new ObservableCollection<string>() { "9600", "115200" };

        public ObservableCollection<string> Checksum { get; set; } =
            new ObservableCollection<string>() { "CRC-17", "BASIC" };

        public string SelectedSenderBaudrate
        {
            get => _selectedSenderBaudrate.ToString();
            set
            {
                _selectedSenderBaudrate = int.Parse(value);
                RaisePropertyChanged(nameof(SelectedSenderBaudrate));
            }
        }

        public string SelectedSenderChecksum
        {
            get => _selectedSenderChecksum.ToString();
            set
            {
                _selectedSenderChecksum = value;
                if (value == "CRC-17")
                {
                    _whichChecksumSender = 0;
                    RaisePropertyChanged(nameof(SelectedSenderChecksum));
                }
                else if (value == "BASIC")
                {
                    _whichChecksumSender = 1;
                    RaisePropertyChanged(nameof(SelectedSenderChecksum));
                }

                RaisePropertyChanged(nameof(SelectedSenderChecksum));
            }
        }

        public string SelectedReceiverChecksum
        {
            get => _selectedReceiverChecksum.ToString();
            set
            {
                _selectedReceiverChecksum = value;
                if (value == "CRC-17")
                {
                    _whichChecksumReceiver = 0;
                    RaisePropertyChanged(nameof(SelectedReceiverChecksum));
                }
                else if (value == "BASIC")
                {
                    _whichChecksumReceiver = 1;
                    RaisePropertyChanged(nameof(SelectedReceiverChecksum));
                }

                RaisePropertyChanged(nameof(SelectedReceiverChecksum));
            }
        }


        public string SelectedSenderCom
        {
            get => _selectedSenderCom;
            set
            {
                if (_selectedSenderCom == value || value == null)
                    return;

                _selectedSenderCom = value;

                try
                {
                    _transmitter?.Close();
                    _transmitter = new Transmitter(_selectedSenderCom, _selectedSenderBaudrate);
                }
                catch (UnauthorizedAccessException)
                {
                    MessageBox.Show("Port is being used by other process. Acces denied.");
                    RaisePropertyChanged(nameof(SelectedSenderCom));
                }

                RaisePropertyChanged(nameof(SelectedSenderCom));
            }
        }

        public string SelectedReceiverBaudrate
        {
            get => _selectedReceiverBaudrate.ToString();
            set
            {
                _selectedReceiverBaudrate = int.Parse(value);
                RaisePropertyChanged(nameof(SelectedReceiverBaudrate));
            }
        }

        public string SelectedReceiverCom
        {
            get => _selectedReceiverCom;
            set
            {
                if (_selectedReceiverCom == value || value == null)
                    return;
                _selectedReceiverCom = value;
                try
                {
                    _receiver?.Close();
                    _receiver = new Receiver(_selectedReceiverCom, _selectedReceiverBaudrate);
                }
                catch (UnauthorizedAccessException)
                {
                    MessageBox.Show("Port is being used by other process. Acces denied");
                    RaisePropertyChanged(nameof(SelectedReceiverCom));
                }

                RaisePropertyChanged(nameof(SelectedReceiverCom));
            }
        }


        public string SenderTextBox
        {
            get => _senderMessage;

            set
            {
                _senderMessage = value;
                RaisePropertyChanged(nameof(SenderTextBox));
            }
        }

        public string ReceiverTextBox
        {
            get => _receiverMessage;

            set
            {
                _receiverMessage = value;
                RaisePropertyChanged(nameof(ReceiverTextBox));
            }
        }

        public ICommand OpenFile { get; set; }
        public ICommand Send { get; set; }
        public ICommand Receive { get; set; }
        public ICommand RefreshPorts { get; set; }

    }
}