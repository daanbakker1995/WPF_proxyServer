using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ProxyServer.HTTP;

namespace ProxyServer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private Proxy proxy;

        // ProxyServer status
        public bool ProxyStarted { get; private set; }

        // Strings for butten UI
        private const string START_SERVER_TEXT = "Starten";
        private const string CLOSE_SERVER_TEXT = "Stoppen";

        public MainWindow()
        {
            InitializeComponent();
            StartStopButton.Content = START_SERVER_TEXT;
            ProxyStarted = false;
        }

        /// <summary>
        /// StartStopButton click event handler. Starts or stopt proxy server.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartStopButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ProxyStarted)
            {
                if (IsValidIP(InputServerIP.Text)
                   && IsPortNrValid(InputPortNumber.Text)
                   && IsValidBufferSize(InputBufferSize.Text))
                {
                    int portNr = int.Parse(InputPortNumber.Text);
                    int BufferSize = int.Parse(InputBufferSize.Text);
                    int TimeOut = int.Parse(InputCacheTimeOut.Text);
                    AddToChatList("Starten...");
                    proxy = new Proxy(
                        InputServerIP.Text, 
                        portNr, 
                        BufferSize, 
                        TimeOut, 
                        (message) => AddToChatList(message) ,
                        (message) => AddMessageToHTTPDetails(message),
                        IsChecked(hideUserAgent), 
                        IsChecked(filterContent), 
                        IsChecked(logRequestHeader), 
                        IsChecked(logResponseHeaders), 
                        IsChecked(cacheContent),
                        IsChecked(basicAuthentication)
                        );
                    proxy.StartProxy();
                    ProxyStarted = true;
                    UpdateBtnServerStart();
                }
                else
                {
                    UpdateErrorDisplay("Foute gevens, controleer gegevens en probeer opnieuw.");
                }
            }
            else{
                CloseServer();
            }
        }

        private bool IsChecked(CheckBox checkBox)
        {
            if (checkBox.IsChecked == true)
            {
                return true;
            }
            return false;
        }

        private void CloseServer()
        {
            AddToChatList("Stoppen...");
            proxy.StopProxy();
            ProxyStarted = false;
            UpdateBtnServerStart();
        }

        private void MaakLeeg_Click(object sender, RoutedEventArgs e)
        {
            if(proxy != null)
            {
                // proxy.ClearCache(); TODO: Implement button to clear cache
                HttpDetails.Items.Clear();
                LogList.Items.Clear();
            }
        }

        private void AddMessageToHTTPDetails(string message)
        {
            Dispatcher.Invoke(() =>
            {
                ListBoxItem item = new ListBoxItem { Content = message };
                HttpDetails.Items.Add(item);
                // Scroll to item
                LogList.ScrollIntoView(item);
            });
        }


        /// <summary>
        /// Adds a message to the chatlist
        /// </summary>
        private void AddToChatList(String message)
        {
            Dispatcher.Invoke(() =>
            {
                ListBoxItem item = new ListBoxItem { Content = message };
                LogList.Items.Add(item);
                // Scroll to item
                LogList.ScrollIntoView(item);
            });
        }

        /// <summary>
        /// Update interface with Error, by default empty
        /// </summary>
        /// <param name="errorMessage"></param>
        private void UpdateErrorDisplay(String errorMessage = "")
        {
            ErrorTextBlock.Text = errorMessage;
        }

        /// <summary>
        /// Update the text of BtnStartServer
        /// </summary>
        private void UpdateBtnServerStart()
        {
            Dispatcher.Invoke(() =>
            {
                if (!ProxyStarted)
                {
                    StartStopButton.Content = START_SERVER_TEXT;
                }
                else
                {
                    StartStopButton.Content = CLOSE_SERVER_TEXT;
                }
            });
        }

        /// <summary>
        /// Event handler for InputServerIP_KeyUp, checks if valid input else update error display
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InputServerIP_KeyUp(object sender, KeyEventArgs e)
        {
            if (!IsValidIP(InputServerIP.Text))
            {
                UpdateErrorDisplay("Ongeldig IP adres.");
            }
            else
            {
                UpdateErrorDisplay();
            }
        }

        /// <summary>
        /// Event handler for InportPortNumber_KeyUp, checks if input is valid else updates error display
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InputPortNumber_KeyUp(object sender, KeyEventArgs e)
        {
            if (IsPortNrValid(InputPortNumber.Text))
            {
                UpdateErrorDisplay();
            }
            else
            {
                UpdateErrorDisplay("Ongeldig poort nummer.");
            }
        }

        /// <summary>
        /// Event handler for InputBufferSize_KeyUp, checks if bufferSize is valid else updates error display
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InputBufferSize_KeyUp(object sender, KeyEventArgs e)
        {
            if (IsValidBufferSize(InputBufferSize.Text))
            {
                UpdateErrorDisplay();
            }
            else
            {
                UpdateErrorDisplay("Ongeldig poort nummer.");
            }
        }

        /// <summary>
        /// Checks if given string can be converted into an int and is its a valid number based on <code>IsValidPortNumber</code>
        /// </summary>
        /// <param name="portNr"></param>
        /// <returns></returns>
        private bool IsPortNrValid(String portNr)
        {
            bool Isvalid = false;
            if (portNr != "")
            {
                if (int.TryParse(portNr, out _))
                {
                    //if (IsValidPortNumber(int.Parse(portNr)))
                    //{
                        Isvalid = true;
                    //}
                }
            }
            return Isvalid;
        }

        /// <summary>
        /// Check if param is valid ip4
        /// </summary>
        /// <param name="IPAdress"></param>
        /// <returns></returns>
        private bool IsValidIP(String IPAdress)
        {
            const int allowedSplitValues = 4;
            var splitValues = IPAdress.Split('.');
            if (splitValues.Length != allowedSplitValues || string.IsNullOrWhiteSpace(IPAdress)) return false; // Check if split value is 4.
            return splitValues.All(r => byte.TryParse(r, out byte tempForParsing)); // Returns true if all split values can be parsed to bytes
        }

        /// <summary>
        /// Check if int is a valid port number.
        /// Based on valid port numbers: https://en.wikipedia.org/wiki/List_of_TCP_and_UDP_port_numbers#Dynamic,_private_or_ephemeral_ports
        /// </summary>
        /// <param name="portNumber"></param>
        /// <returns></returns>
        private bool IsValidPortNumber(int portNumber)
        {
            const int minValidPortNumber = 49152;
            const int maxValidPortNumber = 65535;
            if (portNumber >= minValidPortNumber && portNumber <= maxValidPortNumber)
            {
                return true;
            }
            return false;
        }

        private bool IsValidBufferSize(string BufferSize)
        {
            Regex RegMatch = new Regex(@"^[0-9]*$");
            // buffersize can be between 0 and 1024, and only contains numbers -> if valid, field is valid
            if (RegMatch.IsMatch(BufferSize) && !string.IsNullOrEmpty(BufferSize) && int.Parse(BufferSize) > 0)
            {
                return true;
            }
            return false;
        }
    }
}
