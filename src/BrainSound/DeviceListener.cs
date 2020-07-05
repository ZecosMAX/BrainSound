using Sanford.Multimedia;
using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

//using System.Windows.Devices.Midi;

namespace BrainSound
{
    class DeviceListener
    {
        InputDevice inputDevice = new InputDevice(0);

        public int[] ChannelMessages = new int[1000];
        public int ChannelMessagesCount = 0;

        public TextBox messages;

        public void Load(TextBox messages)
        {
            this.messages = messages;

            inputDevice.SysCommonMessageReceived += InputDevice_SysCommonMessageReceived;
            inputDevice.ChannelMessageReceived += InputDevice_ChannelMessageReceived;
            inputDevice.SysRealtimeMessageReceived += InputDevice_SysRealtimeMessageReceived;
            inputDevice.SysExMessageReceived += InputDevice_SysExMessageReceived;

            inputDevice.StartRecording();
        }

        private void InputDevice_SysExMessageReceived(object sender, SysExMessageEventArgs e)
        {
            //MessagesTextBox.Text += $"SysEX: {e.Message.MessageType}\n";
        }

        private void InputDevice_SysRealtimeMessageReceived(object sender, SysRealtimeMessageEventArgs e)
        {
            //MessagesTextBox.Text += $"SysRealtime: {e.Message.Message}\n";
        }

        private void InputDevice_ChannelMessageReceived(object sender, ChannelMessageEventArgs e)
        {
            int bytemask = 255;

            int StatusByte = (e.Message.Message & (bytemask << 0));
            int DataByte1  = (e.Message.Message & (bytemask << 8)) >> 8;
            int DataByte2  = (e.Message.Message & (bytemask << 16)) >> 16;

            bool OnOff = Convert.ToBoolean(StatusByte & (1 << 4));

            ChannelMessagesCount++;
            messages.Text = 
                $"Messages Count:       {ChannelMessagesCount}\n" +
                $"Last message (dec):   {e.Message.Message}\n" +
                $"Last message (bin):   {Convert.ToString(e.Message.Message, 2).PadLeft(32, '0')}\n" +
                $"Data Byte 0:          {Convert.ToString(StatusByte, 2).PadLeft(8, '0')}\n" +
                $"Data Byte 1:          {Convert.ToString(DataByte1, 2).PadLeft(8, '0')}\n" +
                $"Data Byte 2:          {Convert.ToString(DataByte2, 2).PadLeft(8, '0')}\n" +
                $"\n" +
                $"Note {DataByte1} ({IntToNote(DataByte1)}) {(OnOff ? "On" : "Off")}\n" +
                $"Note Velocity: {DataByte2}\n" +
                $"\n";
        }

        private void InputDevice_SysCommonMessageReceived(object sender, SysCommonMessageEventArgs e)
        {
            //MessagesTextBox.Text += $"Sys Common: {e.Message.Message}\n";
        }

        private void UpdateText()
        {
            messages.Text = string.Join("\nChannel: ", ChannelMessages);
        }
        
        public static string IntToNote(int note)
        {
            Notes _note = (Notes)(note % 12);
            int octave = note / 12;

            string result = "";

            switch (_note)
            {
                case Notes.C:
                    result = "C";
                    break;
                case Notes.CSharp:
                    result = "C#";
                    break;
                case Notes.D:
                    result = "D";
                    break;
                case Notes.DSharp:
                    result = "D#";
                    break;
                case Notes.E:
                    result = "E";
                    break;
                case Notes.F:
                    result = "F";
                    break;
                case Notes.FSharp:
                    result = "F#";
                    break;
                case Notes.G:
                    result = "G";
                    break;
                case Notes.GSharp:
                    result = "G#";
                    break;
                case Notes.A:
                    result = "A";
                    break;
                case Notes.ASharp:
                    result = "A#";
                    break;
                case Notes.B:
                    result = "B";
                    break;
                default:
                    break;
            }

            result += $"{octave}";

            return result;
        }

        public void Close()
        {
            inputDevice.StopRecording();
            inputDevice.Close();
        }
    }

    public enum Notes
    {
        C,
        CSharp,
        D,
        DSharp,
        E,
        F,
        FSharp,
        G,
        GSharp,
        A,
        ASharp,
        B
    }
}
