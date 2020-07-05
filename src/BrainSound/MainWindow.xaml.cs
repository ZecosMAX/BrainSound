using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Sanford.Multimedia.Midi;

namespace BrainSound
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        

        public MainWindow()
        {
            InitializeComponent();
        }

        private void midiInPortListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DeviceListener deviceListener = new DeviceListener();
            deviceListener.Load(MessagesTextBox);

            MessagesTextBox.TextChanged += MessagesTextBox_TextChanged;

            LabelDeviceCount.Content = $"Device count: {InputDevice.DeviceCount}";
            LabelDeviceName.Content = $"Device name: {InputDevice.GetDeviceCapabilities(0).name}";
            LabelDeviceMID.Content = $"Device MID: {InputDevice.GetDeviceCapabilities(0).mid}";
            LabelDevicePID.Content = $"Device PID: {InputDevice.GetDeviceCapabilities(0).pid}";
            LabelDeviceSPP.Content = $"Device Support: {InputDevice.GetDeviceCapabilities(0).support}";
            LabelDeviceDV.Content = $"Device Driver ver: {InputDevice.GetDeviceCapabilities(0).driverVersion}";
        }

        private void InputDevice_SysExMessageReceived(object sender, SysExMessageEventArgs e)
        {
            MessagesTextBox.Text += $"SysEX: {e.Message.MessageType}\n";
        }

        private void InputDevice_SysRealtimeMessageReceived(object sender, SysRealtimeMessageEventArgs e)
        {
            MessagesTextBox.Text += $"SysRealtime: {e.Message.Message}\n";
        }

        private void InputDevice_ChannelMessageReceived(object sender, ChannelMessageEventArgs e)
        {
            MessagesTextBox.Text += $"Channel: {e.Message.Message}\n";
            MessagesTextBox.ScrollToEnd();

        }

        private void InputDevice_SysCommonMessageReceived(object sender, SysCommonMessageEventArgs e)
        {
            MessagesTextBox.Text += $"Sys Common: {e.Message.Message}\n";
        }

        private void MessagesTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(MessagesTextBox.LineCount > 1000)
            {
                MessagesTextBox.Clear();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
